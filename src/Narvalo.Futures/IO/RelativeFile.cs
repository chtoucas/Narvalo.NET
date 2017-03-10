// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.IO;

    public sealed partial class RelativeFile
    {
        private readonly string _relativeDirectoryName;

        // REVIEW: Require.NotEmpty on relativeDirectoryName?
        public RelativeFile(FileInfo file, string relativeDirectoryName)
        {
            Require.NotNull(file, nameof(file));
            Require.NotNull(relativeDirectoryName, nameof(relativeDirectoryName));

            File = file;
            _relativeDirectoryName = relativeDirectoryName;
        }

        public FileInfo File { get; }

        public string RelativeName => Path.Combine(_relativeDirectoryName, _file.Name);
    }
}
