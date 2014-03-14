// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FileFinder
    {
        readonly Func<FileInfo, bool> _fileFilter;
        readonly Func<DirectoryInfo, bool> _directoryFilter;

        public FileFinder(
            Func<DirectoryInfo, bool> directoryFilter,
            Func<FileInfo, bool> fileFilter)
        {
            Require.NotNull(directoryFilter, "directoryFilter");
            Require.NotNull(fileFilter, "fileFilter");

            _directoryFilter = directoryFilter;
            _fileFilter = fileFilter;
        }

        public event EventHandler<SubfolderEventArgs> EnteringSubfolder;

        public IEnumerable<FileItem> Find(DirectoryInfo startDirectory, string searchPattern)
        {
            Require.NotNull(startDirectory, "startDirectory");
            Require.NotNullOrEmpty(searchPattern, "searchPattern");

            return FindCore_(new DirectoryInfo(startDirectory.GetNormalizedPath()), searchPattern);
        }

        protected virtual void OnEnteringSubfolder(SubfolderEventArgs e)
        {
            EventHandler<SubfolderEventArgs> localHandler = EnteringSubfolder;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

        IEnumerable<FileItem> FindCore_(DirectoryInfo startDirectory, string searchPattern)
        {
            var startUri = new Uri(startDirectory.FullName);

            var stack = new Stack<DirectoryInfo>();
            stack.Push(startDirectory);

            while (stack.Count > 0) {
                var subfolder = stack.Pop();
                var subfolderPath = subfolder.GetRelativePathTo(startUri);

                OnEnteringSubfolder(new SubfolderEventArgs(subfolderPath));

                var files = subfolder
                    .EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly)
                    .Where(_fileFilter);

                foreach (var file in files) {
                    yield return new FileItem(subfolderPath, file);
                }

                var folders = subfolder
                    .EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .Where(_directoryFilter);

                foreach (var folder in folders) {
                    stack.Push(folder);
                }
            }
        }
    }
}
