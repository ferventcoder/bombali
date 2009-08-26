namespace bombali.tests.integration.infrastructure.app.mapping
{
    using System;
    using System.Reflection;
    using bdddoc.core;
    using bombali.infrastructure.app.mapping;
    using bombali.infrastructure.app.monitorchecks;
    using bombali.infrastructure.app.settings;
    using bombali.infrastructure.containers;
    using bombali.infrastructure.mapping;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using domain;
    using Rhino.Mocks;

    public class MapFromMonitorConfigurationElementToIMonitorSpecs
    {
        public abstract class concern_for_map_from_monitor_configuration_to_monitor :
            observations_for_a_sut_with_a_contract<IMapper<MonitorConfigurationElement, IMonitor>, MapFromMonitorConfigurationElementToIMonitor>
        {
            protected static IMonitor result;
            protected static IContainer the_container;
            protected static MapFromEmailConfigurationToCSVString csv_mapper;

            private context c = () =>
                                    {
                                        csv_mapper = new MapFromEmailConfigurationToCSVString();
                                        the_container = an<IContainer>();
                                        Container.initialize_with(the_container);
                                        the_container.Stub(
                                            x => x.Resolve<IMapper<EmailConfigurationCollection, String>>()).Return(
                                            csv_mapper);
                                    };
        }

        [Concern(typeof (MapFromMonitorConfigurationElementToIMonitor))]
        public class when_a_monitor_configuration_element_is_mapped_to_monitor :
            concern_for_map_from_monitor_configuration_to_monitor
        {
            protected static MonitorConfigurationElement input = BombaliConfiguration.settings.items_to_monitor.Item(0);

            private because b = () => result = sut.map_from(input);

            [Observation]
            public void should_map_successfully()
            {
                result.should_not_be_null();
            }

            [Observation]
            public void should_map_name_correctly()
            {
                result.name.should_be_equal_to(input.name);
            }

            [Observation]
            public void should_map_what_to_check_correctly()
            {
                result.what_to_check.should_be_equal_to(input.item_to_check);
            }

            [Observation]
            public void should_map_interval_correctly()
            {
                result.interval_in_minutes_for_check.should_be_equal_to(input.minutes_between_checks);
            }

            [Observation]
            public void should_map_type_correctly()
            {
                Type dude = Type.GetType("bombali.infrastructure.app.monitorchecks.ServerCheck, bombali");
                //ConstructorInfo ci = monitor.GetConstructor(new Type[] { });
                ConstructorInfo ci = dude.GetConstructor(new Type[] {});
                object o = ci.Invoke(new object[] {});
                o.should_be_an<ServerCheck>();
            }
        }

        [Concern(typeof (MapFromMonitorConfigurationElementToIMonitor))]
        public class when_a_monitor_configuration_element_is_mapped_to_monitor_and_configuration_element_has_no_name :
            concern_for_map_from_monitor_configuration_to_monitor
        {
            protected static MonitorConfigurationElement input = BombaliConfiguration.settings.items_to_monitor.Item(1);

            private because b = () => result = sut.map_from(input);

            [Observation]
            public void should_map_successfully()
            {
                result.should_not_be_null();
            }

            [Observation]
            public void should_map_name_to_item_to_check_when_name_is_empty()
            {
                result.name.should_be_equal_to(input.item_to_check);
            }
        }

        [Concern(typeof (MapFromMonitorConfigurationElementToIMonitor))]
        public class when_a_monitor_configuration_element_is_mapped_to_monitor_and_configuration_element_has_no_emails_to :
                concern_for_map_from_monitor_configuration_to_monitor
        {
            protected static MonitorConfigurationElement input = BombaliConfiguration.settings.items_to_monitor.Item(1);

            private because b = () => result = sut.map_from(input);

            [Observation]
            public void should_map_successfully()
            {
                result.should_not_be_null();
            }

            [Observation]
            public void should_use_default_collection_of_emails_to()
            {
                result.who_to_notify_as_comma_separated_list.should_contain("email1@noreply.com");
            }
        }
    }
}