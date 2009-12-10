namespace bombali.infrastructure.app.processors
{
    using System.Collections.Generic;
    using domain;
    using sidepop.infrastructure.extensions;
    using sidepop.Mail;

    public class MailParser : IMailParser
    {
        public MailQueryType parse(Email message, IList<IMonitor> monitors, IDictionary<string, ApprovalType> authorization_dictionary)
        {
            MailQueryType query_type = MailQueryType.Authorizing;

            string user = message.from_address.to_lower();
            bool user_is_authorized = false;

            if (authorization_dictionary.ContainsKey(user))
            {
                ApprovalType user_is_approved = authorization_dictionary[user];
                if (user_is_approved == ApprovalType.Approved)
                {
                    user_is_authorized = true;
                }
            }


            if (user_is_authorized)
            {
                query_type = MailQueryType.Help;
                string subject_and_body = message.subject + "|" + message.message_body;

                if (message_contains_status(subject_and_body)) query_type = MailQueryType.Status;
                if (message_contains_config(subject_and_body)) query_type = MailQueryType.Configuration;
                if (message_contains_down(subject_and_body)) query_type = MailQueryType.CurrentDownItems;
                if (message_contains_approve(subject_and_body)) query_type = MailQueryType.Authorized;
                if (message_contains_deny(subject_and_body)) query_type = MailQueryType.Denied;
                if (message_contains_version(subject_and_body)) query_type = MailQueryType.Version;
            }

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

        private static bool message_contains_approve(string message)
        {
            return message.to_lower().Contains("approve");
        }

        private static bool message_contains_deny(string message)
        {
            return message.to_lower().Contains("deny");
        }

        private static bool message_contains_version(string message)
        {
            return message.to_lower().Contains("version");
        }
    }
}