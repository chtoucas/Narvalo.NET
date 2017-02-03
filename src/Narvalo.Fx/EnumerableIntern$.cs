// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class EnumerableInternExtensions
    {
        #region Overrides for auto-generated (extension) methods on IEnumerable<Maybe<T>>

        // Custom version of CollectCore.
        public static Maybe<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            var list = new List<TSource>();

            foreach (var m in @this)
            {
                if (!m.IsSome)
                {
                    return Maybe<IEnumerable<TSource>>.None;
                }

                list.Add(m.Value);
            }

            return Maybe.Of(list.AsEnumerable());
        }

        #endregion

        #region Overrides for auto-generated (extension) methods on IEnumerable<Outcome<T>>

        // Custom version of CollectCore.
        public static Outcome<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Outcome<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Outcome<IEnumerable<TSource>>>();

            var list = new List<TSource>();

            foreach (var m in @this)
            {
                // REVIEW: Is this the correct behaviour when m is null?
                if (m == null)
                {
                    continue;
                }

                if (!m.IsSuccess)
                {
                    return Outcome.Failure<IEnumerable<TSource>>(m.ToExceptionDispatchInfo());
                }

                list.Add(m.ToValue());
            }

            return Outcome.Success(list.AsEnumerable());
        }

        #endregion
    }
}
