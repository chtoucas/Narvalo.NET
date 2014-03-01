// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Monads.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    sealed class Monoid<T>
    {
        // [Haskell] mempty
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static Monoid<T> Empty { get { throw new NotImplementedException(); } }

        // [Haskell] mappend
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "other")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public Monoid<T> Append(Monoid<T> other)
        {
            throw new NotImplementedException();
        }
    }
}
