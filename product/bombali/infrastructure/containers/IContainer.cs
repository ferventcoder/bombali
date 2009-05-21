namespace bombali.infrastructure.containers
{
    public interface IContainer
    {
        TypeToReturn Resolve<TypeToReturn>();
    }
}