namespace bombali.tests.infrastructure.notifications
{
    using bdddoc.core;
    using bombali.infrastructure.notifications;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;

    public class SendNotificationSpecs
    {
        public abstract class concern_for_sendnotification : observations_for_a_static_sut
        {
        }

        [Concern(typeof(SendNotification))]
        public class when_sendnotification_is_asked_to_send_a_notification : concern_for_sendnotification
        {
            protected static object result;
            const string an_address = "bob@bob.com";

            because b = () => result = SendNotification.from(an_address);

            [Observation]
            public void should_call_underlying_notification_builder()
            {
                result.should_be_an<NotificationBuilder>();
            }
        }
    }
}