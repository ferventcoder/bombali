namespace bombali.infrastructure.mapping
{
    public static class Map
    {
        public static MapBuilder<From> from<From>(From from_object)
        {
            return new MapBuilder<From>(from_object);
        }
    }
}