// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Weaving
{
    using System.IO;
    using Narvalo.Narrative.IO;

    public sealed class FileWeaver : IWeaver<FileInfo>
    {
        readonly IWeaverEngine _weaver;
        readonly IOutputWriter _writer;

        public FileWeaver(IWeaverEngine weaver, IOutputWriter writer)
        {
            Require.NotNull(weaver, "weaver");
            Require.NotNull(writer, "writer");

            _weaver = weaver;
            _writer = writer;
        }

        public void Weave(FileInfo file)
        {
            Require.NotNull(file, "file");

            string content;

            using (var reader = new StreamReader(file.FullName)) {
                content = _weaver.Weave(reader);
            }

            _writer.Write(file, content);
        }
    }
}
