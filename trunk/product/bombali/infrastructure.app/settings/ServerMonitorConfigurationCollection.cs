namespace bombali.infrastructure.app.settings
{
    using System.Configuration;

    /// <summary>
    /// A collection of sites to monitor elements
    /// </summary>
    [ConfigurationCollection(typeof(ServerConfigurationElement))]
    public class ServerMonitorConfigurationCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new WebsiteEntry element
        /// </summary>
        /// <returns>A new WebsiteEntry</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerConfigurationElement();
        }

        /// <summary>
        /// Gets a particular element 
        /// </summary>
        /// <param name="element">A WebsiteEntry element</param>
        /// <returns>The item as a WebsiteEntry</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }

        /// <summary>
        /// A website entry
        /// </summary>
        /// <param name="index">The index of the item</param>
        /// <returns>An item at the index</returns>
        public ServerConfigurationElement Item(int index)
        {
            return (ServerConfigurationElement)(base.BaseGet(index));
        }

    }
}