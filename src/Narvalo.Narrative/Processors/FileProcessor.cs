// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Processors
{
    using System;
    using System.IO;
    using Narvalo.IO;

    public sealed class FileProcessor
    {
        readonly IWeaver _weaver;
        readonly IOutputWriter _writer;

        public FileProcessor(IWeaver weaver, IOutputWriter writer)
        {
            _weaver = weaver;
            _writer = writer;
        }

        public void Process(FileInfo file)
        {
            string content;

            using (var reader = new StreamReader(file.FullName)) {
                content = _weaver.Weave(reader);
            }

            _writer.Write(new RelativeFile(file, String.Empty), content);
        }
    }
}
