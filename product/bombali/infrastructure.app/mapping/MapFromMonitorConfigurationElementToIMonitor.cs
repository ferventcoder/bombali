namespace bombali.infrastructure.app.mapping
{
    using System;
    using domain;
    using infrastructure.mapping;
    using monitorchecks;
    using resolvers;
    using settings;
    using timers;

    public class MapFromMonitorConfigurationElementToIMonitor : IMapper<MonitorConfigurationElement, IMonitor>
    {
        public IMonitor map_from(MonitorConfigurationElement from)
        {
            string emails_to = Map.from(from.emails_to_send_to).to<String>();
            if (string.IsNullOrEmpty(emails_to))
            {
                emails_to = Map.from(BombaliConfiguration.settings.emails_to).to<String>();
            }


            return new Monitor(
                string.IsNullOrEmpty(from.name) ? from.item_to_check : from.name,
                from.item_to_check,
                from.minutes_between_checks,
                emails_to,
                BombaliConfiguration.settings.email_from,
                BombaliConfiguration.settings.smtp_host,
                resolve_check_utility(from.system_type),
                new DefaultTimer(from.minutes_between_checks)
                );
        }

        public ICheck resolve_check_utility(string system_type)
        {  
            ICheck check_utility;
           
            object temp = DefaultInstanceCreator.create_object_from_string_type(system_type);
            if (temp is ICheck)
            {
                check_utility = (ICheck)temp;

            } else
            {
                check_utility = new WebsiteCheck();
            }
            
            return check_utility;
        }
    }
}