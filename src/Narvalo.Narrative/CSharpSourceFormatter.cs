// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Collections.Generic;
    using System.Web;

    public class CSharpSourceFormatter
    {
        readonly IMarkdownEngine _markdown;

        public CSharpSourceFormatter(IMarkdownEngine markdown)
        {
            _markdown = markdown;
        }

        public Section Format(string doc, string code)
        {
            return new Section
            {
                HtmlCode = HttpUtility.HtmlEncode(code),
                HtmlDoc = _markdown.Transform(doc)
            };
        }
    }
}
