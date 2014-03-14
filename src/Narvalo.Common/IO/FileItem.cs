// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.IO
{
    using System.IO;

    public sealed class FileItem
    {
        readonly string _directory;
        readonly FileInfo _file;

        public FileItem(string directory, FileInfo file)
        {
            Require.NotNull(directory, "directory");
            Require.NotNull(file, "file");

            _directory = directory;
            _file = file;
        }

        public FileInfo File { get { return _file; } }

        public string RelativePath
        {
            get { return Path.Combine(_directory, _file.Name); }
        }
    }
}
