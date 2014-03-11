// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative
{
    using System.Collections.Generic;

    public sealed class TemplateModel
    {
        public IEnumerable<BlockBase> Blocks { get; set; }

        public string FileName { get; set; }
    }
}
