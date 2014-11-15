namespace Playground.Logging
{
    using System;

    public class SilentLogger : LoggerBase
    {
        public SilentLogger(string name)
            : base(name) { }

        public SilentLogger(string name, LoggerLevel level)
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
