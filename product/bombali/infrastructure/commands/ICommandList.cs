using System.Collections.Generic;

namespace bombali.infrastructure.commands
{
    public interface ICommandList
    {
        IEnumerable<ICommand> Commands { get; }
        void run();
    }
}