namespace Narvalo.Playground
{
    using System;
    using Serilog;

    public class AppService/*Impl*/ : IAppService
    {
        readonly ILogger _logger;

        [CLSCompliant(false)]
        public AppService(ILogger logger)
        {
            _logger = logger;
        }

        public string Hello(string name)
        {
            return "Hello " + name;
        }
    }
}
