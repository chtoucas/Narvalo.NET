namespace Narvalo.Logging
{
    using System;

    public interface ILoggerFactory 
    {
        ILogger CreateLogger(Type type);

        ILogger CreateLogger(string name);
    }
}
