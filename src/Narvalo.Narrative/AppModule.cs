// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;
    using Autofac;
    using Autofac.Extras.DynamicProxy2;
    using Narvalo.Narrative.Properties;
    using Serilog;
    using Serilog.Events;

    public sealed class AppModule : Module
    {
        readonly AppArguments _arguments;

        public AppModule(AppArguments arguments)
        {
            Require.NotNull(arguments, "arguments");

            _arguments = arguments;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WeavingInterceptor>().AsSelf();

            builder.RegisterType<MarkdownDeepEngine>().As<IMarkdownEngine>();
            builder.Register(CreateTemplate_).As<ITemplate>();

            var weaver = builder.RegisterType<Weaver>().As<IWeaver>();

            if (Log.IsEnabled(LogEventLevel.Information)) {
                weaver.EnableInterfaceInterceptors();
            }

            builder.Register(_ => new PathProvider(_arguments.OutputDirectory)).As<IPathProvider>();

            if (_arguments.DryRun) {
                builder.RegisterType<NoopWriter>().As<IOutputWriter>();
            }
            else {
                builder.RegisterType<OutputWriter>().As<IOutputWriter>();
            }

            if (_arguments.IsDirectory) {
                if (_arguments.RunInParallel) {
                    builder.Register(CreateParallelRunner_).As<IRunner>();
                }
                else {
                    builder.Register(CreateSequentialRunner_).As<IRunner>();
                }
            }
            else {
                builder.Register(CreateFileRunner_).As<IRunner>();
            }
        }

        static RazorTemplate CreateTemplate_(IComponentContext context)
        {
            return new RazorTemplate(Resources.Template, context.Resolve<IMarkdownEngine>());
        }

        IRunner CreateSequentialRunner_(IComponentContext context)
        {
            return new SequentialRunner(
                context.Resolve<IWeaver>(),
                context.Resolve<IOutputWriter>(),
                new DirectoryInfo(_arguments.Path));
        }

        IRunner CreateParallelRunner_(IComponentContext context)
        {
            return new ParallelRunner(
                context.Resolve<IWeaver>(),
                context.Resolve<IOutputWriter>(),
                new DirectoryInfo(_arguments.Path));
        }

        IRunner CreateFileRunner_(IComponentContext context)
        {
            return new Runner(
                context.Resolve<IWeaver>(),
                context.Resolve<IOutputWriter>(),
                new FileInfo(_arguments.Path));
        }
    }
}
