// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    public abstract class BlockBase
    {
        protected BlockBase() { }

        public abstract BlockType BlockType { get; }

        public string Content { get; set; }
    }
}
