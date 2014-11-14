// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narrative.Narrator
{
    using Autofac;
    using Narrative.Properties;
    using Narrative.Templates;
    using Narrative.Weavers;
    using Narvalo;

    public sealed class WeaverEngineModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Require.NotNull(builder, "builder");

            builder.RegisterType<MarkdownDeepEngine>().As<IMarkdownEngine>();
            builder.Register(
                _ => new Template(Resources.Template, _.Resolve<IMarkdownEngine>()))
                .As<ITemplate<TemplateModel>>();

            builder.RegisterGeneric(typeof(WeaverEngine<>)).As(typeof(IWeaverEngine<>));
        }
    }
}
