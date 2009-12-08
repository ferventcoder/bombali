namespace bombali.runners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using domain;
    using infrastructure;
    using infrastructure.app.settings;
    using infrastructure.logging;
    using infrastructure.notifications;
    using orm;
    using sidepop.configuration;
    using sidepop.Mail;
    using sidepop.message.events;
    using sidepop.runners;

    public class BombaliServiceRunner : IRunner
    {
        private readonly IList<IPersistenceStore> persistence_stores;
        private readonly Stopwatch up_time;

        public BombaliServiceRunner(IEnumerable<IPersistenceStore> persistence_stores)
        {
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
            foreach (AccountConfigurationElement account in SidePOPConfiguration.settings.accounts)
            {
                if (account.enabled)
                {
                    SidePopRunner runner = new SidePopRunner(new DefaultPop3Client(account.hostName, account.hostPort,
                                                                            account.useSSL, account.userName,
                                                                            account.password), account.minutes_between_checks);
                    runner.MessagesReceived += runner_messages_received;
                    runner.run();
                    Log.bound_to(this).Info("{0} is configured to watch for messages with user {1} at {2} every {3} minutes.", ApplicationParameters.name,
                                            account.userName, account.hostName, account.minutes_between_checks);
                }
            }
        }

        private void runner_messages_received(object sender, MessageListEventArgs e)
        {
            IEnumerable<MailMessageExtended> messages = e.Messages;

            const string subject = "Bombali Response";
            TimeSpan up_time_current = up_time.Elapsed;
            string text_message = string.Format("{0} has been up and running for {1} days {2} hours {3} minutes and {4} seconds.", ApplicationParameters.name, up_time_current.Days, up_time_current.Hours, up_time_current.Minutes, up_time_current.Seconds);

            foreach (MailMessageExtended message in messages)
            {
                Log.bound_to(this).Info("{0} received a message from {1}. Responding that service is still running.",ApplicationParameters.name,message.From.Address);
            
                SendNotification.from(BombaliConfiguration.settings.email_from).to(message.From.Address).with_subject(
                subject).with_message(text_message).and_use_notification_host(BombaliConfiguration.settings.smtp_host);
            }
        }

        private void start_monitoring()
        {
            IList<IMonitor> monitors = new List<IMonitor>();

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