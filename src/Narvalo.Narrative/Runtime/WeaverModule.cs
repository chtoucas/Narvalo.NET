// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Runtime
{
    using Autofac;
    using Autofac.Extras.DynamicProxy2;
    using Narvalo.Narrative.Properties;
    using Serilog;
    using Serilog.Events;

    public sealed class WeaverModule : Module
    {
        public bool IsDebuggingEnabled
        {
            get { return Log.IsEnabled(LogEventLevel.Debug); }
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MarkdownDeepEngine>().As<IMarkdownEngine>();
            builder.RegisterType<RazorTemplate>().As<ITemplate>()
                .WithParameter("input", Resources.Template);

            var weaver = builder.RegisterType<Weaver>().As<IWeaver>();

            //if (IsDebuggingEnabled) {
            //    builder.RegisterType<WeavingInterceptor>().AsSelf();

            //    weaver.EnableInterfaceInterceptors();
            //}
        }
    }
}
