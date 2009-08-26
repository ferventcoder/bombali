using bdddoc.core;
using developwithpassion.bdd.contexts;
using developwithpassion.bdd.mbunit;
using developwithpassion.bdd.mbunit.standard;
using developwithpassion.bdd.mbunit.standard.observations;

namespace bombali.tests.infrastructure.resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using bombali.infrastructure.app.monitorchecks;
    using bombali.infrastructure.app.settings;
    using bombali.infrastructure.resolvers;
    using MbUnit.Framework;

    public class DefaultInstanceCreatorSpecs
    {
        public abstract class concern_for_default_instance_creator : observations_for_a_static_sut
        {
        }

        [Concern(typeof(DefaultInstanceCreator))]
        public class when_using_the_instance_creator_to_resolve_an_object_from_a_string_type_that_exists : concern_for_default_instance_creator
        {
            protected static object result;
            private const string object_to_create = "bombali.infrastructure.app.monitorchecks.ServerCheck, bombali";
			
            context c = () =>
                            {
			            
                            };
        
            because b = () => result = DefaultInstanceCreator.create_object_from_string_type(object_to_create);
            

            [Observation]
            public void should_have_the_correct_interface()
            {
                result.should_be_an<ICheck>();
            }

            [Observation]
            public void should_be_an_instance_of_the_correct_type()
            {
                result.should_be_an<ServerCheck>();
            }

            [Observation]
            public void should_not_be_null()
            {
                result.should_not_be_null();
            }
            
        }

        [TestFixture]
        public class When_learning_about_type_resolution
        {
           
            [Test]
            public void should_resolve_a_type_by_string()
            {
                Type t = typeof(DefaultInstanceCreatorSpecs);
                Type t2 = System.Type.GetType("bombali.tests.infrastructure.resolvers.DefaultInstanceCreatorSpecs");

                Assert.AreEqual(t, t2);
            }

            [Test]
            public void should_resolve_a_type_by_string_with_assembly()
            {
                Type t = typeof(DefaultInstanceCreatorSpecs);
                Type t2 = Type.GetType("bombali.tests.infrastructure.resolvers.DefaultInstanceCreatorSpecs,bombali.tests");

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

                Type t2 = Type.GetType("bombali.tests.infrastructure.resolvers.DefaultInstanceCreatorSpecs,bombali.tests");
            }

            [Test]
            public void should_have_two_items_with_the_same_type()
            {
                const string type_string = "bombali.infrastructure.app.monitorchecks.ServerCheck, bombali";
                ServerCheck server_check = new ServerCheck();
                Type type = Type.GetType(type_string);

                Type monitor = Type.GetType(type_string);

                Assert.AreEqual(type.UnderlyingSystemType, server_check.GetType());
                Assert.AreEqual(monitor.UnderlyingSystemType, server_check.GetType());
            }

        }

    }
}