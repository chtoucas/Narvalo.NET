namespace Narvalo.Fx
{
    using System;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Collections;

    public static class NameValueCollectionExtensions
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
    }
}
