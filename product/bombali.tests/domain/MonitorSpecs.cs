namespace bombali.tests.domain
{
    using bdddoc.core;
    using bombali.domain;
    using bombali.infrastructure;
    using bombali.infrastructure.app.monitorchecks;
    using bombali.infrastructure.containers;
    using bombali.infrastructure.notifications;
    using bombali.infrastructure.timers;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using Rhino.Mocks;

    public class MonitorSpecs
    {
        public abstract class concern_for_monitor : observations_for_a_sut_with_a_contract<IMonitor, Monitor>
        {
            protected static object result;
            protected const string monitor_name = "monitor";
            protected const string url = "http://";
            protected const double interval_in_minutes = 0.2;
            protected const string emails_to = "dude@s.com,dude3@ss.com";
            protected const string email_from = "yeppers@s.com";
            protected const string notification_host = "smtp.home.com";
            protected static ITimer timer;
            protected static ICheck check_utility;
            protected static IContainer the_container;
            protected static INotification notification;

            public override IMonitor create_sut()
            {
                //return base.create_sut();
                return new Monitor(monitor_name, url, interval_in_minutes, emails_to, email_from, notification_host,check_utility, timer);
            }
        }

        [Concern(typeof(Monitor))]
        public class when_monitor_is_created : concern_for_monitor
        {
            context c = () =>
                {
                    check_utility = an<ICheck>();
                    timer = an<ITimer>();
                };

            because b = () => result = sut;

            [Observation]
            public void should_be_an_instance_of_IMonitor()
            {
                result.should_be_an_instance_of<IMonitor>();
            }

            [Observation]
            public void should_be_an_instance_of_Monitor()
            {
                result.should_be_an_instance_of<Monitor>();
            }

            [Observation]
            public void should_set_the_name_correctly()
            {
                sut.name.should_be_equal_to(monitor_name);
            }

            [Observation]
            public void should_set_the_url_correctly()
            {
                sut.what_to_check.should_be_equal_to(url);
            }

            [Observation]
            public void should_set_the_interval_correctly()
            {
                sut.interval_in_minutes_for_check.should_be_equal_to(interval_in_minutes);
            }

            [Observation]
            public void should_set_the_notification_list_correctly()
            {
                sut.who_to_notify_as_comma_separated_list.should_be_equal_to(emails_to);
            }

            [Observation]
            public void should_set_the_notification_from_correctly()
            {
                sut.who_notification_comes_from.should_be_equal_to(email_from);
            }

            [Observation]
            public void should_set_the_notification_host_correctly()
            {
                sut.notification_host.should_be_equal_to(notification_host);
            }
        }

        [Concern(typeof(Monitor))]
        public class when_monitor_is_asked_to_start_monitoring : concern_for_monitor
        {
            context c = () =>
                {
                    check_utility = an<ICheck>();
                    timer = an<ITimer>();
                    timer.Stub(x => x.start());
                };

            because b = () => sut.start_monitoring();

            [Observation]
            public void should_have_called_the_timer_start_method()
            {
                timer.AssertWasCalled(x => x.start());
            }
        }

        [Concern(typeof(Monitor))]
        public class when_monitor_is_asked_to_change_interval : concern_for_monitor
        {
            const double new_interval = 0.5;

            context c = () =>
                {
                    check_utility = an<ICheck>();
                    timer = an<ITimer>();
                    timer.Stub(x => x.change_interval(new_interval));
                };

            because b = () => sut.change_interval(new_interval);


            [Observation]
            public void should_have_called_the_timer_change_interval_method()
            {
                timer.AssertWasCalled(x => x.change_interval(new_interval));
            }
        }

        [Concern(typeof(Monitor))]
        public class when_monitor_is_asked_to_make_a_desicion_on_a_failed_hit_and_the_result_before_was_successful : concern_for_monitor
        {
            static string subject;
            static string message;

            context c = () =>
            {
                check_utility = an<ICheck>();
                timer = an<ITimer>();
                notification = an<INotification>();
                the_container = an<IContainer>();
                Container.initialize_with(the_container);
                the_container.Stub(x => x.Resolve<INotification>()).Return(notification);

                const string reporting_type = "BAD";
                subject = string.Format("{0} - \"{1}\" {2}", ApplicationParameters.name, monitor_name, reporting_type);
                message = string.Format("{0} reports {1} for {2}.", ApplicationParameters.name, "NotFound", url);

                notification.Stub(
                    x => x.send_notification(notification_host, email_from, emails_to, subject, message)).
                    IgnoreArguments();
            };

            because b = () => ((Monitor)sut).make_decision_based_on_check(false);

            [Observation]
            public void should_have_called_the_container_to_resolve_notification()
            {
                the_container.AssertWasCalled(x => x.Resolve<INotification>());
            }

            //[Observation]
            //public void should_have_called_send_notification_method_on_notification()
            //{
            //    notification.AssertWasCalled(x => x.send_notification(notification_host, email_from, emails_to, subject, message));
            //}
        }

        [Concern(typeof(Monitor))]
        public class when_monitor_is_asked_to_make_a_desicion_on_a_sucessful_hit_and_the_result_before_was_successful : concern_for_monitor
        {
            static string subject;
            static string message;
            
            context c = () =>
            {
                check_utility = an<ICheck>();
                timer = an<ITimer>();
                notification = an<INotification>();
                the_container = an<IContainer>();
                Container.initialize_with(the_container);
                the_container.Stub(x => x.Resolve<INotification>()).Return(notification);

                const string reporting_type = "BAD";
                subject = string.Format("{0} - \"{1}\" {2}", ApplicationParameters.name, monitor_name, reporting_type);
                message = string.Format("{0} reports {1} for {2}.", ApplicationParameters.name, "NotFound", url);

                notification.Stub(
                    x => x.send_notification(notification_host, email_from, emails_to, subject, message)).
                    IgnoreArguments();
            };

            because b = () => ((Monitor)sut).make_decision_based_on_check(true);

            [Observation]
            public void should_not_have_called_the_container_to_resolve_notification()
            {
                the_container.AssertWasNotCalled(x => x.Resolve<INotification>());
            }

            [Observation]
            public void should_not_have_called_send_notification_method_on_notification()
            {
                notification.AssertWasNotCalled(x => x.send_notification(notification_host, email_from, emails_to, subject, message));
            }
        }

    }
}