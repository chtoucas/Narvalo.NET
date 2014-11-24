// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace DocuMaker.Narrator
{
    using CommandLine;

    // Cf. http://testapi.codeplex.com/
    public sealed class Arguments
    {
        bool _dryRun = false;
        bool _dryRunSet = false;
        bool _runInParallel = true;
        bool _runInParallelSet = false;

        [Option('p', "path", Required = true)]
        public string Path { get; set; }

        /// <summary>
        /// Perform a trial run with no change commited.
        /// </summary>
        [Option('n', "dry-run")]
        public bool DryRun
        {
            get
            {
                return _dryRun;
            }

            set
            {
                _dryRunSet = true;
                _dryRun = value;
            }
        }

        [Option('X', "parallel")]
        public bool RunInParallel
        {
            get
            {
                return _runInParallel;
            }

            set
            {
                _runInParallelSet = true;
                _runInParallel = value;
            }
        }

        internal bool DryRunSet { get { return _dryRunSet; } }

        internal bool RunInParallelSet { get { return _runInParallelSet; } }
    }
}
