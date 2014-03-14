// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;

    public sealed class FileItem
    {
        readonly string _directory;
        readonly string _name;

        public FileItem(string directory, string name)
        {
            Require.NotNull(directory, "directory");
            Require.NotNullOrEmpty(name, "name");

            _directory = directory;
            _name = name;
        }

        public string Directory { get { return _directory; } }
        public string Name { get { return _name; } }

        public string RelativePath
        {
            get { return Path.Combine(Directory, Name); }
        }
    }
}
