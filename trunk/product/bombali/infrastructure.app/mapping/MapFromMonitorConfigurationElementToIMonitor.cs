namespace bombali.infrastructure.app.mapping
{
    using System;
    using domain;
    using infrastructure.mapping;
    using monitorchecks;
    using settings;
    using timers;
    using bombali.infrastructure.containers;

    public class MapFromMonitorConfigurationElementToIMonitor : IMapper<MonitorConfigurationElement, IMonitor>
    {
        public IMonitor map_from(MonitorConfigurationElement from)
        {
            string emails_to = Map.from(from.emails_to_send_to).to<String>();
            if (string.IsNullOrEmpty(emails_to))
            {
                emails_to = Map.from(BombaliConfiguration.settings.emails_to).to<String>();
            }

            //Type generic = typeof(Container.get_an_instance_of<>);
            //Type specific = generic.MakeGenericType(typeof(from.system_type));
            //ConstructorInfo ci = specific.GetConstructor(new Type[] { });
            //object o = ci.Invoke(new object[] { });

            //Container.get_an_instance_of<>();
            //http://www.google.com/search?hl=en&q=passing+runtime+Type+to+Generic&aq=f&oq=&aqi=
            //http://stackoverflow.com/questions/513952/c-specifying-generic-collection-type-param-at-runtime

            Type monitor = Type.GetType(from.system_type);
            //ConstructorInfo ci = monitor.GetConstructor(new Type[] { });
            

            return new Monitor(
                string.IsNullOrEmpty(from.name) ? from.item_to_check : from.name,
                from.item_to_check,
                from.minutes_between_checks,
                emails_to,
                BombaliConfiguration.settings.email_from,
                BombaliConfiguration.settings.smtp_host,
                (ICheck)Container.get_an_instance_of(monitor.UnderlyingSystemType),
                new DefaultTimer(from.minutes_between_checks)
                );
        }
    }
}