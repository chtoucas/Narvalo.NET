// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class DirectoryWalker
    {
        readonly Func<FileInfo, bool> _fileFilter;
        readonly Func<DirectoryInfo, bool> _directoryFilter;

        public DirectoryWalker(Func<DirectoryInfo, bool> directoryFilter, Func<FileInfo, bool> fileFilter)
        {
            Require.NotNull(directoryFilter, "directoryFilter");
            Require.NotNull(fileFilter, "fileFilter");

            _fileFilter = fileFilter;
            _directoryFilter = directoryFilter;
        }

        public event EventHandler<SubfolderEventArgs> OnSubfolder;

        public IEnumerable<FileItem> Walk(DirectoryInfo rootDirectory, string searchPattern)
        {
            Require.NotNull(rootDirectory, "rootDirectory");
            Require.NotNullOrEmpty(searchPattern, "searchPattern");

            if (!rootDirectory.Exists) {
                throw new DirectoryNotFoundException("FIXME");
            }

            return WalkCore_(new DirectoryInfo(rootDirectory.GetNormalizedPath()), searchPattern);
        }

        protected virtual void OnEnteringSubfolder(SubfolderEventArgs e)
        {
            EventHandler<SubfolderEventArgs> localHandler = OnSubfolder;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

        IEnumerable<FileItem> WalkCore_(DirectoryInfo rootDirectory, string searchPattern)
        {
            var rootUri = new Uri(rootDirectory.FullName);

            var stack = new Stack<DirectoryInfo>();
            stack.Push(rootDirectory);

            while (stack.Count > 0) {
                var directory = stack.Pop();
                var relativePath = directory.GetRelativePathTo(rootUri);

                OnEnteringSubfolder(new SubfolderEventArgs(relativePath));

                var files = directory
                    .EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly)
                    .Where(_fileFilter);

                foreach (var file in files) {
                    yield return new FileItem(relativePath, file.Name);
                }

                var subdirs = directory
                    .EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .Where(_directoryFilter);

                foreach (var dir in subdirs) {
                    stack.Push(dir);
                }
            }
        }
    }
}
