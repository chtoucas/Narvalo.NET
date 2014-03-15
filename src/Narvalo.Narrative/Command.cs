// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
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

            var path = options.Directory;

            if (path.Length == 0) {
                Log.Warning("No path given.");
                return;
            }

            var directory = new DirectoryInfo(path);

            if (!directory.Exists) {
                throw new DirectoryNotFoundException("FIXME");
            }

            //RunSequential_(directory);
            RunParallel_(directory);
        }

        void RunSequential_(DirectoryInfo directory)
        {
            var finder = new FileFinder(DirectoryFilter_, FileFilter_);
            finder.TraversingDirectory += OnTraversingDirectory;

            var files = finder.Find(directory, "*.cs");

            foreach (var file in files) {
                ProcessFile_(file);
            }

            //finder.TraversingDirectory -= OnTraversingDirectory;
        }

        void RunParallel_(DirectoryInfo directory)
        {
            var finder = new ConcurrentFileFinder(DirectoryFilter_, FileFilter_);
            finder.TraversingDirectory += OnTraversingDirectory;

            var files = finder.Find(directory, "*.cs");

            Parallel.ForEach(files, ProcessFile_);

            //finder.TraversingDirectory -= OnTraversingDirectory;
        }

        void ProcessFile_(RelativeFile file)
        {
            Log.Debug("Processing file {RelativeName}", file.RelativeName);

            var parser = new SourceParser(file.File.FullName);
            var blocks = parser.Parse();

            var data = new TemplateData(blocks)
            {
                Title = file.RelativeName,
            };

            //var output = _template.Render(data);

            //var destination = GetOutputPath_(file);

            //File.WriteAllText(destination, output);
        }

        void OnTraversingDirectory(object sender, RelativeDirectoryEventArgs e)
        {
            Log.Debug("Entering directory {RelativeName}", e.RelativeDirectory.RelativeName);

            //var newPath = Path.Combine(_settings.OutputDirectory, e.RelativeDirectory.RelativeName);

            //Directory.CreateDirectory(newPath);
        }

        string GetOutputPath_(RelativeFile file)
        {
            var newPath = Path.Combine(_settings.OutputDirectory, file.RelativeName);

            return Path.ChangeExtension(newPath, "html");
        }
    }
}
