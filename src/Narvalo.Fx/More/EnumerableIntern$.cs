// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.More
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx.Internal;

    internal static class EnumerableInternExtensions
    {
        #region Overrides for auto-generated (extension) methods on IEnumerable<T>

        public static IEnumerable<TSource> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicateM, nameof(predicateM));
            Warrant.NotNull<IEnumerable<TSource>>();

            return @this
                .Where(_ => predicateM.Invoke(_).ValueOrElse(false))
                .EmptyIfNull();
        }

        #endregion
    }
}
