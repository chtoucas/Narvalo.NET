// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    public interface ICompositeViewTypeResolver
    {
        Type Resolve(Type viewType);
    }
}
