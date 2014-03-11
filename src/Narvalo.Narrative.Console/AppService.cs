// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Serilog;

    public sealed class AppService
    {
        static readonly List<string> FilesToIgnore_ = new List<string> {
			"Designer.cs"
		};

        readonly AppSettings _settings;
        readonly Template _template;

        public AppService(AppSettings settings, Template template)
        {
            Require.NotNull(settings, "settings");
            Require.NotNull(template, "template");

            _settings = settings;
            _template = template;
        }

        public void Process(string[] paths)
        {
            if (paths.Length == 0) {
                Log.Warning("No path given.");
                return;
            }

            PrepareEnvironment_();

            var sources = from directoryPath in paths
                          from fileName in FindCSharpFilesInDirectory_(directoryPath)
                          select Tuple.Create(directoryPath, fileName);

            foreach (var source in sources) {
                ProcessSource_(source.Item1, source.Item2);
            }
        }

        static IEnumerable<string> FindCSharpFilesInDirectory_(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.cs", SearchOption.TopDirectoryOnly)
                .Where(_ => !FilesToIgnore_.Any(v => _.EndsWith(v, StringComparison.OrdinalIgnoreCase)))
                .Select(_ => new FileInfo(_).Name);
        }

        void ProcessSource_(string directoryPath, string fileName)
        {
            Log.Debug("Processing {File}...", fileName);

            var filePath = Path.Combine(directoryPath, fileName);
            var blocks = new CSharpFile(filePath).Parse();

            var output = _template.Render(new TemplateModel
            {
                Blocks = blocks,
                FileName = fileName,
            });

            SaveOutput_(fileName, output);
        }

        void PrepareEnvironment_()
        {
            var outputPath = _settings.OutputDirectory;

            if (!Directory.Exists(outputPath)) {
                Log.Debug("Creating output path: {Path}.", outputPath);

                Directory.CreateDirectory(outputPath);
            }
        }

        void SaveOutput_(string fileName, string text)
        {
            var outputFileName = Path.ChangeExtension(fileName, "html");
            var destination = Path.Combine(_settings.OutputDirectory, outputFileName);

            File.WriteAllText(destination, text);
        }
    }
}
