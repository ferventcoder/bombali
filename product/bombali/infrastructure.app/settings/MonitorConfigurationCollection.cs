namespace bombali.infrastructure.app.settings
{
    using System.Configuration;

    /// <summary>
    /// A collection of monitor elements
    /// </summary>
    [ConfigurationCollection(typeof(MonitorConfigurationElement))]
    public class MonitorConfigurationCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new Monitor element
        /// </summary>
        /// <returns>A new Monitor Entry</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MonitorConfigurationElement();
        }

        /// <summary>
        /// Gets a particular element 
        /// </summary>
        /// <param name="element">A Monitor  element</param>
        /// <returns>The item as a Monitor</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }

        /// <summary>
        /// A monitor entry
        /// </summary>
        /// <param name="index">The index of the item</param>
        /// <returns>An item at the index</returns>
        public MonitorConfigurationElement Item(int index)
        {
            return (MonitorConfigurationElement)(base.BaseGet(index));
        }

    }
}