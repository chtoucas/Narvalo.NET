// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Configuration
{
    using CommandLine;

    // Cf. 
    // - http://testapi.codeplex.com/
    // - http://stackoverflow.com/questions/491595/best-way-to-parse-command-line-arguments-in-c
    public sealed class Arguments
    {
        [Option('p', "path", Required = true)]
        public string Path { get; set; }

        /// <summary>
        /// Perform a trial run with no change commited.
        /// </summary>
        [Option('n', "dry-run", DefaultValue = false)]
        public bool DryRun { get; set; }

        [Option('o', "output")]
        public string OutputDirectory { get; set; }

        // FIXME: Does not work...
        [Option('x', "parallel")]
        public bool? RunInParallel { get; set; }
    }
}
