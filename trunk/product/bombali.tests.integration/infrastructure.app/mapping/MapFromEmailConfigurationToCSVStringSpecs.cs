namespace bombali.tests.integration.infrastructure.app.mapping
{
    using System;
    using bdddoc.core;
    using bombali.infrastructure.app.mapping;
    using bombali.infrastructure.app.settings;
    using bombali.infrastructure.mapping;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;

    public class MapFromEmailConfigurationToCSVStringSpecs
    {
        public abstract class concern_for_map_from_email_configuration_to_comma_separated_value :
            observations_for_a_sut_with_a_contract<IMapper<EmailConfigurationCollection, String>, MapFromEmailConfigurationToCSVString>
        {
        }

        [Concern(typeof(MapFromEmailConfigurationToCSVString))]
        public class when_email_configuration_is_mapped_to_a_comma_separated_string :
            concern_for_map_from_email_configuration_to_comma_separated_value
        {
            protected static object result;

            because b = () => result = sut.map_from(BombaliConfiguration.settings.sites_to_monitor.Item(2).emails_to_send_to);

            [Observation]
            public void should_map_successfully()
            {
                result.should_not_be_null();
            }

            [Observation]
            public void should_not_have_a_comma_at_the_end()
            {
                ((string)result).LastIndexOf(',').should_be_less_than(((string)result).Length - 1);
            }

            [Observation]
            public void should_properly_concatenate_values()
            {
                result.should_be_equal_to("someone@overhere.com,someoneelse@overhere.com,someoneotherthanthose@overhere.com");
            }
        }
    }
}