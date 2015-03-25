// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Fx;

    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#",
        Justification = "[Intentionally] The method implements the Try... pattern. Only used internally.")]
    internal delegate bool TryConverter<in TSource, TResult>(TSource value, out TResult result);

    internal static class TryConverterExtensions
    {
        public static TResult? NullInvoke<TSource, TResult>(this TryConverter<TSource, TResult> @this, TSource value)
            where TResult : struct
        {
            Require.Object(@this);

            if (value == null) { return null; }

            TResult result;
            return @this.Invoke(value, out result) ? result : (TResult?)null;
        }

        public static Maybe<TResult> MayInvoke<TSource, TResult>(this TryConverter<TSource, TResult> @this, TSource value)
            where TResult : class
        {
            Require.Object(@this);

            if (value == null) { return Maybe<TResult>.None; }

            TResult result;
            return @this.Invoke(value, out result) ? Maybe.Of(result) : Maybe<TResult>.None;
        }
    }
}
