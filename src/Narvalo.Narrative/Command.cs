// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Narvalo.IO;
    using Serilog;

    public sealed class Command
    {
        static readonly List<string> DirectoriesToIgnore_ = new List<string> { "bin", "obj", "_Aliens" };
        static readonly Func<DirectoryInfo, bool> DirectoryFilter_
             = _ => !DirectoriesToIgnore_.Any(s => _.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
        static readonly Func<FileInfo, bool> FileFilter_
             = _ => !_.Name.EndsWith("Designer.cs", StringComparison.OrdinalIgnoreCase);

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

            var rootPath = options.Directory;

            if (rootPath.Length == 0) {
                Log.Warning("No path given.");
                return;
            }

            var rootDirectory = new DirectoryInfo(rootPath);

            if (!rootDirectory.Exists) {
                throw new DirectoryNotFoundException("FIXME");
            }

            var finder = new FileFinder(DirectoryFilter_, FileFilter_);
            finder.EnteringSubfolder += (sender, e) =>
            {
                Log.Debug("Entering {RelativePath}", e.RelativePath);

                var folderPath = Path.Combine(_settings.OutputDirectory, e.RelativePath);
                //Directory.CreateDirectory(folderPath);
            };

            var files = finder.Find(rootDirectory, "*.cs");

            foreach (var file in files) {
                Log.Debug("Processing {RelativePath}", file.RelativePath);

                //var blocks = Parse_(rootPath, file);
                //var output = Render_(blocks, file);

                //Save_(file.RelativePath, output);
            }
        }

        IEnumerable<Block> Parse_(string rootPath, FileItem item)
        {
            var filePath = Path.Combine(rootPath, item.RelativePath);
            var parser = new SourceParser(filePath);
            return parser.Parse();
        }

        string Render_(IEnumerable<Block> blocks, FileItem item)
        {
            var data = new TemplateData(blocks)
            {
                Title = item.RelativePath,
            };

            return _template.Render(data);
        }

        void Save_(string relativePath, string text)
        {
            var fileName = Path.ChangeExtension(relativePath, "html");
            var destination = Path.Combine(_settings.OutputDirectory, fileName);

            File.WriteAllText(destination, text);
        }
    }
}
