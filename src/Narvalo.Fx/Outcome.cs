// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Outcome{T}"/>.
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Outcome{S}"/>.
    /// </summary>
    public static partial class Outcome
    {
        public static Outcome<T> FromError<T>(ExceptionDispatchInfo exceptionInfo)
        {
            Expect.NotNull(exceptionInfo);
            Warrant.NotNull<Outcome<T>>();

            return Outcome<T>.η(exceptionInfo);
        }
    }

    // Provides extension methods for IEnumerable<Outcome<T>>.
    public static partial class Outcome
    {
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Outcome<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Outcome<TSource>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                // REVIEW: Is this the correct behaviour for null?
                if (item == null) { yield return default(TSource); }

                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
