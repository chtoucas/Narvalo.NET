// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.DocuMaker.Templating
{
    using System.Collections.Generic;
    using Narvalo.DocuMaker.Parsing;

    public interface ITemplateModel
    {
        IEnumerable<Block> Blocks { get; set; }
    }
}
