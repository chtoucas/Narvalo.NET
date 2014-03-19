// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Narvalo.IO;

    public abstract class DirectoryRunnerBase : IRunner
    {
        static readonly List<string> DirectoriesToIgnore_ = new List<string> { "bin", "obj", "_Aliens" };

        protected static readonly Func<DirectoryInfo, bool> DirectoryFilter
             = _ => !DirectoriesToIgnore_.Any(s => _.Name.Equals(s, StringComparison.OrdinalIgnoreCase));
        protected static readonly Func<FileInfo, bool> FileFilter
             = _ => !_.Name.EndsWith("Designer.cs", StringComparison.OrdinalIgnoreCase);

        readonly IWeaver _weaver;
        readonly IOutputWriter _writer;
        readonly DirectoryInfo _directory;

        protected DirectoryRunnerBase(IWeaver weaver, IOutputWriter writer, DirectoryInfo directory)
        {
            _weaver = weaver;
            _writer = writer;
            _directory = directory;
        }

        protected DirectoryInfo Directory { get { return _directory; } }

        public abstract void Run();

        protected void RunCore(RelativeFile file)
        {
            var content = _weaver.Weave(file.File);

            _writer.Write(file, content);
        }

        protected void OnDirectoryStart(object sender, RelativeDirectoryEventArgs e)
        {
            _writer.CreateDirectory(e.RelativeDirectory);
        }
    }
}
