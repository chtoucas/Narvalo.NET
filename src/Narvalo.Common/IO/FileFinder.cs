// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;

    public class FileFinder
    {
        private readonly Func<DirectoryInfo, bool> _directoryFilter;
        private readonly Func<FileInfo, bool> _fileFilter;

        public FileFinder(
            Func<DirectoryInfo, bool> directoryFilter,
            Func<FileInfo, bool> fileFilter)
        {
            Require.NotNull(directoryFilter, "directoryFilter");
            Require.NotNull(fileFilter, "fileFilter");

            _directoryFilter = directoryFilter;
            _fileFilter = fileFilter;
        }

        public event EventHandler<RelativeDirectoryEventArgs> DirectoryStart;

        public event EventHandler<RelativeDirectoryEventArgs> DirectoryEnd;

        public IEnumerable<RelativeFile> Find(DirectoryInfo startDirectory, string searchPattern)
        {
            Require.NotNull(startDirectory, "startDirectory");
            Require.NotNullOrEmpty(searchPattern, "searchPattern");
            Contract.Ensures(Contract.Result<IEnumerable<RelativeFile>>() != null);

            var rootPath = PathUtility.AppendDirectorySeparator(startDirectory.FullName);
            var rootUri = new Uri(rootPath);

            var stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(rootPath));

            while (stack.Count > 0)
            {
                var directory = stack.Pop();

                var relativeDirectoryName
                    = PathUtility.MakeRelativePathInternal(rootUri, directory.FullName);
                var relativeDirectory = new RelativeDirectory(directory, relativeDirectoryName);

                OnDirectoryStart(new RelativeDirectoryEventArgs(relativeDirectory));

                var files = directory
                    .EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly)
                    .Where(_fileFilter);

                foreach (var file in files)
                {
                    yield return new RelativeFile(file, relativeDirectoryName);
                }

                OnDirectoryEnd(new RelativeDirectoryEventArgs(relativeDirectory));

                var subdirs = directory
                    .EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .Where(_directoryFilter);

                foreach (var dir in subdirs)
                {
                    stack.Push(dir);
                }
            }
        }

        protected virtual void OnDirectoryStart(RelativeDirectoryEventArgs e)
        {
            EventHandler<RelativeDirectoryEventArgs> localHandler = DirectoryStart;

            if (localHandler != null)
            {
                localHandler(this, e);
            }
        }

        protected virtual void OnDirectoryEnd(RelativeDirectoryEventArgs e)
        {
            EventHandler<RelativeDirectoryEventArgs> localHandler = DirectoryEnd;

            if (localHandler != null)
            {
                localHandler(this, e);
            }
        }

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "[CodeContracts] Object Invariants.")]
        private void ObjectInvariants()
        {
            Contract.Invariant(_directoryFilter != null);
            Contract.Invariant(_fileFilter != null);
        }
    }
}
