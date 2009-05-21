namespace bombali.infrastructure.commands
{
    using System.Collections.Generic;
    using logging;

    public class CommandList : ICommandList
    {
        IList<ICommand> commands;

        public CommandList(IEnumerable<ICommand> commands)
        {
            Log.bound_to(this).Debug("Initializing {0}", GetType().Name);
            if(commands != null)
            {
                this.commands = new List<ICommand>(commands);
            }
        }

        public IEnumerable<ICommand> Commands
        {
            get
            {
                if(commands == null) commands = new List<ICommand>();
                return commands;
            }
        }

        public void run()
        {
            foreach(ICommand command in Commands) command.run();
        }
    }
}