// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FileFinder
    {
        readonly Func<DirectoryInfo, bool> _directoryFilter;
        readonly Func<FileInfo, bool> _fileFilter;

        public FileFinder(
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

            var stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(rootPath));

            while (stack.Count > 0) {
                var directory = stack.Pop();

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
            EventHandler<RelativeDirectoryEventArgs> localHandler = TraversingDirectory;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }
    }
}
