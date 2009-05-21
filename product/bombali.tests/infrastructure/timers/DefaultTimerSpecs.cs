namespace bombali.tests.infrastructure.timers
{
    using System;
    using System.Threading;
    using System.Timers;
    using bdddoc.core;
    using bombali.infrastructure.timers;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;

    public class DefaultTimerSpecs
    {
        public abstract class concern_for_default_timer : observations_for_a_sut_with_a_contract<ITimer, DefaultTimer>
        {
        }

        [Concern(typeof(DefaultTimer))]
        public class when_default_timer_is_created : concern_for_default_timer
        {
            protected static object result;
            const double interval_in_minutes = .01d;

            context c = () => provide_a_basic_sut_constructor_argument(interval_in_minutes);

            [Observation]
            public void should_not_have_any_errors()
            {
                //nothing to write here
            }
        }

        [Concern(typeof(DefaultTimer))]
        public class when_default_timer_has_elapsed_its_time : concern_for_default_timer
        {
            protected static object result;
            const double interval_in_minutes = .01d;
            protected static bool was_called;

            context c = () => provide_a_basic_sut_constructor_argument(interval_in_minutes);

            because b = () =>
                {
                    sut.Elapsed += sut_Elapsed;
                    sut.start();
                };

            static void sut_Elapsed(object sender, ElapsedEventArgs e)
            {
                was_called = true;
            }

            [Observation]
            public void should_have_fired_the_elapsed_event()
            {
                Thread.Sleep((int)TimeSpan.FromMinutes(interval_in_minutes).TotalMilliseconds + 800);
                was_called.should_be_true();
            }
        }
    }
}