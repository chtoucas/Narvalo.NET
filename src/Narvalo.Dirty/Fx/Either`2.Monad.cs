namespace Narvalo.Fx
{
    using System;

    public abstract partial class Either<TLeft, TRight>
    {
        public Either<TResult, TResult> Map<TResult>(
            Func<TLeft, TResult> leftSelector,
            Func<TRight, TResult> rightSelector)
        {
            Requires.NotNull(leftSelector, "leftSelector");
            Requires.NotNull(rightSelector, "rightSelector");

            return IsLeft
               ? Either<TResult, TResult>.Left(leftSelector(LeftValue))
               : Either<TResult, TResult>.Right(rightSelector(RightValue));
        }
    }
}
