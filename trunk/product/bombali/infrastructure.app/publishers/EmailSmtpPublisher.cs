using System;
using System.Net.Mail;
using bombali.infrastructure.logging;

namespace bombali.infrastructure.app.publishers
{
	public class EmailSmtpPublisher : IPublisher
	{
		private readonly string smtp_host;
		private readonly string from;
		private readonly string to;
		private readonly string subject;

		public EmailSmtpPublisher(string smtp_host,string from, string to, string subject)
		{
			this.smtp_host = smtp_host;
			this.from = from;
			this.to = to;
			this.subject = subject;
		}

		public void publish(string message)
		{
			MailMessage message_to_send = new MailMessage {From = new MailAddress(from)};
			message_to_send.To.Add(to);
			message_to_send.Subject = subject;
			message_to_send.Body = message;

			Log.bound_to(this).Info("Sending email to {0} with subject \"{1}\" and message:{2}{3}.", to, subject, Environment.NewLine, message);

			SmtpClient smtp_client = new SmtpClient(smtp_host);
			smtp_client.Send(message_to_send);
		}
	}
}