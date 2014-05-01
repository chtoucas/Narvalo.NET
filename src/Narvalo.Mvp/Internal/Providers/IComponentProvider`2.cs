// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    internal interface IComponentProvider<in TInput, out TComponent>
    {
        TComponent GetComponent(TInput input);
    }
}
