using Autofac;

namespace Narvalo.Playground
{
    using Narvalo.Diagnostics;

    public class AppModule : Module
    {
        public AppModule() : base() { }

        protected override void Load(ContainerBuilder builder)
        {
            Requires.NotNull(builder, "builder");

            //builder.Register(_ => Logger.Factory).As<ILoggerFactory>().SingleInstance();
            //builder.Register(_ => Logger.Create(typeof(Program).Namespace)).As<ILogger>().SingleInstance();

            builder.RegisterType<AppService>().As<IAppService>();
        }
    }
}
