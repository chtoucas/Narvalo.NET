// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Threading;

    public class ConcurrentFileFinder
    {
        readonly Func<DirectoryInfo, bool> _directoryFilter;
        readonly Func<FileInfo, bool> _fileFilter;

        public ConcurrentFileFinder(
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

            var rootPath = PathUtility.AppendDirectorySeparator(startDirectory.FullName);
            var rootUri = new Uri(rootPath);

            var stack = new ConcurrentStack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(rootPath));

            while (!stack.IsEmpty) {
                DirectoryInfo directory;

                if (!stack.TryPop(out directory)) {
                    // REVIEW: What are the conditions that might cause TryPop to fail?
                    // Corollary: is it safe to use stack.TryPop directly to update a "shared" 
                    // directory variable?
                    yield break;
                }

                var relativeDirectoryName 
                    = PathUtility.MakeRelativePathInternal(rootUri, directory.FullName);
                var relativeDirectory = new RelativeDirectory(directory, relativeDirectoryName);

                OnDirectoryStart(new RelativeDirectoryEventArgs(relativeDirectory));

                var files = directory
                    .EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly)
                    .Where(_fileFilter);

                foreach (var file in files) {
                    yield return new RelativeFile(file, relativeDirectoryName);
                }

                OnDirectoryEnd(new RelativeDirectoryEventArgs(relativeDirectory));

                var subdirs = directory
                    .EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .Where(_directoryFilter);

                foreach (var dir in subdirs) {
                    stack.Push(dir);
                }
            }
        }

        protected virtual void OnDirectoryStart(RelativeDirectoryEventArgs e)
        {
            // NB: It is not really necessary to use Interlocked.CompareExchange
            // as the .NET compiler understands this pattern.
            EventHandler<RelativeDirectoryEventArgs> localHandler
                = Interlocked.CompareExchange(ref DirectoryStart, null, null);

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

        protected virtual void OnDirectoryEnd(RelativeDirectoryEventArgs e)
        {
            EventHandler<RelativeDirectoryEventArgs> localHandler
                = Interlocked.CompareExchange(ref DirectoryEnd, null, null);

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

#if CONTRACTS_FULL
        [ContractInvariantMethod]
        void ObjectInvariants()
        {
            Contract.Invariant(_directoryFilter != null);
            Contract.Invariant(_fileFilter != null);
        }
#endif
    }
}
