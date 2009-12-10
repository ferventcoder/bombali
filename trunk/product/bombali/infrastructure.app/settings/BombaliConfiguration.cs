namespace bombali.infrastructure.app.settings
{
    using System.Configuration;

    /// <summary>
    /// Configuration Handler for Bombali
    /// </summary>
    public sealed class BombaliConfiguration : ConfigurationSection
    {
        static readonly BombaliConfiguration _settings =
            ConfigurationManager.GetSection("bombali") as BombaliConfiguration;

        /// <summary>
        /// Settings section
        /// </summary>
        public static BombaliConfiguration settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Configuration section of items to monitor
        /// </summary>
        [ConfigurationProperty("itemsToMonitor", IsRequired = false, IsDefaultCollection = true)]
        public MonitorConfigurationCollection items_to_monitor
        {
            get { return (MonitorConfigurationCollection)this["itemsToMonitor"]; }
        }

        /// <summary>
        /// Configuration section of default emails to mail to
        /// </summary>
        [ConfigurationProperty("defaultEmailsTo", IsRequired = false, IsDefaultCollection = true)]
        public EmailConfigurationCollection emails_to
        {
            get { return (EmailConfigurationCollection)this["defaultEmailsTo"]; }
        }

        /// <summary>
        /// Who will the email be coming from
        /// </summary>
        [ConfigurationProperty("emailFrom", IsRequired = true)]
        public string email_from
        {
            get { return (string)this["emailFrom"]; }
        }

        /// <summary>
        /// The smtp Host server
        /// </summary>
        [ConfigurationProperty("smtpHost", IsRequired = true)]
        public string smtp_host
        {
            get { return (string)this["smtpHost"]; }
        }

        /// <summary>
        /// The email of the administrator
        /// </summary>
        [ConfigurationProperty("administratorEmail", IsRequired = true)]
        public string administrator_email
        {
            get { return (string)this["administratorEmail"]; }
        }

    }
}