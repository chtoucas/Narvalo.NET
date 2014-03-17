// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;

    public sealed class Runner : IRunner
    {
        readonly IWeaver _weaver;
        readonly FileInfo _file;
        readonly string _outputDirectory;
        bool _dryRun = false;

        public Runner(IWeaver weaver, FileInfo file, string outputDirectory)
        {
            _weaver = weaver;
            _file = file;
            _outputDirectory = outputDirectory;
        }

        public bool DryRun { get { return _dryRun; } set { _dryRun = value; } }

        public void Run()
        {
            var outputPath = Path.Combine(_outputDirectory, _file.Name);

            _weaver.Weave(_file, outputPath);
        }
    }
}
