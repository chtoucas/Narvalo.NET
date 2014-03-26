// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Parsing
{
    using System.Text;

    public abstract class Block
    {
        readonly string _content;

        Block(string content)
        {
            Require.NotNull(content, "content");

            _content = content;
        }

        public abstract BlockType BlockType { get; }

        public string Content { get { return _content; } }

        public static Block Code(StringBuilder builder)
        {
            return Code(builder.ToString());
        }

        public static Block Code(string content)
        {
            return new CodeImpl(content);
        }

        public static Block Markdown(StringBuilder builder)
        {
            return Markdown(builder.ToString());
        }

        public static Block Markdown(string content)
        {
            return new MarkdownImpl(content);
        }

        sealed class CodeImpl : Block
        {
            public CodeImpl(string content) : base(content) { }

            public override BlockType BlockType { get { return BlockType.Code; } }
        }

        sealed class MarkdownImpl : Block
        {
            public MarkdownImpl(string content) : base(content) { }

            public override BlockType BlockType { get { return BlockType.Markdown; } }
        }
    }
}
