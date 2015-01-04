// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Templating
{
    using System.Collections.Generic;
    using Prose.Parsing;

    public sealed class TemplateModel : ITemplateModel
    {
        public IEnumerable<Block> Blocks { get; set; }

        public string Title { get; set; }
    }
}
