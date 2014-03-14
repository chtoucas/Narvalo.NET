// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public abstract class DirectoryWalker
    {
        readonly Func<FileInfo, bool> _fileFilter;
        readonly Func<DirectoryInfo, bool> _directoryFilter;

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

            while (stack.Count > 0) {
                var directory = stack.Pop();

                OnDirectoryStart(directory);

                var files = directory
                    .EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly)
                    .Where(_fileFilter);

                foreach (var file in files) {
                    OnFile(file);
                }

                OnDirectoryEnd(directory);

                var subdirs = directory
                    .EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .Where(_directoryFilter);

                foreach (var subdir in subdirs) {
                    stack.Push(subdir);
                }
            }
        }

        protected abstract void OnDirectoryStart(DirectoryInfo directory);

        protected abstract void OnDirectoryEnd(DirectoryInfo directory);

        protected abstract void OnFile(FileInfo file);
    }
}
