namespace bombali.infrastructure.logging.custom
{
    using ILog = log4net.ILog;

    public class Log4NetLogger : logging.ILog
    {
        private readonly ILog logger;

        public Log4NetLogger(ILog logger)
        {
            this.logger = logger;
            //logger.DebugFormat("Initializing {0}<{1}>", GetType().FullName, logger.Logger.Name);
        }

        public void Debug(string message, params object[] args)
        {
            logger.DebugFormat(message, args);
        }

        public void Info(string message, params object[] args)
        {
            logger.InfoFormat(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            logger.WarnFormat(message, args);
        }

        public void Error(string message, params object[] args)
        {
            logger.ErrorFormat(message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            logger.FatalFormat(message, args);
        }
    }
}