// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Weaving
{
    using System.Collections.Generic;
    using System.IO;
    using Narvalo.Narrative.Parsing;
    using Narvalo.Narrative.Templating;

    public class WeaverEngine : IWeaverEngine
    {
        readonly IParser _parser;
        readonly ITemplate _template;

        public WeaverEngine(IParser parser, ITemplate template)
        {
            Require.NotNull(parser, "parser");
            Require.NotNull(template, "template");

            _parser = parser;
            _template = template;
        }

        public string Weave(TextReader reader)
        {
            IEnumerable<Block> blocks = _parser.Parse(reader);

            var data = new TemplateData(blocks)
            {
                //Title = file.RelativeName,
            };

            return _template.Render(data);
        }
    }
}
