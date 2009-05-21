using log4net.Config;

[assembly: XmlConfigurator(Watch = true)]

namespace bombali.host
{
    using System;
    using System.ServiceProcess;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;
    using infrastructure;
    using log4net;
    using runners;

    public partial class BombaliService : ServiceBase
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(BombaliService));
        IWindsorContainer _container;

        protected override void OnStart(string[] args)
        {
            _logger.InfoFormat("Starting {0} service.", ApplicationParameters.name);
            try
            {
                InitializeIOC(args);
                IRunner runner = infrastructure.containers.Container.get_an_instance_of<IRunner>();
                runner.run_the_application();

                _logger.InfoFormat("{0} service is now actively monitoring.", ApplicationParameters.name);

                if((args.Length > 0) && (Array.IndexOf(args, "/console") != -1))
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            catch(Exception ex)
            {
                _logger.ErrorFormat("{0} service had an error on {1} (with user {2}):{3}{4}", ApplicationParameters.name,
                                    Environment.MachineName, Environment.UserName,
                                    Environment.NewLine, ex.ToString());
            }
        }

        protected void InitializeIOC(string[] args)
        {
            _logger.Debug("Attempting to initialize IOC container.");
            _container = new WindsorContainer(new XmlInterpreter());
            infrastructure.containers.Container.initialize_with(new infrastructure.containers.custom.WindsorContainer(_container));
        }

        protected override void OnStop()
        {
            try
            {
                _logger.InfoFormat("Stopping {0} service.", ApplicationParameters.name);
                DisposeIOC();
                _logger.InfoFormat("{0} service has shut down.", ApplicationParameters.name);
            }
            catch(Exception ex)
            {
                _logger.ErrorFormat("{0} service had an error on {1} (with user {2}):{3}{4}", ApplicationParameters.name,
                                    Environment.MachineName, Environment.UserName,
                                    Environment.NewLine, ex.ToString());
            }
        }

        public void DisposeIOC()
        {
            _logger.Debug("Disposing the IOC container.");
            infrastructure.containers.Container.initialize_with(null);
            _container.Dispose();
        }

        public void RunConsole(string[] args)
        {
            OnStart(args);
            OnStop();
        }
    }
}