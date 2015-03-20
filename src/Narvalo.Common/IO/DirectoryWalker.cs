// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Generic;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif
    using System.IO;
    using System.Linq;

    using Narvalo.Internal;

    public abstract class DirectoryWalker
    {
        private readonly Func<DirectoryInfo, bool> _directoryFilter;
        private readonly Func<FileInfo, bool> _fileFilter;

        protected DirectoryWalker(
            Func<DirectoryInfo, bool> directoryFilter,
            Func<FileInfo, bool> fileFilter)
        {
            Require.NotNull(directoryFilter, "directoryFilter");
            Require.NotNull(fileFilter, "fileFilter");

            _directoryFilter = directoryFilter;
            _fileFilter = fileFilter;
        }

        public void Walk(DirectoryInfo startDirectory, string searchPattern)
        {
            Require.NotNull(startDirectory, "startDirectory");
            Require.NotNullOrEmpty(searchPattern, "searchPattern");

            var stack = new Stack<DirectoryInfo>();
            stack.Push(startDirectory);

            while (stack.Count > 0)
            {
                var directory = stack.Pop();

                if (directory == null)
                {
                    throw new InvalidOperationException("FIXME");
                }

                OnDirectoryStart(directory);

                var files = directory
                    .EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly)
                    .AssumeNotNull()
                    .Where(_fileFilter);

                foreach (var file in files)
                {
                    OnFile(file);
                }

                OnDirectoryEnd(directory);

                var subdirs = directory
                    .EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .AssumeNotNull()
                    .Where(_directoryFilter);

                foreach (var dir in subdirs)
                {
                    stack.Push(dir);
                }
            }
        }

        protected abstract void OnDirectoryStart(DirectoryInfo directory);

        protected abstract void OnDirectoryEnd(DirectoryInfo directory);

        protected abstract void OnFile(FileInfo file);
        
#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_directoryFilter != null);
            Contract.Invariant(_fileFilter != null);
        }

#endif
    }
}
