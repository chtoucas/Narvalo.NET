// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Diagnostics.CodeAnalysis;
    using CommandLine;

    public sealed class CommandOptions
    {
        [Option('d', Required = true)]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string Directory { get; set; }

        public static CommandOptions Parse(string[] args)
        {
            var self = new CommandOptions();

            var parser = new CommandLine.Parser();
            parser.ParseArgumentsStrict(
                args,
                self,
                () => { throw new NarrativeException("Failed to parse the cmdline arguments."); });

            return self;
        }
    }
}
