namespace bombali.infrastructure.app.mapping
{
    using System;
    using domain;
    using infrastructure.mapping;
    using monitorchecks;
    using settings;
    using timers;

    public class MapFromServerConfigurationElementToIMonitor : IMapper<ServerConfigurationElement, IMonitor>
    {
        public IMonitor map_from(ServerConfigurationElement from)
        {
            string emails_to = Map.from(from.emails_to_send_to).to<String>();
            if (string.IsNullOrEmpty(emails_to))
            {
                emails_to = Map.from(BombaliConfiguration.settings.emails_to).to<String>();
            }

            return new Monitor(
                string.IsNullOrEmpty(from.name) ? from.server_address : from.name,
                from.server_address,
                from.minutes_between_checks,
                emails_to,
                BombaliConfiguration.settings.email_from,
                BombaliConfiguration.settings.smtp_host,
                new ServerCheck(),
                new DefaultTimer(from.minutes_between_checks)
                );
        }
    }
}