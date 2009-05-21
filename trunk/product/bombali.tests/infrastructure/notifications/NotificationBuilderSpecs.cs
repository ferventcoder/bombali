using bdddoc.core;
using developwithpassion.bdd.contexts;
using developwithpassion.bdd.mbunit;
using developwithpassion.bdd.mbunit.standard;
using developwithpassion.bdd.mbunit.standard.observations;

namespace bombali.tests.infrastructure.notifications
{
    using bombali.infrastructure.containers;
    using bombali.infrastructure.mapping;
    using bombali.infrastructure.notifications;
    using Rhino.Mocks;

    public class NotificationBuilderSpecs
    {
        public abstract class concern_for_notification_builder : observations_for_a_sut_without_a_contract<NotificationBuilder>
        {
        }

        [Concern(typeof(NotificationBuilder))]
        public class when_notification_builder_is_creating_a_notification : concern_for_notification_builder
        {
            protected static object result;
            const string email_to ="dude@dude.com";
            const string email_message = "hey man, hows it?";
            const string email_subject="haven't heard from you in awhile";
            const string smtp_host = "mail.somewhere.com";
            const string email_from = "bob@bob.com";
            protected static IContainer the_container;
            static INotification notification;

            context c = () =>
                {
                    notification = an<INotification>();
                    the_container = an<IContainer>();
                    Container.initialize_with(the_container);
                    the_container.Stub(x => x.Resolve<INotification>()).Return(notification);
                    notification.Stub(x => x.send_notification(smtp_host, email_from, email_to, email_subject, email_message));

                    provide_a_basic_sut_constructor_argument(email_from);
                };
        
            because b = () => sut.to(email_to).with_message(email_message).with_subject(email_subject).and_use_notification_host(smtp_host);

            [Observation]
            public void should_call_the_container_to_resolve_an_instance_of_INotification()
            {
                the_container.AssertWasCalled(x => x.Resolve<INotification>());
            }

             [Observation]
            public void should_automatically_tell_the_INotification_to_send_notification()
            {
                 notification.AssertWasCalled(x => x.send_notification(smtp_host, email_from, email_to, email_subject, email_message));
            }
        }
    }
}