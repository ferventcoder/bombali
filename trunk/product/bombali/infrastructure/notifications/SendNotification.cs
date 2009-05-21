namespace bombali.infrastructure.notifications
{
    public static class SendNotification
    {
        public static NotificationBuilder from(string an_address)
        {
            return new NotificationBuilder(an_address);
        }
    }
}