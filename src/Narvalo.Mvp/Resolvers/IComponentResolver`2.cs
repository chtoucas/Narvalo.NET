// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    public interface IComponentResolver<in TInput, out TComponent>
    {
        TComponent Resolve(TInput input);
    }
}
