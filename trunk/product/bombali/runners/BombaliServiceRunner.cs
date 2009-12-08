namespace bombali.runners
{
    using System.Collections.Generic;
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

        public BombaliServiceRunner(IEnumerable<IPersistenceStore> persistence_stores)
        {
            this.persistence_stores = new List<IPersistenceStore>(persistence_stores);
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
                }
            }
        }

        private void runner_messages_received(object sender, MessageListEventArgs e)
        {
            IEnumerable<MailMessageExtended> messages = e.Messages;
            foreach (MailMessageExtended message in messages)
            {
                Log.bound_to(this).Info("{0} received a message from {1}. Responding that service is still running.",ApplicationParameters.name,message.From.Address);
                SendNotification.from(BombaliConfiguration.settings.email_from).to(message.From.Address).with_subject(
                "Bombali Received Message").with_message("Bombali is up and running.").and_use_notification_host(BombaliConfiguration.settings.smtp_host);
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