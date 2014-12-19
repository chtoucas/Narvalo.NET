// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Templating
{
    using System;
    using System.Linq;
    using System.Web;
    using Prose.Parsing;
    using Narvalo;

    public sealed class Template : ITemplate<TemplateModel>
    {
        readonly IMarkdownEngine _markdown;
        readonly Lazy<Type> _razorTemplateType;

        public Template(string input, IMarkdownEngine markdown)
        {
            Require.NotNull(input, "input");
            Require.NotNull(markdown, "markdown");

            _markdown = markdown;

            _razorTemplateType = new Lazy<Type>(() => new RazorTemplateCompiler(input).Compile());
        }

        public string Render(TemplateModel model)
        {
            Require.NotNull(model, "model");

            var razorTemplate = Activator.CreateInstance(_razorTemplateType.Value) as RazorTemplateBase;
            if (razorTemplate == null) {
                throw new TemplateException("The template does not inherit from RazorTemplateBase.");
            }

            razorTemplate.Title = model.Title;
            razorTemplate.Blocks = model.Blocks.Select(_ => Transform_(_));

            razorTemplate.Execute();

            return razorTemplate.Buffer.ToString();
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
