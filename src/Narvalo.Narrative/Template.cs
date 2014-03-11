// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using System.Linq;
    using System.Web;
    using Narvalo.Narrative.Internal;

    public sealed class Template
    {
        readonly IMarkdownEngine _markdown;
        readonly Lazy<Type> _templateType;

        public Template(string path, IMarkdownEngine markdown)
        {
            Require.NotNullOrEmpty(path, "path");
            Require.NotNull(markdown, "markdown");

            _markdown = markdown;

            _templateType = new Lazy<Type>(() => TemplateCompiler.Compile(path));
        }

        public string Render(TemplateModel model)
        {
            Require.NotNull(model, "model");

            var template = Activator.CreateInstance(_templateType.Value) as TemplateBase;
            if (template == null) {
                throw new NarrativeException("The template does not inherit from TemplateBase.");
            }

            template.Title = model.FileName;
            template.Blocks = model.Blocks.Select(_ => Transform_(_));

            template.Execute();

            return template.Buffer.ToString();
        }

        HtmlBlock Transform_(BlockBase block)
        {
            switch (block.BlockType) {
                case BlockType.Code:
                    return new HtmlBlock
                    {
                        BlockType = block.BlockType,
                        Content = new HtmlString(HttpUtility.HtmlEncode(block.Content))
                    };
                case BlockType.Markdown:
                    return new HtmlBlock
                    {
                        BlockType = block.BlockType,
                        Content = new HtmlString(_markdown.Transform(block.Content))
                    };
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
