// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Logging
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    public class DebuggerLogger : LoggerBase
    {
        const int Informational_ = 0;
        const int Warning_ = 1;
        const int Error_ = 2;

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

            if (exception != null)
            {
                string debuggerMessage = Format.CurrentCulture(
                    Strings.DebuggerLogger_MessageFormat,
                    exception.GetType().FullName,
                    exception.Message,
                    exception.StackTrace);

                Debugger.Log(debuggerLevel, Name, debuggerMessage);
            }
        }

        static int ToDebuggerLevel_(LoggerLevel level)
        {
            switch (level)
            {
                case LoggerLevel.Debug:
                case LoggerLevel.Informational:
                case LoggerLevel.Notice:
                    return Informational_;
                case LoggerLevel.Warning:
                    return Warning_;
                case LoggerLevel.Error:
                case LoggerLevel.Critical:
                case LoggerLevel.Alert:
                case LoggerLevel.Fatal:
                    return Error_;
                case LoggerLevel.None:
                default:
                    // NB: Normalement on n'arrive jamais là.
                    throw new NotSupportedException("Unsupported level.");
            }
        }
    }
}
