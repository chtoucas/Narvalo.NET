// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Weaving
{
    using System.IO;
    using Narvalo.IO;
    using Narvalo.Narrative.IO;

    public sealed class RelativeFileWeaver : IWeaver<RelativeFile>
    {
        readonly IWeaverEngine _weaver;
        readonly IOutputWriter _writer;

        public RelativeFileWeaver(IWeaverEngine weaver, IOutputWriter writer)
        {
            Require.NotNull(weaver, "weaver");
            Require.NotNull(writer, "writer");

            _weaver = weaver;
            _writer = writer;
        }

        public void Weave(RelativeFile file)
        {
            Require.NotNull(file, "file");

            string content;

            using (var reader = new StreamReader(file.File.FullName)) {
                content = _weaver.Weave(reader);
            }

            _writer.Write(file, content);
        }
    }
}
