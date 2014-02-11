namespace Narvalo.Fx
{
    using System;

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
