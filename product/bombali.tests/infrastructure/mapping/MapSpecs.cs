using bdddoc.core;
using developwithpassion.bdd.contexts;
using developwithpassion.bdd.mbunit;
using developwithpassion.bdd.mbunit.standard;
using developwithpassion.bdd.mbunit.standard.observations;

namespace bombali.tests.infrastructure.mapping
{
    using bombali.infrastructure.commands;
    using bombali.infrastructure.mapping;

    public class MapSpecs
    {
        public abstract class concern_for_map : observations_for_a_static_sut
        {
        }

        [Concern(typeof(Map))]
        public class when_map_from_is_called : concern_for_map
        {
            protected static object result;
            static ICommand command;
			
            context c = () =>
                {
                    command = an<ICommand>();
                };
        
            because b = () => result = Map.from(command);
            

            [Observation]
            public void should_call_underlying_map_builder()
            {
                result.should_be_an<MapBuilder<ICommand>>();
            }
        }
    }
}