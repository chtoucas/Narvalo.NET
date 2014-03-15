// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using Autofac;
    using Narvalo.Narrative.Properties;

    public sealed class CmdLineModule : Module
    {
        readonly CmdLineOptions _options;

        public CmdLineModule(CmdLineOptions options)
        {
            Require.NotNull(options, "options");

            _options = options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.RegisterType<MarkdownDeepEngine>().As<IMarkdownEngine>();

            builder.Register(_ => _options).AsSelf();
            builder.RegisterType<CmdLine>().AsSelf();
            builder.Register(CreateRazorTemplate_).AsSelf();
        }

        static RazorTemplate CreateRazorTemplate_(IComponentContext context)
        {
            return new RazorTemplate(Resources.Template, context.Resolve<IMarkdownEngine>());
        }
    }
}
