// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    // In case, we decide to create a generic version of Moneypenny.
    internal interface IMoneypenny<T, out TCurrency>
        : IAmount<T, long>, IEquatable<T>, IComparable<T>, IComparable, IFormattable
    {
        TCurrency Currency { get; }

        decimal ToMajor();

        T Increment();
        T Decrement();
    }
}
