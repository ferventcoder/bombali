namespace bombali.infrastructure.notifications
{
    using containers;

    public class NotificationBuilder
    {
        readonly string notification_from;
        string notification_host;
        string notification_to;
        string notification_subject;
        string notification_message;

        public NotificationBuilder(string from_email)
        {
            notification_from = from_email;
        }

        public NotificationBuilder to(string a_comma_separated_list_of_email_addresses)
        {
            notification_to = a_comma_separated_list_of_email_addresses;
            return this;
        }

        public NotificationBuilder with_subject(string subject_of_message)
        {
            notification_subject = subject_of_message;
            return this;
        }

        public NotificationBuilder with_message(string message)
        {
            notification_message = message;
            return this;
        }

        public void and_use_notification_host(string host)
        {
            notification_host = host;
            build();
        }

        void build()
        {
            Container.get_an_instance_of<INotification>().send_notification(notification_host, notification_from, notification_to, notification_subject, notification_message);
        }
    }
}