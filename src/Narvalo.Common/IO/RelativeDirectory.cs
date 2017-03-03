// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.IO;

    public sealed partial class RelativeDirectory
    {
        private readonly DirectoryInfo _directory;
        private readonly string _relativeName;

        // REVIEW: Require.NotEmpty on relativeName?
        public RelativeDirectory(DirectoryInfo directory, string relativeName)
        {
            Require.NotNull(directory, nameof(directory));
            Require.NotNull(relativeName, nameof(relativeName));

            _directory = directory;
            _relativeName = relativeName;
        }

        public DirectoryInfo Directory
        {
            get
            {
                return _directory;
            }
        }

        public string RelativeName
        {
            get
            {
                return _relativeName;
            }
        }
    }
}
