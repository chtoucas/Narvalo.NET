// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using Markdown = MarkdownSharp.Markdown;

    public class MarkdownSharpEngine : IMarkdownEngine
    {
        readonly Markdown _inner;

        public MarkdownSharpEngine()
        {
            _inner = new Markdown();
        }

        public string Transform(string text)
        {
            return _inner.Transform(text);
        }
    }
}
