// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Serilog;

    public sealed class Command
    {
        readonly AppSettings _settings;
        readonly RazorTemplate _template;

        public Command(AppSettings settings, RazorTemplate template)
        {
            Require.NotNull(settings, "settings");
            Require.NotNull(template, "template");

            _settings = settings;
            _template = template;
        }

        public void Run(CommandOptions options)
        {
            Require.NotNull(options, "options");

            var path = options.Directory;

            if (path.Length == 0) {
                Log.Warning("No path given.");
                return;
            }

            var sources = new SourceDirectory(path).FindSources();

            PrepareOutput_(sources);

            foreach (var source in sources) {
                ProcessSource_(path, source);
            }
        }

        void ProcessSource_(string directory, SourceFile source)
        {
            Log.Debug("Processing {File}...", source.RelativePath);

            var filePath = Path.Combine(directory, source.RelativePath);
            var parser = new SourceParser(filePath);
            var blocks = parser.Parse();

            var data = new TemplateData(blocks)
            {
                Title = source.RelativePath,
            };

            var output = _template.Render(data);

            SaveOutput_(source.RelativePath, output);
        }

        void PrepareOutput_(IEnumerable<SourceFile> sources)
        {
            var directories
                = (from source in sources
                   select source.Directory)
                   .Distinct()
                   .Select(_ => Path.Combine(_settings.OutputDirectory, _));

            CreateMissingDirectories_(directories);
        }

        void CreateMissingDirectories_(IEnumerable<string> directories)
        {
            foreach (var directory in directories) {
                Directory.CreateDirectory(directory);
            }
        }

        void SaveOutput_(string relativePath, string text)
        {
            var fileName = Path.ChangeExtension(relativePath, "html");
            var destination = Path.Combine(_settings.OutputDirectory, fileName);

            File.WriteAllText(destination, text);
        }
    }
}
