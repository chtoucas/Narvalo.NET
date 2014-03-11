// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    public sealed class MarkdownBlock : BlockBase
    {
        public MarkdownBlock() : base() { }

        public override BlockType BlockType { get { return BlockType.Markdown; } }
    }
}
