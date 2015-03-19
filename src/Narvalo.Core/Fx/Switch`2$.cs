// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static class SwitchExtensions
    {
        // Filter the left value.
        public static Switch<TLeft, TRight> Filter<TLeft, TRight>(
               this Switch<TLeft, TRight> @this,
               Func<TLeft, bool> leftPredicate)
        {
            Require.Object(@this);
            Require.NotNull(leftPredicate, "leftPredicate");

            return @this.Bind(_ => leftPredicate.Invoke(_) ? @this : Switch<TLeft, TRight>.Empty);
        }

        // Filter the right value.
        public static Switch<TLeft, TRight> Filter<TLeft, TRight>(
               this Switch<TLeft, TRight> @this,
               Func<TRight, bool> rightPredicate)
        {
            Require.Object(@this);
            Require.NotNull(rightPredicate, "rightPredicate");

            return @this.Bind(_ => rightPredicate.Invoke(_) ? @this : Switch<TLeft, TRight>.Empty);
        }
    }
}
