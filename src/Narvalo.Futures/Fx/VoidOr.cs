// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="VoidOr{TError}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="VoidOr{TError}"/>.
    /// </summary>
    public partial class VoidOr
    {
        public static void ThrowIfError<TException>(this VoidOr<TException> @this) where TException : Exception
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                throw @this.Error;
            }
        }
    }

    // Provides extension methods for IEnumerable<VoidOr<TError>>.
    public static partial class VoidOr
    {
        public static IEnumerable<TError> CollectAny<TError>(this IEnumerable<VoidOr<TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TError>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TError> CollectAnyIterator<TError>(IEnumerable<VoidOr<TError>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TError>>();

            foreach (var item in source)
            {
                // REVIEW: Is this the correct behaviour for null?
                if (item == null) { yield return default(TError); }

                if (item.IsError) { yield return item.Error; }
            }
        }
    }
}
