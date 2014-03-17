// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.IO;

    public sealed class DryRunWeaver : IWeaver
    {
        readonly ITemplate _template;

        public DryRunWeaver(ITemplate template)
        {
            Require.NotNull(template, "template");

            _template = template;
        }

        public void Weave(FileInfo file, string outputPath)
        {
            var parser = new SourceParser(file.FullName);
            var blocks = parser.Parse();

            var data = new TemplateData(blocks)
            {
                //Title = file.RelativeName,
            };

            var output = _template.Render(data);
        }
    }
}
