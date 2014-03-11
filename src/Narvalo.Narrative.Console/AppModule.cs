// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using Autofac;

    public sealed class AppModule : Module
    {
        readonly AppSettings _settings;

        public AppModule(AppSettings settings)
        {
            Require.NotNull(settings, "settings");

            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.Register(_ => _settings).AsSelf().SingleInstance();

            builder.RegisterType<AppService>().AsSelf();

            builder.RegisterType<MarkdownDeepEngine>().As<IMarkdownEngine>();
            builder.Register(
                _ => new Template("Resources/linear.cshtml", _.Resolve<IMarkdownEngine>())).As<Template>();
        }
    }
}
