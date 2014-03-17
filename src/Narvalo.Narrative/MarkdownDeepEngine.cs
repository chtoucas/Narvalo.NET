// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System;
    
    using Markdown = MarkdownDeep.Markdown;

    sealed class MarkdownDeepEngine : IMarkdownEngine
    {
        public string Transform(string text)
        {
            Require.NotNull(text, "text");

            Markdown inner = new Markdown
            {
                ExtraMode = true,
                SafeMode = false
            };

            return text.Length == 0 ? String.Empty : inner.Transform(text);
        }
    }
}
