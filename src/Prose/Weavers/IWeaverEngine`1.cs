// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Prose.Weavers
{
    using System.IO;
    using Prose.Templating;

    public interface IWeaverEngine<TModel> where TModel : ITemplateModel
    {
        string Weave(TextReader reader, TModel model);
    }
}
