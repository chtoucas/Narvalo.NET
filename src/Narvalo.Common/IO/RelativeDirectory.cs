// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.IO;

    public sealed class RelativeDirectory
    {
        readonly DirectoryInfo _directory;
        readonly string _relativeName;

        public RelativeDirectory(DirectoryInfo directory, string relativeName)
        {
            Require.NotNull(directory, "directory");
            Require.NotNull(relativeName, "relativeName");

            _directory = directory;
            _relativeName = relativeName;
        }

        public DirectoryInfo Directory { get { return _directory; } }

        public string RelativeName
        {
            get { return _relativeName; }
        }
    }
}
