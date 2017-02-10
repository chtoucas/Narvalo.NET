// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;
    using System.Linq;

    // Overrides a bunch of auto-generated (extension) methods.
    public abstract partial class Outcome<T>
    {
        public Outcome<TResult> Then<TResult>(Outcome<TResult> other)
            => IsSuccess ? other : Outcome<TResult>.η(ExceptionInfo);
    }

    // Overrides for auto-generated (extension) methods on IEnumerable<Outcome<T>>.
    public static partial class EnumerableExtensions
    {
        internal static Outcome<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Outcome<TSource>> @this)
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
                    return Outcome.Failure<IEnumerable<TSource>>(m.ExceptionInfo);
                }

                list.Add(m.Value);
            }

            return Outcome.Success(list.AsEnumerable());
        }
    }
}
