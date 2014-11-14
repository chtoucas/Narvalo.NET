// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narrative.Templates
{
    using System.Collections.Generic;
    using Narrative.Parsers;

    public interface ITemplateModel
    {
        IEnumerable<Block> Blocks { get; set; }
    }
}
