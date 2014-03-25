// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Configuration
{
    using System;
    using System.IO;
    using System.Reflection;
    using Serilog.Events;

    public sealed class Settings
    {
        bool _dryRun = false;
        LogEventLevel _logMinimumLevel = LogEventLevel.Information;
        string _outputDirectory;
        bool _runInParallel = true;

        internal Settings() { }

        public bool DryRun
        {
            get { return _dryRun; }

            internal set { _dryRun = value; }
        }

        [CLSCompliant(false)]
        public LogEventLevel LogMinimumLevel
        {
            get { return _logMinimumLevel; }

            internal set { _logMinimumLevel = value; }
        }

        public string OutputDirectory
        {
            get
            {
                if (_outputDirectory == null) {
                    _outputDirectory = GetDefaultOutputDirectory_();
                }

                return _outputDirectory;
            }

            internal set { _outputDirectory = value; }
        }

        public bool RunInParallel
        {
            get { return _runInParallel; }

            internal set { _runInParallel = value; }
        }

        static string GetDefaultOutputDirectory_()
        {
            var execDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return Path.Combine(execDirectory, "docs");
        }
    }
}
