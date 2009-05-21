namespace bombali.orm
{
    using System.Collections.Generic;
    using domain;

    public interface IPersistenceStore
    {
        IEnumerable<IMonitor> map_to_monitors();
    }
}