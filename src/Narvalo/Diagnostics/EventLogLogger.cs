namespace Narvalo.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Narvalo.Resources;

    public class EventLogLogger : LoggerBase, IDisposable
    {
        readonly string _machineName;
        readonly string _source;

        bool _disposed = false;
        Lazy<EventLog> _eventLogThunk;

        public EventLogLogger(string name, string source)
            : this(name, LoggerLevel.None, source, String.Empty) { }

        public EventLogLogger(string name, string source, string machineName)
            : this(name, LoggerLevel.None, source, machineName) { }

        public EventLogLogger(string name, LoggerLevel level, string source)
            : this(name, level, source, String.Empty) { }

        public EventLogLogger(string name, LoggerLevel level, string source, string machineName)
            : base(name, level)
        {
            Requires.NotNullOrEmpty(source, "source");
            Requires.NotNull(machineName, "machineName");

            _source = source;
            _machineName = machineName;

            _eventLogThunk = new Lazy<EventLog>(() => CreateEventLog_(name, source, machineName));
        }

        public string MachineName { get { return _machineName; } }
        public string Source { get { return _source; } }

        protected override void LogCore(LoggerLevel level, string message, Exception exception)
        {
            if (_disposed) {
                throw ExceptionFactory.ObjectDisposed(typeof(EventLogLogger));
            }

            string eventMessage;

            if (exception == null) {
                eventMessage = String.Format(
                    CultureInfo.CurrentCulture,
                    SR.EventLogLogger_MessageFormat,
                    level,
                    message);
            }
            else {
                eventMessage = String.Format(
                    CultureInfo.CurrentCulture,
                    SR.EventLogLogger_ExceptionFormat,
                    level,
                    message,
                    exception.GetType().FullName,
                    exception.Message,
                    exception.StackTrace);
            }

            _eventLogThunk.Value.WriteEntry(eventMessage, ToEventLogEntryType_(level));
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    if (_eventLogThunk.IsValueCreated) {
                        _eventLogThunk.Value.Close();
                        _eventLogThunk = null;
                    }
                }

                _disposed = true;
            }
        }

        #region Membres privés

        static EventLog CreateEventLog_(string name, string source, string machineName)
        {
            if (String.IsNullOrEmpty(machineName)) {
                return CreateLocalEventLog_(name, source);
            }
            else {
                return CreateRemoteEventLog_(name, source, machineName);
            }
        }

        static EventLog CreateLocalEventLog_(string name, string source)
        {
            if (!EventLog.SourceExists(source)) {
                EventLog.CreateEventSource(source, name);
            }

            return new EventLog(name) { Source = source };
        }

        static EventLog CreateRemoteEventLog_(string name, string source, string machineName)
        {
            if (!EventLog.SourceExists(source, machineName)) {
                var sourceData = new EventSourceCreationData(source, name) { MachineName = machineName };
                EventLog.CreateEventSource(sourceData);
            }

            return new EventLog(name, machineName, source);
        }

        static EventLogEntryType ToEventLogEntryType_(LoggerLevel level)
        {
            switch (level) {
                case LoggerLevel.Debug:
                case LoggerLevel.Informational:
                case LoggerLevel.Notice:
                    return EventLogEntryType.Information;
                case LoggerLevel.Warning:
                    return EventLogEntryType.Warning;
                case LoggerLevel.Error:
                case LoggerLevel.Critical:
                case LoggerLevel.Alert:
                case LoggerLevel.Fatal:
                    return EventLogEntryType.Error;
                case LoggerLevel.None:
                default:
                    // NB: normalement on n'arrive jamais là.
                    throw new NotSupportedException();
            }
        }

        #endregion
    }
}
