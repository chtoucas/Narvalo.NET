// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Collections.Generic;
    using System.IO;

    public class CSharpEssay
    {
        TemplateBase _template;
        CSharpSourceFormatter _formatter;

        public CSharpEssay(TemplateBase template, IMarkdownEngine markdown)
            : this(template, new CSharpSourceFormatter(markdown)) { }

        public CSharpEssay(TemplateBase template, CSharpSourceFormatter formatter)
        {
            _template = template;
            _formatter = formatter;
        }

        public string Build(string fileName)
        {
            var cSharpSource = new CSharpSource(fileName);
            cSharpSource.Parse();

            var sections = cSharpSource.Format(_formatter);

            return GenerateHtml_(fileName, sections);
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
