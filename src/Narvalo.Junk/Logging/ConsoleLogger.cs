namespace Narvalo.Logging
{
    using System;

    public class ConsoleLogger : LoggerBase
    {
        public ConsoleLogger(string name)
            : base(name) { }

        public ConsoleLogger(string name, LoggerLevel level)
            : base(name, level) { }

        protected override void LogCore(LoggerLevel level, string message, Exception exception)
        {
            Console.Out.WriteLine("[{0}] {1}", level, message);

            if (exception != null) {
                Console.Out.WriteLine(
                    "[{0}] {1}: {2} {3}",
                    level,
                    exception.GetType().FullName,
                    exception.Message,
                    exception.StackTrace);
            }
        }
    }
}
