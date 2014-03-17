// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Reflection;
    using CommandLine;
    using Narvalo.Narrative.Properties;

    using IO = System.IO;

    sealed class Arguments
    {
        [Option('p', Required = true)]
        public string Path { get; set; }

        [Option('n', Required = false, DefaultValue = false)]
        public bool DryRun { get; set; }

        [Option('o', Required = false)]
        public string OutputDirectory { get; set; }

        [Option('m', Required = false, DefaultValue = false)]
        public bool RunInParallel { get; set; }

        [Option('v', Required = false, DefaultValue = false)]
        public bool Verbose { get; set; }

        public static Arguments Parse(string[] args)
        {
            var self = new Arguments();

            var parser = new CommandLine.Parser();
            parser.ParseArgumentsStrict(
                args,
                self,
                () => { throw new NarrativeException(Resources.CommandLineException); });

            self.PostProcess_();

            return self;
        }

        void PostProcess_()
        {
            if (OutputDirectory == null) {
                var execDirectory = IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                OutputDirectory = IO.Path.Combine(execDirectory, "docs");
            }
        }
    }
}
