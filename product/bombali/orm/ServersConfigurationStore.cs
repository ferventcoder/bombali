namespace bombali.orm
{
    using System.Collections.Generic;
    using domain;
    using infrastructure.app.settings;
    using infrastructure.mapping;

    public class ServersConfigurationStore : IPersistenceStore
    {
        public IEnumerable<IMonitor> map_to_monitors()
        {
            foreach (ServerConfigurationElement item in BombaliConfiguration.settings.servers_to_monitor)
            {
                yield return Map.from(item).to<IMonitor>();
            }
        }
    }
}