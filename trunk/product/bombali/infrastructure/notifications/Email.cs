namespace bombali.infrastructure.notifications
{
    using System;
    using System.Net.Mail;
    using logging;

    public class Email : INotification
    {
        public void send_notification(string notification_host, string from, string to, string subject, string message)
        {
            MailMessage message_to_send = new MailMessage();
            message_to_send.From = new MailAddress(from);
            message_to_send.To.Add(to);
            message_to_send.Subject = subject;
            message_to_send.Body = message;

            Log.bound_to(this).Info("Sending email to {0} with subject \"{1}\" and message:{2}{3}.",to,subject,Environment.NewLine,message);

            SmtpClient smtp_client = new SmtpClient(notification_host);
            smtp_client.Send(message_to_send);
        }

    }
}