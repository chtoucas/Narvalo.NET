namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Narvalo;
    using Narvalo.Linq;

    public static class NameValueCollectionExtensions
    {
        public static Outcome<T> ParseValue<T>(
            this NameValueCollection @this,
            string param,
            Func<string, Outcome<T>> fun,
            Func<string> missingKeyMessage)
        {
            Require.Object(@this);

            return @this.MayGetValue(param)
                .Map(fun)
                .ValueOrElse(Outcome.Failure<T>(new KeyNotFoundException(missingKeyMessage())));
        }

        public static Maybe<Outcome<T>> ParseValue<T>(
            this NameValueCollection @this,
            string param,
            Func<string, Outcome<T>> fun)
        {
            Require.Object(@this);

            return @this.MayGetValue(param).Map(fun);
        }

        //public static Outcome<Maybe<T>> ParseSomeValue<T>(
        //    this NameValueCollection @this,
        //    string param,
        //    MayFunc<string, T> fun)
        //{
        //    Require.Object(@this);

        //    var value = @this.MayGetValue(param);

        //    if (value.IsNone) {
        //        return Outcome.Success(Maybe<T>.None);
        //    }
        //    else {
        //        var result = value.Bind(_ => fun(_));
        //        if (result.IsSome) {
        //            return Outcome.Success(result);
        //        }
        //        else {
        //            return Outcome.Failure<Maybe<T>>("XXX");
        //        }
        //    }
        //}
    }
}
