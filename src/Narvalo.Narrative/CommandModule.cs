// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using Autofac;
    using Narvalo.Narrative.Properties;

    public sealed class CommandModule : Module
    {
        readonly AppSettings _settings;

        public CommandModule(AppSettings settings)
        {
            Require.NotNull(settings, "settings");

            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.RegisterType<MarkdownDeepEngine>().As<IMarkdownEngine>();

            builder.Register(_ => _settings).AsSelf();
            builder.RegisterType<Command>().AsSelf();
            builder.Register(CreateRazorTemplate_).AsSelf();
        }

        static RazorTemplate CreateRazorTemplate_(IComponentContext context)
        {
            return new RazorTemplate(Resources.Template, context.Resolve<IMarkdownEngine>());
        }
    }
}
