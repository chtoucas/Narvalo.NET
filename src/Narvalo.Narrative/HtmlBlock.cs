// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Web;

    public class HtmlBlock
    {
        public HtmlBlock() { }

        public BlockType BlockType { get; set; }

        public IHtmlString Content { get; set; }
    }
}
