// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.IO;

    public sealed partial class RelativeDirectory
    {
        // REVIEW: Require.NotEmpty on relativeName?
        public RelativeDirectory(DirectoryInfo directory, string relativeName)
        {
            Require.NotNull(directory, nameof(directory));
            Require.NotNull(relativeName, nameof(relativeName));

            Directory = directory;
            RelativeName = relativeName;
        }

        public DirectoryInfo Directory { get; }

        public string RelativeName { get; }
    }
}
