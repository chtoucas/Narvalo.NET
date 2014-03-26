// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Templating
{
    using System.Collections.Generic;

    public sealed class TemplateData
    {
        readonly IEnumerable<Block> _blocks;

        public TemplateData(IEnumerable<Block> blocks)
        {
            Require.NotNull(blocks, "blocks");

            _blocks = blocks;
        }

        public IEnumerable<Block> Blocks { get { return _blocks; } }

        public string Title { get; set; }
    }
}
