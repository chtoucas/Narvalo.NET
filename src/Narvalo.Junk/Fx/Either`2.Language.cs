// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{

    public abstract partial class Either<TLeft, TRight>
    {
        //public TResult Match<TResult>(
        //    Func<TLeft, TResult> leftSelector,
        //    Func<TRight, TResult> rightSelector)
        //{
        //    Require.NotNull(leftSelector, "leftSelector");
        //    Require.NotNull(rightSelector, "rightSelector");

        //    return IsLeft ? leftSelector(LeftValue) : rightSelector(RightValue);
        //}
    }
}
