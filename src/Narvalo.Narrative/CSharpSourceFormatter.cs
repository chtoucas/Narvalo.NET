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

        public IEnumerable<Section> Format(string source)
        {
            var cSharpCode = new CSharpSource(source);
            cSharpCode.Parse();

            foreach (var section in cSharpCode.Sections) {
                yield return new Section
                {
                    HtmlCode = HttpUtility.HtmlEncode(section.HtmlCode),
                    HtmlDoc = _markdown.Transform(section.HtmlDoc)
                };
            }
        }
    }
}
