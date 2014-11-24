// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace DocuMaker.IO
{
    using System.IO;
    using Narvalo;
    using Narvalo.IO;

    public sealed class OutputWriter : IOutputWriter
    {
        readonly IPathProvider _pathProvider;

        public OutputWriter(IPathProvider pathProvider)
        {
            Require.NotNull(pathProvider, "pathProvider");

            _pathProvider = pathProvider;
        }

        public void CreateDirectory(RelativeDirectory directory)
        {
            var path = _pathProvider.GetDirectoryPath(directory);

            Directory.CreateDirectory(path);
        }

        public void Write(RelativeFile file, string content)
        {
            var path = _pathProvider.GetFilePath(file);

            File.WriteAllText(path, content);
        }

        public void Write(FileInfo file, string content)
        {
            var path = _pathProvider.GetFilePath(file);

            File.WriteAllText(path, content);
        }
    }
}
