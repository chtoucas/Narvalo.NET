// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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

        public event EventHandler<RelativeDirectoryEventArgs> TraversingDirectory;

        public IEnumerable<RelativeFile> Find(DirectoryInfo startDirectory, string searchPattern)
        {
            Require.NotNull(startDirectory, "startDirectory");
            Require.NotNullOrEmpty(searchPattern, "searchPattern");

            var rootPath = PathUtility.GetNormalizedPath(startDirectory);
            var rootUri = new Uri(rootPath);

            var stack = new ConcurrentStack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(rootPath));

            while (!stack.IsEmpty) {
                DirectoryInfo directory;
                if (!stack.TryPop(out directory)) {
                    // REVIEW: What are the conditions that might cause TryPop to fail.
                    // Is it safe to use stack.TryPop with while?
                    yield break;
                }

                var relativeDirectoryName 
                    = PathUtility.MakeRelativePathInternal(rootUri, directory.FullName);
                var relativeDirectory = new RelativeDirectory(directory, relativeDirectoryName);

                OnTraversingDirectory(new RelativeDirectoryEventArgs(relativeDirectory));

                var files = directory
                    .EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly)
                    .Where(_fileFilter);

                foreach (var file in files) {
                    yield return new RelativeFile(file, relativeDirectoryName);
                }

                var subdirs = directory
                    .EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .Where(_directoryFilter);

                foreach (var dir in subdirs) {
                    stack.Push(dir);
                }
            }
        }

        protected virtual void OnTraversingDirectory(RelativeDirectoryEventArgs e)
        {
            // NB: Not really necessary to use Interlocked.CompareExchange as the .NET
            // compiler understands this pattern.
            EventHandler<RelativeDirectoryEventArgs> localHandler
                = Interlocked.CompareExchange(ref TraversingDirectory, null, null);

            if (localHandler != null) {
                localHandler(this, e);
            }
        }
    }
}
