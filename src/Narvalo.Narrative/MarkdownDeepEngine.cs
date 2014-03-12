// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    using Markdown = MarkdownDeep.Markdown;

    public sealed class MarkdownDeepEngine : IMarkdownEngine
    {
        readonly Markdown _inner;

        public MarkdownDeepEngine()
        {
            _inner = new Markdown
            {
                ExtraMode = true,
                SafeMode = false
            };
        }

        public string Transform(string text)
        {
            Require.NotNull(text, "text");

            return text.Length == 0 ? String.Empty : _inner.Transform(text);
        }
    }
}
