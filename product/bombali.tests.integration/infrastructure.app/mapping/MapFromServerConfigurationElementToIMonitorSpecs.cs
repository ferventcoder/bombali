namespace bombali.tests.integration.infrastructure.app.mapping
{
    using System;
    using bdddoc.core;
    using bombali.infrastructure.app.mapping;
    using bombali.infrastructure.app.settings;
    using bombali.infrastructure.containers;
    using bombali.infrastructure.mapping;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using domain;
    using Rhino.Mocks;

    public class MapFromServerConfigurationElementToIMonitorSpecs
    {
        public abstract class concern_for_map_from_server_configuration_to_monitor : observations_for_a_sut_with_a_contract<IMapper<ServerConfigurationElement, IMonitor>, MapFromServerConfigurationElementToIMonitor>
        {
            protected static IMonitor result;
            protected static IContainer the_container;
            protected static MapFromEmailConfigurationToCSVString csv_mapper;

            context c = () =>
                {
                    csv_mapper = new MapFromEmailConfigurationToCSVString();
                    the_container = an<IContainer>();
                    Container.initialize_with(the_container);
                    the_container.Stub(x => x.Resolve<IMapper<EmailConfigurationCollection, String>>()).Return(csv_mapper);
                };
        }

        [Concern(typeof(MapFromServerConfigurationElementToIMonitor))]
        public class when_a_server_configuration_element_is_mapped_to_monitor : concern_for_map_from_server_configuration_to_monitor
        {
            protected static ServerConfigurationElement input = BombaliConfiguration.settings.servers_to_monitor.Item(0);

            because b = () => result = sut.map_from(input);

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
                result.what_to_check.should_be_equal_to(input.server_address);
            }

            [Observation]
            public void should_map_interval_correctly()
            {
                result.interval_in_minutes_for_check.should_be_equal_to(input.minutes_between_checks);
            }

        }

        [Concern(typeof(MapFromServerConfigurationElementToIMonitor))]
        public class when_a_server_configuration_element_is_mapped_to_monitor_and_configuration_element_has_no_name : concern_for_map_from_server_configuration_to_monitor
        {
            protected static ServerConfigurationElement input = BombaliConfiguration.settings.servers_to_monitor.Item(1);

            because b = () => result = sut.map_from(input);

            [Observation]
            public void should_map_successfully()
            {
                result.should_not_be_null();
            }

            [Observation]
            public void should_map_name_to_server_when_name_is_empty()
            {
                result.name.should_be_equal_to(input.server_address);
            }

        }

        [Concern(typeof(MapFromServerConfigurationElementToIMonitor))]
        public class when_a_server_configuration_element_is_mapped_to_monitor_and_configuration_element_has_no_emails_to : concern_for_map_from_server_configuration_to_monitor
        {
            protected static ServerConfigurationElement input = BombaliConfiguration.settings.servers_to_monitor.Item(1);

            because b = () => result = sut.map_from(input);

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