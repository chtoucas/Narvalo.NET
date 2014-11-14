// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narrative.Templates
{
    using System.Collections.Generic;
    using Narrative.Parsers;

    public sealed class TemplateModel : ITemplateModel
    {
        public IEnumerable<Block> Blocks { get; set; }

        public string Title { get; set; }
    }
}
