// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.DocuMaker.Narrator
{
    using Autofac;
    using Narvalo;
    using Narvalo.DocuMaker.Properties;
    using Narvalo.DocuMaker.Templating;
    using Narvalo.DocuMaker.Weavers;

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
