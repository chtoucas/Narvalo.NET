// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Narvalo.IO;

    public abstract class DirectoryRunnerBase : IRunner
    {
        static readonly List<string> DirectoriesToIgnore_ = new List<string> { "bin", "obj", "_Aliens" };

        protected static readonly Func<DirectoryInfo, bool> DirectoryFilter
             = _ => !DirectoriesToIgnore_.Any(s => _.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
        protected static readonly Func<FileInfo, bool> FileFilter
             = _ => !_.Name.EndsWith("Designer.cs", StringComparison.OrdinalIgnoreCase);

        readonly IWeaver _weaver;
        readonly DirectoryInfo _directory;
        readonly string _outputDirectory;
        bool _dryRun = false;

        protected DirectoryRunnerBase(IWeaver weaver, DirectoryInfo directory, string outputDirectory)
        {
            _weaver = weaver;
            _directory = directory;
            _outputDirectory = outputDirectory;
        }

        public bool DryRun { get { return _dryRun; } set { _dryRun = value; } }

        protected DirectoryInfo Directory { get { return _directory; } }

        public abstract void Run();

        protected void RunCore(RelativeFile file)
        {
            var outputPath = Path.Combine(_outputDirectory, file.RelativeName);
            var outputFile = Path.ChangeExtension(outputPath, "html");

            _weaver.Weave(file.File, outputFile);
        }

        protected void OnDirectoryStart(object sender, RelativeDirectoryEventArgs e)
        {
            var targetDirectoryPath = Path.Combine(_outputDirectory, e.RelativeDirectory.RelativeName);

            if (!DryRun) {
                System.IO.Directory.CreateDirectory(targetDirectoryPath);
            }
        }
    }
}
