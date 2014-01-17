namespace Narvalo.Log4Net
{
    using System;
    using log4net;
    using Narvalo.Diagnostics;

    public sealed class Log4NetLogger : LoggerBase
    {
        readonly ILog _inner;

        Log4NetLogger(ILog inner)
            : base(inner.Logger.Name, GetLoggerLevel(inner))
        {
            _inner = inner;
        }

        internal static Log4NetLogger Create(ILog inner)
        {
            Require.NotNull(inner, "inner");

            return new Log4NetLogger(inner);
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
                    throw new NotSupportedException("Unsupported level.");
            }
        }

        public static LoggerLevel GetLoggerLevel(ILog log)
        {
            // REVIEW: Vérifier la correspondance.
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
