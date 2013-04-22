namespace Narvalo.Playground
{
    using Narvalo.Diagnostics;

    public class AppService/*Impl*/ : IAppService
    {
        readonly ILogger _logger;

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
