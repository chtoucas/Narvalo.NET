// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace DocuMaker.Weavers
{
    public interface IWeaver<in T>
    {
        void Weave(T source);
    }
}
