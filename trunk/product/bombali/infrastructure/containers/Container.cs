namespace bombali.infrastructure.containers
{
    using System;

    public static class Container
    {
        private static IContainer the_container;

        public static void initialize_with(IContainer container)
        {
            the_container = container;
        }

        public static TypeToGet get_an_instance_of<TypeToGet>()
        {
            return the_container.Resolve<TypeToGet>();
        }

        public static TypeToGet get_an_instance_of<TypeToGet>(TypeToGet type_to_get)
        {
            //BUG: this doesn't work right - looks for System.Type
            return get_an_instance_of<TypeToGet>();
        }
    }
}