namespace bombali.tests.integration.infrastructure.app.monitorchecks
{
    using bdddoc.core;
    using bombali.infrastructure.app.monitorchecks;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;

    public class WebsiteCheckSpecs
    {
        public abstract class concern_for_websitecheck : observations_for_a_sut_with_a_contract<ICheck, WebsiteCheck>
        {
            protected static bool result;
            protected static string good_url = "http://www.yahoo.com";

            protected static string bad_url =
                "http://www.aldkjaksdfjaksjfaksdfljaksdfjaljdsflaksjdoesntexist.com/www/wontwothreefourm/alskdfjalskdjfalskdjfalskfjd";
        }

        [Concern(typeof(WebsiteCheck))]
        public class when_websitecheck_is_asked_to_run_against_a_good_site : concern_for_websitecheck
        {
            because b = () => result = sut.run_check(good_url);

            [Observation]
            public void should_successfully_connect_to_a_website()
            {
                result.should_be_true();
            }

            [Observation]
            public void should_set_last_response_to_OK()
            {
                sut.last_response.should_be_equal_to("OK");
            }
        }

        [Concern(typeof(WebsiteCheck))]
        public class when_websitecheck_is_asked_to_run_against_a_bad_site : concern_for_websitecheck
        {
            because b = () => result = sut.run_check(bad_url);

            [Observation]
            public void should_not_successfully_connect_to_a_website()
            {
                result.should_be_false();
            }

            [Observation]
            public void should_set_last_response_to_NotFound()
            {
                sut.last_response.should_be_equal_to("NotFound");
            }
        }
    }
}