namespace Narvalo.Playground
{
    using System;
    using Narvalo.Diagnostics;
    using Narvalo.Log4Net;

    public static class Logger
    {
        static readonly ILoggerFactory Factory_ = new Log4NetFactory();

        public static ILoggerFactory Factory { get { return Factory_; } }

        public static ILogger Create(Type type)
        {
            return Factory_.CreateLogger(type);
        }

        public static ILogger Create(string name)
        {
            return Factory_.CreateLogger(name);
        }
    }
}
