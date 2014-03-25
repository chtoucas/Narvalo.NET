// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Processors
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Narvalo.IO;

    public abstract class DirectoryProcessorBase
    {
        static readonly List<string> DirectoriesToIgnore_ = new List<string> { "bin", "obj", "_Aliens" };

        protected static readonly Func<DirectoryInfo, bool> DirectoryFilter
             = _ => !DirectoriesToIgnore_.Any(s => _.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
        protected static readonly Func<FileInfo, bool> FileFilter
             = _ => !_.Name.EndsWith("Designer.cs", StringComparison.OrdinalIgnoreCase);

        readonly IWeaver _weaver;
        readonly IOutputWriter _writer;

        protected DirectoryProcessorBase(IWeaver weaver, IOutputWriter writer)
        {
            _weaver = weaver;
            _writer = writer;
        }

        public abstract void Process(DirectoryInfo directory);

        protected void ProcessFile(RelativeFile file)
        {
            string content;

            using (var reader = new StreamReader(file.File.FullName)) {
                content = _weaver.Weave(reader);
            }

            _writer.Write(file, content);
        }

        protected void OnDirectoryStart(object sender, RelativeDirectoryEventArgs e)
        {
            _writer.CreateDirectory(e.RelativeDirectory);
        }
    }
}
