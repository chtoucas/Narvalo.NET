// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Narvalo.Collections;

    public sealed class SourceDirectory
    {
        static readonly List<string> FilesToIgnore_ = new List<string> {
			"Designer.cs"
		};
        static readonly List<string> FoldersToIgnore_ = new List<string> {
			@"bin",
            @"obj",
		};

        readonly string _path;

        public SourceDirectory(string path)
        {
            Require.NotNullOrEmpty(path, "path");

            _path = path;
        }

        public IEnumerable<SourceFile> Find()
        {
            return from dir in FindSubDirectories_()
                   from file in FindSources_(Path.Combine(_path, dir))
                   select new SourceFile
                   {
                       Directory = dir,
                       FileName = file
                   };
        }

        IEnumerable<string> FindSubDirectories_()
        {
            var path = new FileInfo(_path).FullName;

            return Directory.GetDirectories(path, "*", SearchOption.AllDirectories)
                .Select(_ => _.Replace(path + Path.DirectorySeparatorChar, String.Empty))
                .Where(_ => !FoldersToIgnore_.Any(s => _.StartsWith(s, StringComparison.OrdinalIgnoreCase)))
                .Append(".")
                ;
        }

        static IEnumerable<string> FindSources_(string path)
        {
            return Directory.GetFiles(path, "*.cs", SearchOption.TopDirectoryOnly)
                .Where(_ => !FilesToIgnore_.Any(s => _.EndsWith(s, StringComparison.OrdinalIgnoreCase)))
                .Select(_ => new FileInfo(_).Name);
        }
    }
}
