using log4net.Config;

[assembly: XmlConfigurator(Watch = true)]

namespace bombali.host
{
    using System;
    using System.ServiceProcess;
    using Castle.Core;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Castle.Windsor.Configuration.Interpreters;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using infrastructure;
    using infrastructure.data.accessors;
    using log4net;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Event;
    using NHibernate.Tool.hbm2ddl;
    using orm;
    using runners;
    using Environment=System.Environment;

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
            _container.Register( 
                Component.For(typeof(Repository)).Forward<IRepository>().ImplementedBy<Repository>(). 
                    Interceptors(InterceptorReference.ForType<LockInterceptor>() 
                                 //,InterceptorReference.ForType<TransactionInterceptor>() 
                                 ).Anywhere 
                    );
            _container.Kernel.AddComponentInstance("nhFactory",typeof(ISessionFactory),build_session_factory());
            infrastructure.containers.Container.initialize_with(new infrastructure.containers.custom.WindsorContainer(_container));
        }

        private static ISessionFactory build_session_factory()
        {
            return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2005
                        .ConnectionString(c =>
                            c.FromConnectionStringWithKey("bombali")))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<EmailMapping>())
                    .ExposeConfiguration(cfg =>
                    {
                        cfg.SetListener(ListenerType.PreInsert, new AuditEventListener());
                        cfg.SetListener(ListenerType.PreUpdate, new AuditEventListener());
                        //build_schema(cfg);
                    })
                    .BuildSessionFactory();
        }

        private static void build_schema(Configuration cfg)
        {
            SchemaExport schema_export = new SchemaExport(cfg);
            schema_export.SetOutputFile(@"..\..\..\..\bombali.database\Bombali\Up\0001_CreateTables.sql");
            schema_export.Create(true,false);
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