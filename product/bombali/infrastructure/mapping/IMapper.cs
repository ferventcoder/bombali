namespace bombali.infrastructure.mapping
{
    public interface IMapper<From, To>
    {
        To map_from(From from);
    }
}