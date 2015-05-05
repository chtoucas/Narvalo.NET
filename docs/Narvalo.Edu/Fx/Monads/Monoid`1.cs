// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Monads
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class Monoid<T>
    {
        // [Haskell] mempty
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations",
            Justification = "[Educational] This code is not meant to be used.")]
        public static Monoid<T> Empty { get { throw new NotImplementedException(); } }

        // [Haskell] mappend
        public Monoid<T> Append(Monoid<T> other)
        {
            throw new NotImplementedException();
        }
    }
}
