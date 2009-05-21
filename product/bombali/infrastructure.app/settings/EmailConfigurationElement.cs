using System.Configuration;

namespace bombali.infrastructure.app.settings
{
    /// <summary>
    /// An email configuration for a particular site
    /// </summary>
    public class EmailConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// The name of the item
        /// </summary>
        [ConfigurationProperty("name", IsRequired = false)]
        public string name
        {
            get { return (string) this["name"]; }
        }

        /// <summary>
        /// The description of the item
        /// </summary>
        [ConfigurationProperty("description", IsRequired = false)]
        public string description
        {
            get { return (string) this["description"]; }
        }

        /// <summary>
        /// Whether the item is enabled or not
        /// </summary>
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool enabled
        {
            get { return (bool) this["enabled"]; }
        }

        /// <summary>
        /// The email to send to
        /// </summary>
        [ConfigurationProperty("email", IsRequired = true)]
        public string email
        {
            get { return (string) this["email"]; }
        }
    }
}