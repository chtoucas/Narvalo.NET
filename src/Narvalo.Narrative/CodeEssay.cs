// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Collections.Generic;
    using System.IO;

    public class CodeEssay
    {
        IMarkdownEngine _markdown;
        TemplateBase _template;

        public CodeEssay(TemplateBase template, IMarkdownEngine markdown)
        {
            _template = template;
            _markdown = markdown;
        }

        public string Process(string source)
        {
            var formatter = new CSharpSourceFormatter(_markdown);
            var sections = formatter.Format(source);

            return GenerateHtml_(source, sections);
        }

        string GenerateHtml_(string source, IEnumerable<Section> sections)
        {
            _template.Title = Path.GetFileName(source);
            _template.Sections = sections;

            _template.Execute();

            return _template.Buffer.ToString();
        }
    }
}
