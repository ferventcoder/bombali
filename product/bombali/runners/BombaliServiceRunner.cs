namespace bombali.runners
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Timers;
    using domain;
    using infrastructure;
    using infrastructure.app.processors;
    using infrastructure.app.settings;
    using infrastructure.data.accessors;
    using infrastructure.logging;
    using infrastructure.mapping;
    using infrastructure.notifications;
    using infrastructure.timers;
    using orm;
    using sidepop.infrastructure.extensions;
    using sidepop.Mail;
    using sidepop.message.events;
    using sidepop.runners;
    using Email = domain.Email;
    using Version = infrastructure.information.Version;

    public class BombaliServiceRunner : IRunner
    {
        private readonly IMailParser mail_processor;
        private readonly IRepository repository;
        private readonly IList<IPersistenceStore> persistence_stores;
        private readonly Stopwatch up_time;
        private IList<IMonitor> monitors;
        private IDictionary<string, ApprovalType> authorization_dictionary;
        private readonly IDictionary<string, string> subscribers = new Dictionary<string, string>();

        public BombaliServiceRunner(IEnumerable<IPersistenceStore> persistence_stores, IMailParser mail_processor, IRepository repository)
        {
            this.mail_processor = mail_processor;
            this.repository = repository;
            this.persistence_stores = new List<IPersistenceStore>(persistence_stores);
            up_time = new Stopwatch();
            up_time.Start();
        }

        public void run_the_application()
        {
            configure_mail_watcher();
            start_monitoring();
            set_up_authorization_dictionary();
            configure_subscriber_timer();
        }

        private void set_up_authorization_dictionary()
        {
            Log.bound_to(this).Info("{0} is setting up the authorized users list.", ApplicationParameters.name);
            authorization_dictionary = new Dictionary<string, ApprovalType>();
            authorization_dictionary.Add(BombaliConfiguration.settings.administrator_email.to_lower(), ApprovalType.Approved);
            Log.bound_to(this).Debug("{0} added {1} to the authorized users list.", ApplicationParameters.name,
                                     BombaliConfiguration.settings.administrator_email);
            foreach (IMonitor monitor in monitors)
            {
                foreach (string email_address in monitor.who_to_notify_as_comma_separated_list.Split(','))
                {
                    if (!authorization_dictionary.ContainsKey(email_address.to_lower()))
                    {
                        authorization_dictionary.Add(email_address.to_lower(), ApprovalType.Approved);
                        Log.bound_to(this).Debug("{0} added {1} to the authorized users list.", ApplicationParameters.name, email_address.to_lower());
                    }
                }
            }
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
            try
            {
                IEnumerable<SidePOPMailMessage> messages = e.Messages;

                foreach (SidePOPMailMessage message in messages)
                {
                    Email mail_message = Map.from(message).to<Email>();
                    parse_and_send_response(mail_message);
                    save_email_message(mail_message);
                }
            }
            catch (Exception ex)
            {
                Log.bound_to(this).Error("{0} service had an error processing messages on {1} (with user {2}):{3}{4}", ApplicationParameters.name,
                                         Environment.MachineName, Environment.UserName,
                                         Environment.NewLine, ex.ToString());
            }
        }

        private void save_email_message(Email mail_message)
        {
            Log.bound_to(this).Info("{0} is archiving message \"{1}\".", ApplicationParameters.name, mail_message.message_id);
            repository.save_or_update(mail_message);
        }

        private void parse_and_send_response(Email mail_message)
        {
            string respond_to = mail_message.from_address.to_lower();
            MailQueryType query_type = mail_processor.parse(mail_message, monitors, authorization_dictionary);
            Log.bound_to(this).Info("{0} received a message from {1} of type {2}.", ApplicationParameters.name, respond_to, query_type.ToString());

            string response_text = string.Empty;

            if (query_type == MailQueryType.Authorized || query_type == MailQueryType.Denied)
            {
                string[] body_words = mail_message.message_body.Split(' ');
                foreach (string body_word in body_words)
                {
                    if (body_word.Contains("@"))
                    {
                        respond_to = body_word.Replace(Environment.NewLine, "");
                        break;
                    }
                }
            }

            switch (query_type)
            {
                case MailQueryType.Denied:
                    authorization_dictionary.Add(respond_to, ApprovalType.Denied);
                    return;
                    break;
                case MailQueryType.Authorized:
                    if (!authorization_dictionary.ContainsKey(respond_to.to_lower()))
                    {
                        authorization_dictionary.Add(respond_to.to_lower(), ApprovalType.Approved);
                    }
                    response_text = string.Format("Congratulations - you have been approved!{0}Send 'help' for options", Environment.NewLine);
                    break;
                case MailQueryType.Help:
                    response_text =
                        string.Format(
                            "Options- send 1:{0}help{0}status{0}config{0}down - errors{0}version{0}sub{0}unsub",
                            Environment.NewLine);
                    break;
                case MailQueryType.Status:
                    TimeSpan up_time_current = up_time.Elapsed;
                    response_text = string.Format("{0} has been up and running for {1} days {2} hours {3} minutes and {4} seconds.", ApplicationParameters.name,
                                                  up_time_current.Days, up_time_current.Hours, up_time_current.Minutes, up_time_current.Seconds);
                    break;
                case MailQueryType.CurrentDownItems:
                    response_text = string.Format("Services currently down:{0}", Environment.NewLine);
                    foreach (IMonitor monitor in monitors)
                    {
                        if (monitor.who_to_notify_as_comma_separated_list.to_lower().Contains(respond_to))
                        {
                            if (!monitor.status_is_good) response_text += string.Format("{0}{1}", monitor.name, Environment.NewLine);
                        }
                    }
                    break;
                case MailQueryType.Authorizing:
                    response_text =
                        string.Format("Bombali notified admin to add you to approved list. If you are added, you will receive a response.");
                    break;
                case MailQueryType.Version:
                    response_text = string.Format("Bombali is currently running version {0}.", Version.get_version());
                    break;
                case MailQueryType.Subscribe:
                    if (!subscribers.ContainsKey(respond_to.to_lower())) subscribers.Add(respond_to.to_lower(), respond_to);
                    response_text = string.Format("You are now subscribed to receive daily status messages");
                    break;
                case MailQueryType.Unsubscribe:
                    if (subscribers.ContainsKey(respond_to.to_lower())) subscribers.Remove(respond_to.to_lower());
                    response_text = string.Format("You are now unsubscribed from daily status messages");
                    break;
                default:
                    response_text = string.Format("{0} has not been implemented yet. Please watch for updates.", query_type);
                    break;
            }

            send_notification(respond_to, response_text);

            if (query_type == MailQueryType.Authorizing)
            {
                send_notification(BombaliConfiguration.settings.administrator_email,
                                  string.Format("{0} reqests approval. Send approve/deny w/email address. Ex. 'deny bob@nowhere.com'", respond_to));
            }
        }

        private static void send_notification(string send_to, string message_text)
        {
            SendNotification
                .from(BombaliConfiguration.settings.email_from)
                .to(send_to)
                .with_subject("Bombali")
                .with_message(message_text)
                .and_use_notification_host(BombaliConfiguration.settings.smtp_host);
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

        #region Subscribers Notice

        private DefaultTimer subscriber_timer;
        private bool status_message_sent;
        private const int status_message_hour = 21;
        private const int subscriber_timer_interval_in_minutes = 15;

        private void configure_subscriber_timer()
        {
            subscriber_timer = new DefaultTimer(subscriber_timer_interval_in_minutes);
            subscriber_timer.Elapsed += subscriber_timer_Elapsed;
            subscriber_timer.start();
        }

        private void subscriber_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            subscriber_timer.stop();

            int current_hour = DateTime.Now.Hour;
            if (current_hour != status_message_hour) status_message_sent = false;

            if (current_hour == status_message_hour && !status_message_sent)
            {
                foreach (KeyValuePair<string, string> subscriber in subscribers)
                {
                    TimeSpan up_time_current = up_time.Elapsed;
                    string message_text = string.Format("{0} has been up and running for {1} days {2} hours {3} minutes and {4} seconds.",
                                                        ApplicationParameters.name,
                                                        up_time_current.Days, up_time_current.Hours, up_time_current.Minutes, up_time_current.Seconds);
                    send_notification(subscriber.Value, message_text);
                }
                status_message_sent = true;
            }

            subscriber_timer.start();
        }

        #endregion
    }
}