// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narrative.IO
{
    using System.IO;
    using Narvalo;
    using Narvalo.IO;

    public sealed class PathProvider : IPathProvider
    {
        readonly string _outputDirectory;

        public PathProvider(string outputDirectory)
        {
            Require.NotNullOrEmpty(outputDirectory, "outputDirectory");

            _outputDirectory = outputDirectory;
        }

        public string GetDirectoryPath(RelativeDirectory directory)
        {
            return GetPath_(directory.RelativeName);
        }

        public string GetFilePath(RelativeFile file)
        {
            return GetFilePath_(file.RelativeName);
        }

        public string GetFilePath(FileInfo file)
        {
            return GetFilePath_(file.Name);
        }

        string GetFilePath_(string fileName)
        {
            return Path.ChangeExtension(GetPath_(fileName), "html");
        }

        string GetPath_(string path)
        {
            return Path.Combine(_outputDirectory, path);
        }
    }
}
