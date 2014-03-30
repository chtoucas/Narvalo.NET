// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Templates
{
    public interface ITemplate<TModel> where TModel : ITemplateModel
    {
        string Render(TModel model);
    }
}
