// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Templating
{
    public interface ITemplate
    {
        string Render(TemplateData data);
    }
}
