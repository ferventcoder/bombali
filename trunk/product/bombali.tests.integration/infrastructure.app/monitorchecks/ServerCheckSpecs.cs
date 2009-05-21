namespace bombali.tests.integration.infrastructure.app.monitorchecks
{
    using bdddoc.core;
    using bombali.infrastructure.app.monitorchecks;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;

    public class ServerCheckSpecs
    {
        public abstract class concern_for_server_check : observations_for_a_sut_with_a_contract<ICheck, ServerCheck>
        {
            protected static bool result;
            protected static string good_server_address = "www.yahoo.com";

            protected static string bad_server_address = "bob";
        }

        [Concern(typeof(ServerCheck))]
        public class when_server_check_is_asked_to_reach_a_good_server : concern_for_server_check
        {
            because b = () => result = sut.run_check(good_server_address);

            [Observation]
            public void should_reach_the_server_successfully()
            {
                result.should_be_true();
            }

            [Observation]
            public void should_set_last_response_to_Success()
            {
                sut.last_response.should_be_equal_to("Success");
            }
        }

        [Concern(typeof(ServerCheck))]
        public class when_server_check_is_asked_to_reach_a_bad_server : concern_for_server_check
        {
            because b = () => result = sut.run_check(bad_server_address);

            [Observation]
            public void should_not_reach_the_server_successfully()
            {
                result.should_be_false();
            }

            [Observation]
            public void should_set_last_response_to_DestinationUnreachable()
            {
                sut.last_response.should_be_equal_to("DestinationUnreachable");
            }
        }
    }
}