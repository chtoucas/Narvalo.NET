// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection.Emit;

    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces",
        Justification = "Only defined to clearly state the actual purpose of this interface.")]
    public interface IPresenterConstructorResolver
        : IComponentResolver<Tuple<Type, Type>, DynamicMethod> { }
}
