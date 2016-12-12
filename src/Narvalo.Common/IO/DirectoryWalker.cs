// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;

    public abstract partial class DirectoryWalker
    {
        private readonly Func<DirectoryInfo, bool> _directoryFilter;
        private readonly Func<FileInfo, bool> _fileFilter;

        protected DirectoryWalker(
            Func<DirectoryInfo, bool> directoryFilter,
            Func<FileInfo, bool> fileFilter)
        {
            Require.NotNull(directoryFilter, nameof(directoryFilter));
            Require.NotNull(fileFilter, nameof(fileFilter));

            _directoryFilter = directoryFilter;
            _fileFilter = fileFilter;
        }

        public void Walk(DirectoryInfo startDirectory, string searchPattern)
        {
            Require.NotNull(startDirectory, nameof(startDirectory));
            Require.NotNullOrEmpty(searchPattern, nameof(searchPattern));

            var stack = new Stack<DirectoryInfo>();
            stack.Push(startDirectory);

            while (stack.Count > 0)
            {
                var directory = stack.Pop();

                if (directory == null)
                {
                    // XXX: Fix the message
                    throw new InvalidOperationException();
                }

                OnDirectoryStart(directory);

                var files = directory
                    .EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
                Contract.Assume(files != null);

                foreach (var file in files.Where(_fileFilter))
                {
                    OnFile(file);
                }

                OnDirectoryEnd(directory);

                var subdirs = directory
                    .EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
                Contract.Assume(subdirs != null);

                foreach (var dir in subdirs.Where(_directoryFilter))
                {
                    stack.Push(dir);
                }
            }
        }

        protected abstract void OnDirectoryStart(DirectoryInfo directory);

        protected abstract void OnDirectoryEnd(DirectoryInfo directory);

        protected abstract void OnFile(FileInfo file);
    }
}

#if CONTRACTS_FULL

namespace Narvalo.IO
{
    using System.Diagnostics.Contracts;

    public abstract partial class DirectoryWalker
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_directoryFilter != null);
            Contract.Invariant(_fileFilter != null);
        }
    }
}

#endif
