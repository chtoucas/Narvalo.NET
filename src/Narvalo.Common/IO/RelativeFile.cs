// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.Diagnostics.Contracts;
    using System.IO;

    public sealed class RelativeFile
    {
        readonly FileInfo _file;
        readonly string _relativeDirectoryName;

        // REVIEW: Require.NotEmpty on relativeDirectoryName?
        public RelativeFile(FileInfo file, string relativeDirectoryName)
        {
            Require.NotNull(file, "file");
            Require.NotNull(relativeDirectoryName, "relativeDirectoryName");

            _file = file;
            _relativeDirectoryName = relativeDirectoryName;
        }

        public FileInfo File { get { return _file; } }

        public string RelativeName
        {
            get { return Path.Combine(_relativeDirectoryName, _file.Name); }
        }

#if CONTRACTS_FULL
        [ContractInvariantMethod]
        void ObjectInvariants()
        {
            Contract.Invariant(_file != null);
            Contract.Invariant(_relativeDirectoryName != null);
        }
#endif
    }
}
