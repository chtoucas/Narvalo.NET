// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Collections.Generic;
    using System.IO;

    public class Weaver : IWeaver
    {
        readonly ITemplate _template;

        public Weaver(ITemplate template)
        {
            Require.NotNull(template, "template");

            _template = template;
        }

        public string Weave(TextReader reader)
        {
            Require.NotNull(reader, "reader");

            var parser = new CSharpParser();
            IEnumerable<Block> blocks = parser.Parse(reader);

            var data = new TemplateData(blocks)
            {
                //Title = file.RelativeName,
            };

            return _template.Render(data);
        }
    }
}
