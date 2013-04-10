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

            // On utilise la configuration telle que trouvée dans l'AppDomain.CurrentDomain.BaseDirectory
            return Log4NetProxy.Create(LogManager.GetLogger(type));
        }

        public ILogger CreateLogger(string name)
        {
            Requires.NotNullOrEmpty(name, "name");

            return Log4NetProxy.Create(LogManager.GetLogger(name));
        }

        #endregion
    }
}