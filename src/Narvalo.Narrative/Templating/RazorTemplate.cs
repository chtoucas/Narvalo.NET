// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Templating
{
    using System;
    using System.Linq;
    using System.Web;
    using Narvalo.Narrative.Parsing;

    public sealed class RazorTemplate : ITemplate
    {
        readonly IMarkdownEngine _markdown;
        readonly Lazy<Type> _templateType;

        public RazorTemplate(string input, IMarkdownEngine markdown)
        {
            Require.NotNull(input, "input");
            Require.NotNull(markdown, "markdown");

            _markdown = markdown;

            _templateType = new Lazy<Type>(() => new RazorTemplateCompiler(input).Compile());
        }

        public string Render(TemplateData data)
        {
            Require.NotNull(data, "data");

            var template = Activator.CreateInstance(_templateType.Value) as RazorTemplateBase;
            if (template == null) {
                throw new TemplateException("The template does not inherit from RazorTemplateBase.");
            }

            template.Title = data.Title;
            template.Blocks = data.Blocks.Select(_ => Transform_(_));

            template.Execute();

            return template.Buffer.ToString();
        }

        RazorTemplateBlock Transform_(Block block)
        {
            switch (block.BlockType) {
                case BlockType.Code:
                    return new RazorTemplateBlock(block.BlockType)
                    {
                        Content = new HtmlString(HttpUtility.HtmlEncode(block.Content))
                    };
                case BlockType.Markdown:
                    return new RazorTemplateBlock(block.BlockType)
                    {
                        Content = new HtmlString(_markdown.Transform(block.Content))
                    };
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
