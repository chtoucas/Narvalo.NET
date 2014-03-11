// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using CommandLine;
    using System.Diagnostics.CodeAnalysis;

    public sealed class AppOptions
    {
        [OptionArray]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] Paths { get; set; }
    }
}
