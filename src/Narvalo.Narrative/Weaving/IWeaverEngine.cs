// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Weaving
{
    using System.IO;

    public interface IWeaverEngine
    {
        string Weave(TextReader reader);
    }
}
