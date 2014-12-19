// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Narrator
{
    using System;
    using Serilog.Events;

    public sealed class Settings
    {
        bool _dryRun = false;
        LogEventLevel _logMinimumLevel = LogEventLevel.Information;
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

        public string OutputDirectory { get; internal set; }

        public bool RunInParallel
        {
            get { return _runInParallel; }
            internal set { _runInParallel = value; }
        }

        public string Path { get; internal set; }
    }
}
