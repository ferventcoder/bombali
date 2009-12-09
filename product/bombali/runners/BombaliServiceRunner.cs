namespace bombali.runners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using domain;
    using infrastructure;
    using infrastructure.app.processors;
    using infrastructure.app.settings;
    using infrastructure.logging;
    using infrastructure.notifications;
    using orm;
    using sidepop.Mail;
    using sidepop.message.events;
    using sidepop.runners;

    public class BombaliServiceRunner : IRunner
    {
        private readonly IMailParser mail_processor;
        private readonly IList<IPersistenceStore> persistence_stores;
        private readonly Stopwatch up_time;
        private IList<IMonitor> monitors;

        public BombaliServiceRunner(IEnumerable<IPersistenceStore> persistence_stores, IMailParser mail_processor)
        {
            this.mail_processor = mail_processor;
            this.persistence_stores = new List<IPersistenceStore>(persistence_stores);
            up_time = new Stopwatch();
            up_time.Start();
        }

        public void run_the_application()
        {
            configure_mail_watcher();
            start_monitoring();
        }

        private void configure_mail_watcher()
        {
            EmailWatcherConfigurator configurator = new SidePopXmlConfigurator();
            foreach (EmailWatcher emailWatcher in configurator.configure())
            {
                emailWatcher.MessagesReceived += runner_messages_received;
                emailWatcher.start();
            }
        }

        private void runner_messages_received(object sender, MessageListEventArgs e)
        {
            IEnumerable<SidePOPMailMessage> messages = e.Messages;

            const string subject = "Bombali Response";
            
            foreach (SidePOPMailMessage message in messages)
            {
                MailQueryType query_type = mail_processor.parse(message);
                Log.bound_to(this).Info("{0} received a message from {1} of type {2}.", ApplicationParameters.name, message.From.Address, query_type.ToString());

                string response_text = string.Empty;

                switch (query_type)
                {
                    case MailQueryType.Help:
                        response_text = string.Format("Options - send one:{0} help - this menu{0} status - up time{0} config - all monitors{0} down - current monitors in error{0}", Environment.NewLine);
                        break;
                    case MailQueryType.Status:
                        TimeSpan up_time_current = up_time.Elapsed;
                        response_text = string.Format("{0} has been up and running for {1} days {2} hours {3} minutes and {4} seconds, thank you very much.", ApplicationParameters.name, up_time_current.Days, up_time_current.Hours, up_time_current.Minutes, up_time_current.Seconds);
                        break;
                    case MailQueryType.CurrentDownItems:
                        response_text = string.Format("Services currently down:{0}", Environment.NewLine);
                        foreach (IMonitor monitor in monitors)
                        {
                            if (!monitor.status_is_good) response_text += string.Format("{0}{1}", monitor.name, Environment.NewLine);
                        }
                        break;
                    default:
                        response_text = string.Format("{0} has not been implemented yet. Please watch for updates.",query_type.ToString());
                        break;
                }

                SendNotification.from(BombaliConfiguration.settings.email_from).to(message.From.Address).with_subject(
                subject).with_message(response_text).and_use_notification_host(BombaliConfiguration.settings.smtp_host);
            }
        }

        private void start_monitoring()
        {
            monitors = new List<IMonitor>();

            foreach (IPersistenceStore persistence_store in persistence_stores)
            {
                foreach (IMonitor monitor in persistence_store.map_to_monitors()) monitors.Add(monitor);
            }

            foreach (IMonitor monitor in monitors)
            {
                //new Thread(() => 
                //    { 
                if (monitor != null) monitor.start_monitoring();
                //}
                //).Start();
            }
        }
    }
}