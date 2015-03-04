// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using Narvalo;

    public abstract partial class Either<TLeft, TRight>
    {
        public Either<TResult, TResult> Map<TResult>(
            Func<TLeft, TResult> leftSelector,
            Func<TRight, TResult> rightSelector)
        {
            Require.NotNull(leftSelector, "leftSelector");
            Require.NotNull(rightSelector, "rightSelector");

            return IsLeft
               ? Either<TResult, TResult>.Left(leftSelector(LeftValue))
               : Either<TResult, TResult>.Right(rightSelector(RightValue));
        }
    }
}
