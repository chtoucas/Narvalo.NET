namespace Narvalo.Fx
{
    using System;

    public static class MaybeExtensions
    {
        public static Outcome<TResult> Match<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Outcome<TResult>> fun,
            string errorMessage)
        {
            return @this.Map(fun).ValueOrElse(Outcome.Failure<TResult>(errorMessage));
        }

        public static Outcome<TResult> Match<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Outcome<TResult>> fun,
            Func<string> errorMessageFactory)
        {
            return @this.Map(fun).ValueOrElse(Outcome.Failure<TResult>(errorMessageFactory()));
        }
    }
}
