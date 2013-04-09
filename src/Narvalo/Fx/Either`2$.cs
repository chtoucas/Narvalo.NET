namespace Narvalo.Fx
{
    using System;

    public static class EitherExtensions
    {
        public static Either<TResult, TResult> Map<T, TResult>(
           this Either<T, T> either,
           Func<T, TResult> selector)
        {
            Requires.NotNull(selector, "selector");

            return either.IsLeft
               ? Either<TResult, TResult>.Left(selector(either.LeftValue))
               : Either<TResult, TResult>.Right(selector(either.RightValue));
        }

        public static TResult Match<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either,
            Func<TLeft, TResult> leftFun,
            Func<TRight, TResult> rightFun)
        {
            Requires.NotNull(leftFun, "leftFun");
            Requires.NotNull(rightFun, "rightFun");

            return either.IsLeft ? leftFun(either.LeftValue) : rightFun(either.RightValue);
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
