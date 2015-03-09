// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Templating
{
    public interface IMarkdownEngine
    {
        string Transform(string text);
    }
}
