namespace Narvalo.Diagnostics.Log4Net
{
    using System;
    using log4net;

    public sealed class Log4NetProxy : LoggerBase
    {
        readonly ILog _inner;

        Log4NetProxy(ILog inner)
            : base(inner.Logger.Name, GetLoggerLevel(inner))
        {
            _inner = inner;
        }

        internal static Log4NetProxy Create(ILog inner)
        {
            Requires.NotNull(inner, "inner");

            return new Log4NetProxy(inner);
        }

        protected override void LogCore(LoggerLevel level, string message, Exception exception)
        {
            switch (level) {
                case LoggerLevel.Debug:
                    _inner.Debug(message, exception);
                    break;
                case LoggerLevel.Informational:
                case LoggerLevel.Notice:
                    _inner.Info(message, exception);
                    break;
                case LoggerLevel.Warning:
                    _inner.Warn(message, exception);
                    break;
                case LoggerLevel.Error:
                    _inner.Error(message, exception);
                    break;
                case LoggerLevel.Critical:
                case LoggerLevel.Alert:
                case LoggerLevel.Fatal:
                    _inner.Fatal(message, exception);
                    break;
                case LoggerLevel.None:
                default:
                    // NB: normalement on n'arrive jamais là.
                    break;
            }
        }

        public static LoggerLevel GetLoggerLevel(ILog log)
        {
            // TODO: vérifier la correspondance.
            LoggerLevel level = LoggerLevel.None;

            if (log.IsFatalEnabled) {
                level |= LoggerLevel.Critical | LoggerLevel.Alert | LoggerLevel.Fatal;
            }
            if (log.IsErrorEnabled) {
                level |= LoggerLevel.Error;
            }
            if (log.IsWarnEnabled) {
                level |= LoggerLevel.Warning;
            }
            if (log.IsInfoEnabled) {
                level |= LoggerLevel.Informational | LoggerLevel.Notice;
            }
            if (log.IsDebugEnabled) {
                level |= LoggerLevel.Debug;
            }

            return level;
        }
    }
}
