// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Narvalo.IO;
    using NodaTime;
    using Serilog;

    public sealed class CmdLine
    {
        static readonly List<string> DirectoriesToIgnore_ = new List<string> { "bin", "obj", "_Aliens" };
        static readonly Func<DirectoryInfo, bool> DirectoryFilter_
             = _ => !DirectoriesToIgnore_.Any(s => _.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
        static readonly Func<FileInfo, bool> FileFilter_
             = _ => !_.Name.EndsWith("Designer.cs", StringComparison.OrdinalIgnoreCase);

        readonly CmdLineOptions _options;
        readonly RazorTemplate _template;

        public CmdLine(CmdLineOptions options, RazorTemplate template)
        {
            Require.NotNull(options, "options");
            Require.NotNull(template, "template");

            _options = options;
            _template = template;
        }

        bool DryRun { get { return _options.DryRun; } }

        string OutputDirectory { get { return _options.OutputDirectory; } }

        string Path { get { return _options.Path; } }

        bool RunInParallel { get { return _options.RunInParallel; } }

        bool Verbose { get { return _options.Verbose; } }

        public void Run()
        {
            if (DryRun) {
                Log.Warning("Dry run.");
            }

            if (Path.Length == 0) {
                Log.Error("No path given.");
                return;
            }

            var attrs = File.GetAttributes(Path);

            var stopWatch = Stopwatch.StartNew();

            if (attrs.HasFlag(FileAttributes.Directory)) {
                ProcessDirectory_(new DirectoryInfo(Path));
            }
            else {
                ProcessFile_(new FileInfo(Path));
            }

            var elapsedTime = Duration.FromTicks(stopWatch.Elapsed.Ticks);
            Log.Information("Elapsed time: {ElapsedTime}.", elapsedTime);
        }

        void ProcessFile_(FileInfo file)
        {
            ProcessFile_(new RelativeFile(file, String.Empty));
        }

        void ProcessFile_(RelativeFile file)
        {
            var parser = new SourceParser(file.File.FullName);
            var blocks = parser.Parse();

            var data = new TemplateData(blocks)
            {
                Title = file.RelativeName,
            };

            var output = _template.Render(data);

            var destination = GetOutputPath_(file);

            if (!DryRun) {
                File.WriteAllText(destination, output);
            }
        }

        void ProcessDirectory_(DirectoryInfo directory)
        {
            if (RunInParallel) {
                ProcessDirectoryInParallel_(directory);
            }
            else {
                ProcessDirectorySequentially_(directory);
            }
        }

        void ProcessDirectorySequentially_(DirectoryInfo directory)
        {
            var finder = new FileFinder(DirectoryFilter_, FileFilter_);
            finder.TraversingDirectory += OnTraversingDirectory_;

            var files = finder.Find(directory, "*.cs");

            foreach (var file in files) {
                ProcessFile_(file);
            }

            //finder.TraversingDirectory -= OnTraversingDirectory_;
        }

        void ProcessDirectoryInParallel_(DirectoryInfo directory)
        {
            var finder = new ConcurrentFileFinder(DirectoryFilter_, FileFilter_);
            finder.TraversingDirectory += OnTraversingDirectory_;

            var files = finder.Find(directory, "*.cs");

            Parallel.ForEach(files, ProcessFile_);

            //finder.TraversingDirectory -= OnTraversingDirectory_;
        }

        void OnTraversingDirectory_(object sender, RelativeDirectoryEventArgs e)
        {
            var targetDirectoryPath = System.IO.Path.Combine(OutputDirectory, e.RelativeDirectory.RelativeName);

            if (!DryRun) {
                Directory.CreateDirectory(targetDirectoryPath);
            }
        }

        string GetOutputPath_(RelativeFile file)
        {
            var outputPath = System.IO.Path.Combine(OutputDirectory, file.RelativeName);

            return System.IO.Path.ChangeExtension(outputPath, "html");
        }
    }
}
