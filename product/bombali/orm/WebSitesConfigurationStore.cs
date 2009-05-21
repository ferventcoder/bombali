namespace bombali.orm
{
    using System.Collections.Generic;
    using domain;
    using infrastructure.app.settings;
    using infrastructure.mapping;

    public class WebSitesConfigurationStore : IPersistenceStore
    {
        public IEnumerable<IMonitor> map_to_monitors()
        {
            foreach(WebsiteConfigurationElement item in BombaliConfiguration.settings.sites_to_monitor)
            {
                yield return Map.from(item).to<IMonitor>();
            }
        }
    }
}