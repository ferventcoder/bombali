namespace bombali.tests.integration.infrastructure.app.settings
{
    using bombali.infrastructure.app.settings;
    using MbUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using bombali.infrastructure.app.monitorchecks;

    public class MonitorConfigurationCollectionSpecs
    {

        [TestFixture]
        public class When_getting_the_items_to_monitor_configuration
        {
            MonitorConfigurationCollection _config;

            [SetUp]
            public void setup_and()
            {
                _config = BombaliConfiguration.settings.items_to_monitor;
            }

            [Test]
            public void Verify_successful_creation()
            {
                Assert.IsNotNull(_config);
            }

            [Test]
            public void should_get_back_one_items_from_the_configuration()
            {
                Assert.AreEqual(1, _config.Count);
            }

            [Test]
            public void should_resolve_a_type_by_string()
            {
                Type t = typeof(MonitorConfigurationCollectionSpecs);
                Type t2 = System.Type.GetType("bombali.tests.integration.infrastructure.app.settings.MonitorConfigurationCollectionSpecs");

                Assert.AreEqual(t, t2);
            }

            [Test]
            public void should_resolve_a_type_by_string_with_assembly()
            {
                Type t = typeof(MonitorConfigurationCollectionSpecs);
                Type t2 = Type.GetType("bombali.tests.integration.infrastructure.app.settings.MonitorConfigurationCollectionSpecs,bombali.tests.integration");

                Console.WriteLine(t2.ToString());
                Assert.AreEqual(t, t2);
            }

            [Test]
            public void should_invoke_a_generic_method_with_type_resolved_at_runtime()
            {
                Type generic = typeof(List<>);
                Type specific = generic.MakeGenericType(typeof(int));
                ConstructorInfo ci = specific.GetConstructor(new Type[] { });
                object o = ci.Invoke(new object[] { });



                Type t2 = Type.GetType("bombali.tests.integration.infrastructure.app.settings.MonitorConfigurationCollectionSpecs,bombali.tests.integration");


            }

            [Test]
            public void should_have_two_items_with_the_same_type()
            {
                string type_string = "bombali.infrastructure.app.monitorchecks.ServerCheck, bombali";
                ServerCheck server_check = new ServerCheck();
                Type type = Type.GetType(type_string);

                Type monitor = Type.GetType(type_string);

                Assert.AreEqual(type.UnderlyingSystemType, server_check.GetType());
                Assert.AreEqual(monitor.UnderlyingSystemType, server_check.GetType());
                
            }

        }
    }
}