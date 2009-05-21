namespace bombali.infrastructure.logging
{
    using System;

    public interface ILogFactory
    {
        ILog create_logger_bound_to(Object type);
    }
}