using Autofac;
using Autofac.Core;

namespace Narvalo.Autofac
{
    using System.Linq;
    using Narvalo.Diagnostics;

    // Cf. http://code.google.com/p/autofac/wiki/Log4NetIntegration
    public class LoggerModule : Module
    {
        readonly ILoggerFactory _factory;

        public LoggerModule(ILoggerFactory factory)
        {
            Requires.NotNull(factory, "factory");

            _factory = factory;
        }

        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            Requires.NotNull(componentRegistry, "componentRegistry");

            registration.Preparing += OnPreparingComponent_;
        }

        void OnPreparingComponent_(object sender, PreparingEventArgs e)
        {
            var t = e.Component.Activator.LimitType;

            e.Parameters = e.Parameters.Union(new[] {
                new ResolvedParameter(
                    (p, i) => p.ParameterType == typeof(ILogger), (p, i) => _factory.CreateLogger(t))
            });
        }
    }
}
