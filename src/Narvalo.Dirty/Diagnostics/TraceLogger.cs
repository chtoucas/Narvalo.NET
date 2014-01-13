namespace Narvalo.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security;

    public class TraceLogger : LoggerBase
    {
        static readonly Dictionary<string, TraceSource> Cache_
            = new Dictionary<string, TraceSource>();

        TraceSource _traceSource;

        public TraceLogger(string name)
            : base(name)
        {
            Initialize_();
        }

        public TraceLogger(string name, LoggerLevel level)
            : base(name, level)
        {
            Initialize_();
        }

        protected override void LogCore(LoggerLevel level, string message, Exception exception)
        {
            if (exception == null) {
                _traceSource.TraceEvent(ToTraceEventType_(level), 0, message);
            }
            else {
                _traceSource.TraceData(ToTraceEventType_(level), 0, message, exception);
            }
        }

        #region Membres privés

        [SecurityCritical]
        void Initialize_()
        {
            lock (Cache_) {
                // because TraceSource is meant to be used as a static member, and because
                // building up the configuraion inheritance is non-trivial, the instances
                // themselves are cached for so multiple TraceLogger instances will reuse
                // the named TraceSources which have been created
                if (Cache_.TryGetValue(Name, out _traceSource)) {
                    return;
                }

                var defaultLevel = ToSourceLevels_(Level);
                _traceSource = new TraceSource(Name, defaultLevel);

                // no further action necessary when the named source is configured
                if (IsSourceConfigured_(_traceSource)) {
                    Cache_.Add(Name, _traceSource);
                    return;
                }

                // otherwise hunt for a shorter source that been configured            
                var foundSource = new TraceSource("Default", defaultLevel);

                var searchName = ShortenName_(Name);
                while (!string.IsNullOrEmpty(searchName)) {
                    var searchSource = new TraceSource(searchName, defaultLevel);
                    if (IsSourceConfigured_(searchSource)) {
                        foundSource = searchSource;
                        break;
                    }

                    searchName = ShortenName_(searchName);
                }

                // reconfigure the created source to act like the found @this
                _traceSource.Switch = foundSource.Switch;
                _traceSource.Listeners.Clear();
                foreach (TraceListener listener in foundSource.Listeners) {
                    _traceSource.Listeners.Add(listener);
                }

                Cache_.Add(Name, _traceSource);
            }
        }

        static string ShortenName_(string name)
        {
            var lastDot = name.LastIndexOf('.');
            if (lastDot != -1) {
                return name.Substring(0, lastDot);
            }
            return null;
        }

        [SecuritySafeCritical]
        static bool IsSourceConfigured_(TraceSource source)
        {
            return !(
                source.Listeners.Count == 1
                && source.Listeners[0] is DefaultTraceListener
                && source.Listeners[0].Name == "Default");
        }

        static LoggerLevel ToLoggerLevel_(SourceLevels level)
        {
            switch (level) {
                case SourceLevels.All:
                    return LoggerLevel.Debug;
                case SourceLevels.Verbose:
                    return LoggerLevel.Debug;
                case SourceLevels.Information:
                    return LoggerLevel.Informational;
                case SourceLevels.Warning:
                    return LoggerLevel.Warning;
                case SourceLevels.Error:
                    return LoggerLevel.Error;
                case SourceLevels.Critical:
                    return LoggerLevel.Fatal;
                default:
                    return LoggerLevel.None;
            }
        }

        static SourceLevels ToSourceLevels_(LoggerLevel level)
        {
            switch (level) {
                case LoggerLevel.Debug:
                    return SourceLevels.Verbose;
                case LoggerLevel.Informational:
                    return SourceLevels.Information;
                case LoggerLevel.Warning:
                    return SourceLevels.Warning;
                case LoggerLevel.Error:
                    return SourceLevels.Error;
                case LoggerLevel.Fatal:
                    return SourceLevels.Critical;
                default:
                    return SourceLevels.Off;
            }
        }

        static TraceEventType ToTraceEventType_(LoggerLevel level)
        {
            switch (level) {
                case LoggerLevel.Debug:
                    return TraceEventType.Verbose;
                case LoggerLevel.Informational:
                    return TraceEventType.Information;
                case LoggerLevel.Warning:
                    return TraceEventType.Warning;
                case LoggerLevel.Error:
                    return TraceEventType.Error;
                case LoggerLevel.Fatal:
                    return TraceEventType.Critical;
                default:
                    return TraceEventType.Verbose;
            }
        }

        #endregion
    }
}
