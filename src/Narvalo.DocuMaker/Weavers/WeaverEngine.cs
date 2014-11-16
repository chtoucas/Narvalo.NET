// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.DocuMaker.Weavers
{
    using System.Collections.Generic;
    using System.IO;
    using Narvalo;
    using Narvalo.DocuMaker.Parsing;
    using Narvalo.DocuMaker.Templating;

    public class WeaverEngine<TModel> : IWeaverEngine<TModel> where TModel : ITemplateModel
    {
        readonly IParser _parser;
        readonly ITemplate<TModel> _template;

        public WeaverEngine(IParser parser, ITemplate<TModel> template)
        {
            Require.NotNull(parser, "parser");
            Require.NotNull(template, "template");

            _parser = parser;
            _template = template;
        }

        public string Weave(TextReader reader, TModel model)
        {
            IEnumerable<Block> blocks = _parser.Parse(reader);

            model.Blocks = blocks;

            return _template.Render(model);
        }
    }
}
