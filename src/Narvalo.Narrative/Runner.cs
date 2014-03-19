// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.IO;
    using Narvalo.IO;

    public sealed class Runner : IRunner
    {
        readonly IWeaver _weaver;
        readonly IOutputWriter _writer;
        readonly FileInfo _file;

        public Runner(IWeaver weaver, IOutputWriter writer, FileInfo file)
        {
            _weaver = weaver;
            _writer = writer;
            _file = file;
        }

        public void Run()
        {
            var content = _weaver.Weave(_file);

            _writer.Write(new RelativeFile(_file, String.Empty), content);
        }
    }
}
