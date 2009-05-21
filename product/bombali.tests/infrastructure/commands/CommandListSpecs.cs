namespace bombali.tests.infrastructure.commands
{
    using System.Collections.Generic;
    using bdddoc.core;
    using bombali.infrastructure.commands;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using Rhino.Mocks;

    public class CommandListSpecs
    {
        public abstract class concern_for_command_list : observations_for_a_sut_with_a_contract<ICommandList, CommandList>
        {
        }

        [Concern(typeof(CommandList))]
        public class when_command_list_is_run : concern_for_command_list
        {
            protected static object result;
            static ICommand command;
            static ICommand command2;

            context c = () =>
                {
                    command = an<ICommand>();
                    command2 = an<ICommand>();
                    IList<ICommand> commands = new List<ICommand> {command, command2};
                    IEnumerable<ICommand> command_list = commands;
                    provide_a_basic_sut_constructor_argument(command_list);

                    command.Stub(x => x.run());
                    command2.Stub(x => x.run());
                };

            because b = () => sut.run();


            [Observation]
            public void should_have_called_each_of_the_underlying_commands()
            {
                command.AssertWasCalled(x => x.run());
                command2.AssertWasCalled(x => x.run());
            }
        }

        [Concern(typeof(CommandList))]
        public class when_command_list_is_created_with_no_items : concern_for_command_list
        {
            protected static object result;

            context c = () =>
                {
                    IList<ICommand> commands = new List<ICommand>();
                    IEnumerable<ICommand> command_list = commands;
                    provide_a_basic_sut_constructor_argument(command_list);
                };

            because b = () => sut.run();

            [Observation]
            public void should_not_have_any_errors()
            {
                // nothing to do here
            }
        }

        [Concern(typeof(CommandList))]
        public class when_command_list_is_created_with_a_null_list : concern_for_command_list
        {
            protected static object result;

            context c = () =>
                {
                    IEnumerable<ICommand> command_list = null;
                    provide_a_basic_sut_constructor_argument(command_list);
                };

            because b = () => sut.run();

            [Observation]
            public void should_not_have_any_errors()
            {
                // nothing to do here
            }
        }
    }
}