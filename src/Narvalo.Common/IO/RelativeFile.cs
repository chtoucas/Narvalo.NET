// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;

    public sealed class RelativeFile
    {
        private readonly FileInfo _file;
        private readonly string _relativeDirectoryName;

        // REVIEW: Require.NotEmpty on relativeDirectoryName?
        public RelativeFile(FileInfo file, string relativeDirectoryName)
        {
            Require.NotNull(file, "file");
            Require.NotNull(relativeDirectoryName, "relativeDirectoryName");

            _file = file;
            _relativeDirectoryName = relativeDirectoryName;
        }

        public FileInfo File
        {
            get
            {
                Contract.Ensures(Contract.Result<FileInfo>() != null);
                return _file;
            }
        }

        public string RelativeName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return Path.Combine(_relativeDirectoryName, _file.Name);
            }
        }
        
#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_file != null);
            Contract.Invariant(_relativeDirectoryName != null);
        }

#endif
    }
}
