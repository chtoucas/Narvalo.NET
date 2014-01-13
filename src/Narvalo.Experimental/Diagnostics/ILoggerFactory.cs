namespace Narvalo.Diagnostics
{
    using System;

    public interface ILoggerFactory 
    {
        ILogger CreateLogger(Type type);

        ILogger CreateLogger(string name);
    }
}
