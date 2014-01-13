namespace Narvalo.Playground
{
    using System;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public static class TestExtensions
    {
        public static Outcome<T> ParseValue<T>(
            this NameValueCollection nvc,
            string param,
            Func<string, Outcome<T>> fun,
            Func<string> missingKeyMessage)
        {
            Requires.Object(nvc);

            return nvc.MayGetValue(param)
                .Map(fun)
                .ValueOrElse(Outcome.Failure<T>(missingKeyMessage));
        }

        public static Maybe<Outcome<T>> ParseValue<T>(
            this NameValueCollection nvc,
            string param,
            Func<string, Outcome<T>> fun)
        {
            Requires.Object(nvc);

            return nvc.MayGetValue(param).Map(fun);
        }

        public static Outcome<Maybe<T>> ParseSomeValue<T>(
            this NameValueCollection nvc,
            string param,
            MayFunc<string, T> fun)
        {
            Requires.Object(nvc);

            var value = nvc.MayGetValue(param);

            if (value.IsNone) {
                return Outcome.Success(Maybe<T>.None);
            }
            else {
                var result = value.Bind(_ => fun(_));
                if (result.IsSome) {
                    return Outcome.Success(result);
                }
                else {
                    return Outcome.Failure<Maybe<T>>("XXX");
                }
            }
        }

        public static bool IsSuccessful<T>(this Maybe<Outcome<T>> @this)
        {
            return @this.IsNone || @this.Value.Successful;
        }

        //public static Outcome<TResult> Match<TSource, TResult>(
        //    this Maybe<TSource> @this,
        //    Func<TSource, Outcome<TResult>> fun,
        //    string errorMessage)
        //{
        //    return @this.Map(fun).ValueOrElse(Outcome.Failure<TResult>(errorMessage));
        //}

        //public static Outcome<TResult> Match<TSource, TResult>(
        //    this Maybe<TSource> @this,
        //    Func<TSource, Outcome<TResult>> fun,
        //    Func<string> errorMessageFactory)
        //{
        //    return @this.Map(fun).ValueOrElse(Outcome.Failure<TResult>(errorMessageFactory()));
        //}
    }
}
