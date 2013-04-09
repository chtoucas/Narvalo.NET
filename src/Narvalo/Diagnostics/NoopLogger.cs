namespace Narvalo.Diagnostics
{
    using System;

    public class NoopLogger : LoggerBase
    {
        public NoopLogger(string name)
            : base(name) { }

        public NoopLogger(string name, LoggerLevel level)
            : base(name, level) { }

        public override bool IsLevelEnabled(LoggerLevel level)
        {
            return false;
        }

        protected override void LogCore(LoggerLevel level, string message, Exception exception)
        {
            ;
        }
    }
}
