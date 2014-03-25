// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Narrative.Weaving
{
    public interface IWeaver<in T>
    {
        void Weave(T source);
    }
}
