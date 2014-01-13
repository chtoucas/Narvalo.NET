namespace Narvalo.Log4Net
{
    using System;
    using log4net;
    using Narvalo.Diagnostics;

    public class Log4NetFactory : ILoggerFactory
    {
        #region ILoggerFactory

        public ILogger CreateLogger(Type type)
        {
            Requires.NotNull(type, "type");

            return Log4NetLogger.Create(LogManager.GetLogger(type));
        }

        public ILogger CreateLogger(string name)
        {
            Requires.NotNullOrEmpty(name, "name");

            return Log4NetLogger.Create(LogManager.GetLogger(name));
        }

        #endregion
    }
}