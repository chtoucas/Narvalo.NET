﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Parsing
{
    using System.Collections.Generic;
    using System.IO;

    public interface IParser
    {
        IEnumerable<Block> Parse(TextReader reader);
    }
}
