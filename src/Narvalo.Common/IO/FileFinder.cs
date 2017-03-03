// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public partial class FileFinder
    {
        private readonly Func<DirectoryInfo, bool> _directoryFilter;
        private readonly Func<FileInfo, bool> _fileFilter;

        public FileFinder(
            Func<DirectoryInfo, bool> directoryFilter,
            Func<FileInfo, bool> fileFilter)
        {
            Require.NotNull(directoryFilter, nameof(directoryFilter));
            Require.NotNull(fileFilter, nameof(fileFilter));

            _directoryFilter = directoryFilter;
            _fileFilter = fileFilter;
        }

        public event EventHandler<RelativeDirectoryEventArgs> DirectoryStart;

        public event EventHandler<RelativeDirectoryEventArgs> DirectoryEnd;

        public IEnumerable<RelativeFile> Find(DirectoryInfo startDirectory, string searchPattern)
        {
            Require.NotNull(startDirectory, nameof(startDirectory));
            Require.NotNullOrEmpty(searchPattern, nameof(searchPattern));

            var rootPath = PathHelpers.AppendDirectorySeparator(startDirectory.FullName);
            var rootUri = new Uri(rootPath);

            var stack = new Stack<DirectoryInfo>();
            stack.Push(new DirectoryInfo(rootPath));

            while (stack.Count > 0)
            {
                var directory = stack.Pop();

                var relativeDirectoryName
                    = PathHelpers.MakeRelativePathCore(rootUri, directory.FullName);
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
            => DirectoryStart?.Invoke(this, e);

        protected virtual void OnDirectoryEnd(RelativeDirectoryEventArgs e)
            => DirectoryEnd?.Invoke(this, e);
    }
}
