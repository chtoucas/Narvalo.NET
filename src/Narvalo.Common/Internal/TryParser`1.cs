// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Fx;

    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#",
        Justification = "The method already returns a boolean to indicate the outcome.")]
    delegate bool TryParser<TResult>(string value, out TResult result);

    static class TryParserExtensions
    {
        internal static T? NullInvoke<T>(this TryParser<T> @this, string value) where T : struct
        {
            Require.Object(@this);

            if (value == null) { return null; }

            T result;
            return @this.Invoke(value, out result) ? result : (T?)null;
        }

        internal static Maybe<T> MayInvoke<T>(this TryParser<T> @this, string value) where T : class
        {
            Require.Object(@this);

            if (value == null) { return Maybe<T>.None; }

            T result;
            return @this.Invoke(value, out result) ? Maybe.Create(result) : Maybe<T>.None;
        }
    }
}
