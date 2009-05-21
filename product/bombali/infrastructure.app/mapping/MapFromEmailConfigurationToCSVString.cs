namespace bombali.infrastructure.app.mapping
{
    using System;
    using System.Text;
    using infrastructure.mapping;
    using settings;

    public class MapFromEmailConfigurationToCSVString : IMapper<EmailConfigurationCollection, String>
    {
        public string map_from(EmailConfigurationCollection from)
        {
            if (from != null && from.Count >= 1)
            {
                StringBuilder emails_to = new StringBuilder();
                foreach (EmailConfigurationElement email_to in from)
                {
                    emails_to.AppendFormat("{0},", email_to.email);
                }
                return emails_to.Remove(emails_to.Length - 1, 1).ToString();
            }
            return string.Empty;
        }
    }
}