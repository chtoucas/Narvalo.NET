namespace Narvalo.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    public class DebuggerLogger : LoggerBase
    {
        const int _Informational = 0;
        const int _Warning = 1;
        const int _Error = 2;

        public DebuggerLogger()
            : base(Debugger.DefaultCategory /* name */) { }

        public DebuggerLogger(string category)
            : base(category /* name */) { }

        public DebuggerLogger(string category, LoggerLevel level)
            : base(category /* name */, level) { }

        protected override void LogCore(LoggerLevel level, string message, Exception exception)
        {
            if (!Debugger.IsAttached) { return; }

            int debuggerLevel = ToDebuggerLevel_(level);

            Debugger.Log(debuggerLevel, Name, message);

            if (exception != null) {
                string debuggerMessage = String.Format(
                    CultureInfo.CurrentCulture,
                    SR.DebuggerLogger_MessageFormat,
                    exception.GetType().FullName,
                    exception.Message,
                    exception.StackTrace);

                Debugger.Log(debuggerLevel, Name, debuggerMessage);
            }
        }

        #region Membres privés

        static int ToDebuggerLevel_(LoggerLevel level)
        {
            switch (level) {
                case LoggerLevel.Debug:
                case LoggerLevel.Informational:
                case LoggerLevel.Notice:
                    return _Informational;
                case LoggerLevel.Warning:
                    return _Warning;
                case LoggerLevel.Error:
                case LoggerLevel.Critical:
                case LoggerLevel.Alert:
                case LoggerLevel.Fatal:
                    return _Error;
                case LoggerLevel.None:
                default:
                    // NB: normalement on n'arrive jamais là.
                    throw new NotSupportedException();
            }
        }

        #endregion
    }
}
