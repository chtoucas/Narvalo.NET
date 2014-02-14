// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    sealed class Monoid<T>
    {
        // [Haskell] mempty
        public static Monoid<T> Empty { get { throw new NotImplementedException(); } }

        // [Haskell] mappend
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "other",
            Justification = "Monad template definition.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Monad template definition.")]
        public Monoid<T> Append(Monoid<T> other)
        {
            throw new NotImplementedException();
        }
    }
}
