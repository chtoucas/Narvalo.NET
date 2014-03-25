// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Runtime
{
    using Autofac;
    using Narvalo.Narrative.Properties;
    using Narvalo.Narrative.Weaving;

    public sealed class WeaverEngineModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MarkdownDeepEngine>().As<IMarkdownEngine>();
            builder.Register(
                _ => new RazorTemplate(Resources.Template, _.Resolve<IMarkdownEngine>())).As<ITemplate>();

            builder.RegisterType<WeaverEngine>().As<IWeaverEngine>();
        }
    }
}
