using System;
using log4net;
using log4net.Core;
using log4net.Repository;

namespace Core
{
    public interface ILoggingService : IDependency
    {
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
        void Error(string message);
        void Error(Exception x);
        void Error(string message, Exception x);
        void Fatal(string message);
        void Fatal(Exception x);
        void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception);
        void Log(LoggingEvent logEvent);
        bool IsEnabledFor(Level level);
        string Name { get; }
        ILoggerRepository Repository { get; }
        void SetLogger(string logger);
    }

    public class Log4NetLogger : ILogger, ILoggingService
    {
        private ILog _logger;
        
        public Log4NetLogger()
        {
            _logger = LogManager.GetLogger(GetType());
            ThreadContext.Properties["EventID"] = 1040;
        }

        public void SetLogger(string logger)
        {
            _logger = LogManager.GetLogger(logger.ToLower());
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception x)
        {
            _logger.Error(x.Message, x);
        }

        public void Error(string message, Exception x)
        {
            _logger.Error(message, x);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(Exception x)
        {
            _logger.Fatal(x.Message, x);
        }

        public void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Log(LoggingEvent logEvent)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabledFor(Level level)
        {
            throw new NotImplementedException();
        }

        public string Name { get; private set; }
        public ILoggerRepository Repository { get; private set; }
    }
}
