// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;

    public sealed class SourceFile
    {
        readonly string _directory;
        readonly string _name;

        public SourceFile(string directory, string name)
        {
            Require.NotNullOrEmpty(directory, "directory");
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
