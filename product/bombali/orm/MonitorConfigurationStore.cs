namespace bombali.orm
{
    using System.Collections.Generic;
    using domain;
    using infrastructure.app.settings;
    using infrastructure.mapping;

    public class MonitorConfigurationStore : IPersistenceStore
    {
        public IEnumerable<IMonitor> map_to_monitors()
        {
            foreach (MonitorConfigurationElement item in BombaliConfiguration.settings.items_to_monitor)
            {
                yield return Map.from(item).to<IMonitor>();
            }
        }
    }
}