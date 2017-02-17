// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class SwitchExtensions
    {
        // Filter the left value.
        public static Switch<TLeft, TRight> Where<TLeft, TRight>(
               this Switch<TLeft, TRight> @this,
               Func<TLeft, bool> leftPredicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(leftPredicate, nameof(leftPredicate));

            return @this.Bind(_ => leftPredicate.Invoke(_) ? @this : Switch<TLeft, TRight>.Empty);
        }

        // Filter the right value.
        public static Switch<TLeft, TRight> Where<TLeft, TRight>(
               this Switch<TLeft, TRight> @this,
               Func<TRight, bool> rightPredicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(rightPredicate, nameof(rightPredicate));

            return @this.Bind(_ => rightPredicate.Invoke(_) ? @this : Switch<TLeft, TRight>.Empty);
        }
    }
}
