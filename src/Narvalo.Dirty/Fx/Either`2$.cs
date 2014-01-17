namespace Narvalo.Fx
{
    using System;

    public static class EitherExtensions
    {
        public static TResult Match<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TLeft, TResult> leftFun,
            Func<TRight, TResult> rightFun)
        {
            Require.NotNull(leftFun, "leftFun");
            Require.NotNull(rightFun, "rightFun");

            return @this.IsLeft ? leftFun(@this.LeftValue) : rightFun(@this.RightValue);
        }
    }
}
