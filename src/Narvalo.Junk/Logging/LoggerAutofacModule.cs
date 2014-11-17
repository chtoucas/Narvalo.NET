namespace Narvalo.Logging
{
    using System.Linq;
    using Autofac;
    using Autofac.Core;
    using Narvalo;

    // Cf. http://code.google.com/p/autofac/wiki/Log4NetIntegration
    public class LoggerAutofacModule : Module
    {
        readonly ILoggerFactory _factory;

        public LoggerAutofacModule(ILoggerFactory factory)
        {
            Require.NotNull(factory, "factory");

            _factory = factory;
        }

        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            Require.NotNull(componentRegistry, "componentRegistry");

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
