namespace bombali.tests.infrastructure.logging.custom
{
    using bdddoc.core;
    using bombali.infrastructure.logging;
    using bombali.infrastructure.logging.custom;
    using developwithpassion.bdd;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using Rhino.Mocks;

    public class Log4NetLoggerSpecs
    {
        public abstract class concern_for_Log4NetLogger : observations_for_a_sut_with_a_contract<ILog, Log4NetLogger>
        {
        }

        [Concern(typeof(Log4NetLogger))]
        public class when_creating_the_log4netlogger : concern_for_Log4NetLogger
        {
            protected static object result;
            protected static log4net.ILog log4net_logger;

            context c = () => { log4net_logger = the_dependency<log4net.ILog>(); };

            //because b = () => sut.Info("dude");

            [Observation]
            public void should_be_an_instance_of_ILog()
            {
                sut.should_be_an_instance_of<ILog>();
            }
        }


        [Concern(typeof(Log4NetLogger))]
        public class when_calling_debug_on_the_logger : concern_for_Log4NetLogger
        {
            protected static object result;
            protected static log4net.ILog log4net_logger;

            context c = () =>
                {
                    log4net_logger = the_dependency<log4net.ILog>();
                    log4net_logger.Stub(x => x.DebugFormat("dude"));
                };

            because b = () => sut.Debug("dude");

            [Observation]
            public void should_have_called_debug_format_on_the_internal_logger()
            {
                log4net_logger.was_told_to(x => x.DebugFormat("dude"));
            }
        }

        [Concern(typeof(Log4NetLogger))]
        public class when_calling_info_on_the_logger : concern_for_Log4NetLogger
        {
            protected static object result;
            protected static log4net.ILog log4net_logger;

            context c = () =>
                {
                    log4net_logger = the_dependency<log4net.ILog>();
                    log4net_logger.Stub(x => x.InfoFormat("dude"));
                };

            because b = () => sut.Info("dude");

            [Observation]
            public void should_have_called_info_format_on_the_internal_logger()
            {
                log4net_logger.was_told_to(x => x.InfoFormat("dude"));
            }
        }

        [Concern(typeof(Log4NetLogger))]
        public class when_calling_warn_on_the_logger : concern_for_Log4NetLogger
        {
            protected static object result;
            protected static log4net.ILog log4net_logger;

            context c = () =>
                {
                    log4net_logger = the_dependency<log4net.ILog>();
                    log4net_logger.Stub(x => x.WarnFormat("dude"));
                };

            because b = () => sut.Warn("dude");

            [Observation]
            public void should_have_called_warn_format_on_the_internal_logger()
            {
                log4net_logger.was_told_to(x => x.WarnFormat("dude"));
            }
        }

        [Concern(typeof(Log4NetLogger))]
        public class when_calling_error_on_the_logger : concern_for_Log4NetLogger
        {
            protected static object result;
            protected static log4net.ILog log4net_logger;

            context c = () =>
                {
                    log4net_logger = the_dependency<log4net.ILog>();
                    log4net_logger.Stub(x => x.ErrorFormat("dude"));
                };

            because b = () => sut.Error("dude");

            [Observation]
            public void should_have_called_error_format_on_the_internal_logger()
            {
                log4net_logger.was_told_to(x => x.ErrorFormat("dude"));
            }
        }

        [Concern(typeof(Log4NetLogger))]
        public class when_calling_fatal_on_the_logger : concern_for_Log4NetLogger
        {
            protected static object result;
            protected static log4net.ILog log4net_logger;

            context c = () =>
                {
                    log4net_logger = the_dependency<log4net.ILog>();
                    log4net_logger.Stub(x => x.FatalFormat("dude"));
                };

            because b = () => sut.Fatal("dude");

            [Observation]
            public void should_have_called_fatal_format_on_the_internal_logger()
            {
                log4net_logger.was_told_to(x => x.FatalFormat("dude"));
            }
        }
    }
}