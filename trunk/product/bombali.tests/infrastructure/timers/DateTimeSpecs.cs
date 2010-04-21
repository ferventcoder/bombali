using bdddoc.core;
using developwithpassion.bdd.contexts;
using developwithpassion.bdd.mbunit;
using developwithpassion.bdd.mbunit.standard;
using developwithpassion.bdd.mbunit.standard.observations;

namespace bombali.tests.infrastructure.timers
{
    using System;

    public class DateTimeSpecs
    {
        public abstract class concern_for_DateTime : observations_for_a_static_sut
        {
        }

        [Concern(typeof(DateTime))]
        public class when_DateTime_is_asked_for_the_hour : concern_for_DateTime
        {
            protected static int result;
            protected static DateTime test_date;

            context c = () =>
                            {
                                test_date = new DateTime(2010, 04, 22, 21, 05, 35);
                            };

            private because b = () => result = test_date.Hour;

            [Observation]
            public void should_use_the_24_hour_timing_convention()
            {
                result.should_be_equal_to(21);
            }
        }
    }
}