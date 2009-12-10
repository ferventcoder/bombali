namespace bombali.infrastructure.information
{
    using System.Reflection;

    public class Version
    {
        public static string get_version()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString(4);
        }
    }
}