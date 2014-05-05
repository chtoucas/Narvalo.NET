// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;

    public interface IPresenterBindingAttributesResolver
        : IComponentResolver<Type, IEnumerable<PresenterBindingAttribute>> { }
}
