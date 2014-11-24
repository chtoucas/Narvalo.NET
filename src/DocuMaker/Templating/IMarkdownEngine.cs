// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace DocuMaker.Templating
{
    public interface IMarkdownEngine
    {
        string Transform(string text);
    }
}
