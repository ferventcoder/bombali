namespace bombali.infrastructure.app.settings
{
    using System.Configuration;

    /// <summary>
    /// A server to monitor
    /// </summary>
    public sealed class ServerConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// The name of the item
        /// </summary>
        [ConfigurationProperty("name", IsRequired = false)]
        public string name
        {
            get { return (string)this["name"]; }
        }

        /// <summary>
        /// The description of the item
        /// </summary>
        [ConfigurationProperty("description", IsRequired = false)]
        public string description
        {
            get { return (string)this["description"]; }
        }

        /// <summary>
        /// Whether the item is enabled or not
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool enabled
        {
            get { return (bool)this["enabled"]; }
        }

        /// <summary>
        /// How many minutes between checks?
        /// </summary>
        [ConfigurationProperty("minutesBetweenChecks", IsRequired = false, DefaultValue = 30d)]
        public double minutes_between_checks
        {
            get { return (double)this["minutesBetweenChecks"]; }
        }

        /// <summary>
        /// What is the server address?
        /// </summary>
        [ConfigurationProperty("server", IsRequired = true)]
        public string server_address
        {
            get { return (string)this["server"]; }
        }

        /// <summary>
        /// Emails to send to
        /// </summary>
        [ConfigurationProperty("emails", IsRequired = false, IsDefaultCollection = true)]
        public EmailConfigurationCollection emails_to_send_to
        {
            get { return (EmailConfigurationCollection)this["emails"]; }
        }
    }
}