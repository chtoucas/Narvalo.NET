// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Narrator
{
    using System.Diagnostics;
    using System.IO;
    using Autofac;
    using Narvalo;
    using Narvalo.Narrative.Properties;
    using Narvalo.Narrative.Weavers;
    using NodaTime;
    using Serilog;

    public sealed class Application
    {
        readonly Settings _settings;

        public Application(Settings settings)
        {
            Require.NotNull(settings, "settings");

            _settings = settings;
        }

        public void Execute()
        {
            Log.Information(Resources.Starting);

            if (_settings.DryRun) {
                Log.Warning(Resources.DryRun);
            }

            var stopWatch = Stopwatch.StartNew();

            Weave_(_settings.Path);

            var elapsedTime = Duration.FromTicks(stopWatch.Elapsed.Ticks);
            Log.Information(Resources.ElapsedTime, elapsedTime);

            Log.Information(Resources.Ending);
        }

        void Weave_(string path)
        {
            var isDirectory = File.GetAttributes(path).HasFlag(FileAttributes.Directory);

            if (isDirectory) {
                Weave_(new DirectoryInfo(path));
            }
            else {
                Weave_(new FileInfo(path));
            }
        }

        void Weave_(DirectoryInfo directory)
        {
            using (var container = CreateContainer_()) {
                container.Resolve<DirectoryWeaverFacade>().Process(directory, _settings.RunInParallel);
            }
        }

        void Weave_(FileInfo file)
        {
            using (var container = CreateContainer_()) {
                container.Resolve<IWeaver<FileInfo>>().Weave(file);
            }
        }

        IContainer CreateContainer_()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new WriterModule
            {
                DryRun = _settings.DryRun,
                OutputDirectory = _settings.OutputDirectory
            });

            builder.RegisterModule(new FileFinderModule());
            builder.RegisterModule(new ParserModule());
            builder.RegisterModule(new WeaverEngineModule());
            builder.RegisterModule(new FileWeaverModule());
            builder.RegisterModule(new DirectoryWeaverModule());

            return builder.Build();
        }
    }
}
