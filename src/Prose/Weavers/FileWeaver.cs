// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Weavers
{
    using System.IO;
    using Prose.IO;
    using Prose.Templating;
    using Narvalo;

    public sealed class FileWeaver : IWeaver<FileInfo>
    {
        readonly IWeaverEngine<TemplateModel> _weaver;
        readonly IOutputWriter _writer;

        public FileWeaver(IWeaverEngine<TemplateModel> weaver, IOutputWriter writer)
        {
            Require.NotNull(weaver, "weaver");
            Require.NotNull(writer, "writer");

            _weaver = weaver;
            _writer = writer;
        }

        public void Weave(FileInfo source)
        {
            Require.NotNull(source, "source");

            var model = new TemplateModel
            {
                Title = source.Name,
            };

            string content;

            using (var reader = new StreamReader(source.FullName)) {
                content = _weaver.Weave(reader, model);
            }

            _writer.Write(source, content);
        }
    }
}
