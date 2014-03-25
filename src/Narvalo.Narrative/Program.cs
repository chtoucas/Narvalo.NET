// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Diagnostics;
    using Autofac;
    using Narvalo.Narrative.Configuration;
    using Narvalo.Narrative.Properties;
    using Narvalo.Narrative.Runtime;
    using NodaTime;
    using Serilog;

    public sealed class Program
    {
        readonly Settings _settings;

        public Program(Settings settings)
        {
            _settings = settings;
        }

        public static void Main(string[] args)
        {
            var settings = SettingsManager.Resolve();

            (new SerilogConfig(settings)).Configure();

            Log.Information(Resources.Starting);

            try {
                new Program(settings).Run();
            }
            catch (NarrativeException ex) {
                Log.Fatal(Resources.UnhandledNarrativeException, ex);
            }

            Log.Information(Resources.Ending);
        }

        public void Run()
        {
            if (_settings.DryRun) {
                Log.Warning(Resources.DryRun);
            }

            var stopWatch = Stopwatch.StartNew();

            // Configure builder.
            var builder = new ContainerBuilder();
            builder.RegisterModule(new WriterModule { Settings = _settings });
            builder.RegisterModule(new WeaverModule());
            builder.RegisterModule(new ProcessorModule());

            using (var container = builder.Build()) {
                var facade = new ProgramFacade(_settings, container);

                facade.Process(@"..\..\..\Narvalo.Common\Apply.cs");
            }

            var elapsedTime = Duration.FromTicks(stopWatch.Elapsed.Ticks);
            Log.Information(Resources.ElapsedTime, elapsedTime);
        }
    }
}
