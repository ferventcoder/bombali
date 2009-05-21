namespace bombali.infrastructure.app.settings
{
    using System.Configuration;

    /// <summary>
    /// A collection of emails to send to for a website monitor
    /// </summary>
    [ConfigurationCollection(typeof(EmailConfigurationCollection))]
    public class EmailConfigurationCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new EmailConfigurationElement element
        /// </summary>
        /// <returns>A new EmailConfigurationElement</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EmailConfigurationElement();
        }

        /// <summary>
        /// Gets a particular element 
        /// </summary>
        /// <param name="element">A EmailConfigurationElement element</param>
        /// <returns>The item as a EmailConfigurationElement</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }

        /// <summary>
        /// A Email Configuration Element
        /// </summary>
        /// <param name="index">The index of the item</param>
        /// <returns>An item at the index</returns>
        public EmailConfigurationElement Item(int index)
        {
            return (EmailConfigurationElement)(base.BaseGet(index));
        }
    }
}