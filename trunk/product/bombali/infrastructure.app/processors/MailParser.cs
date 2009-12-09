namespace bombali.infrastructure.app.processors
{
    using sidepop.infrastructure.extensions;
    using sidepop.Mail;

    internal class MailParser : IMailParser
    {
        public MailQueryType parse(SidePOPMailMessage message)
        {
            MailQueryType query_type = MailQueryType.Help;
            string subject_and_body = message.Subject + "|" + message.Body;

            if (message_contains_status(subject_and_body)) query_type = MailQueryType.Status;
            if (message_contains_config(subject_and_body)) query_type = MailQueryType.Configuration;
            if (message_contains_down(subject_and_body)) query_type = MailQueryType.CurrentDownItems;

            return query_type;
        }

        private static bool message_contains_status(string message)
        {
            return message.to_lower().Contains("status");
        }

        private static bool message_contains_config(string message)
        {
            return message.to_lower().Contains("config");
        }

        private static bool message_contains_down(string message)
        {
            return message.to_lower().Contains("down");
        }
    }
}