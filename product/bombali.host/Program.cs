namespace bombali.host
{
    using System;
    using System.ServiceProcess;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if((args.Length > 0) && (Array.IndexOf(args, "/console") != -1))
            {
                BombaliService service = new BombaliService();
                service.RunConsole(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                                    {
                                        new BombaliService()
                                    };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}