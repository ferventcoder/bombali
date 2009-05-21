namespace bombali.tests.infrastructure.mapping
{
    using System.Collections.Generic;
    using bdddoc.core;
    using bombali.infrastructure.commands;
    using bombali.infrastructure.containers;
    using bombali.infrastructure.mapping;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using Rhino.Mocks;

    public class MapBuilderSpecs
    {
        public abstract class concern_for_map_builder : observations_for_a_sut_without_a_contract<MapBuilder<ICommand>>
        {
        }

        [Concern(typeof(MapBuilder<>))]
        public class when_map_builder_is_called_to_convert_an_item_from_something_to_something_else : concern_for_map_builder
        {
            protected static object result;
            static ICommand command;
            protected static IContainer the_container;
            protected static IMapper<ICommand, ICommandList> mock_mapper;

            context c = () =>
                {
                    the_container = an<IContainer>();
                    Container.initialize_with(the_container);

                    mock_mapper = an<IMapper<ICommand, ICommandList>>();

                    command = an<ICommand>();
                    provide_a_basic_sut_constructor_argument(command);

                    the_container.Stub(x => x.Resolve<IMapper<ICommand, ICommandList>>()).Return(mock_mapper);
                    mock_mapper.Stub(x => x.map_from(command)).Return(new CommandList(new List<ICommand> {command}));
                };

            because b = () => { result = sut.to<ICommandList>(); };

            [Observation]
            public void should_map_successfully()
            {
                result.should_be_an_instance_of<ICommandList>();
            }

            [Observation]
            public void should_have_called_the_container_to_resolve_an_IMapper_of_the_correct_type()
            {
                the_container.AssertWasCalled(x => x.Resolve<IMapper<ICommand, ICommandList>>());
            }

            [Observation]
            public void should_have_called_the_map_from_method_on_the_correct_mapper()
            {
                mock_mapper.AssertWasCalled(x => x.map_from(command));
            }

        }
    }
}