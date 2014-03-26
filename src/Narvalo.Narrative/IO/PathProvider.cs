// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.IO
{
    using System.IO;
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
            return Path.Combine(_outputDirectory, directory.RelativeName);
        }

        public string GetFilePath(RelativeFile file)
        {
            return Path.ChangeExtension(Path.Combine(_outputDirectory, file.RelativeName), "html");
        }

        public string GetFilePath(FileInfo file)
        {
            return Path.ChangeExtension(Path.Combine(_outputDirectory, file.Name), "html");
        }
    }
}
