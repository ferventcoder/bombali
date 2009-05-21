namespace bombali.infrastructure.notifications
{
    public interface INotification
    {
        void send_notification(string notification_host, string from, string to, string subject, string message);
    }
}