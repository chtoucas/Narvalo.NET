namespace Narvalo.Fx
{
    using System;

    public static class EitherExtensions
    {
        public static Either<TResult, TResult> Map<T, TResult>(
           this Either<T, T> @this,
           Func<T, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return @this.IsLeft
               ? Either<TResult, TResult>.Left(selector(@this.LeftValue))
               : Either<TResult, TResult>.Right(selector(@this.RightValue));
        }

        public static TResult Match<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TLeft, TResult> leftFun,
            Func<TRight, TResult> rightFun)
        {
            Requires.NotNull(leftFun, "leftFun");
            Requires.NotNull(rightFun, "rightFun");

            return @this.IsLeft ? leftFun(@this.LeftValue) : rightFun(@this.RightValue);
        }

        //[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        //public static Either<TLeft, TResult> SelectMany<TLeft, TSource, TInner, TResult>(
        //    this Either<TLeft, TSource> id,
        //    Func<TSource, Either<TLeft, TInner>> valueSelector,
        //    Func<TSource, TInner, TResult> resultSelector)
        //{
        //    Requires.NotNull(valueSelector, "valueSelector");
        //    Requires.NotNull(resultSelector, "resultSelector");

        //    return id.Bind(t => valueSelector(t).Map(m => resultSelector(t, m)));
        //}
    }
}
