﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#",
        Justification = "[Intentionally] The method implements the Try... pattern. Only used internally.")]
    [SuppressMessage("Gendarme.Rules.Naming", "UseCorrectCasingRule",
        Justification = "[Ignore] '__result' is a compiler generated parameter.")]
    internal delegate bool TryParser<TResult>(string value, out TResult result);

    internal static class TryParserExtensions
    {
        public static T? NullInvoke<T>(this TryParser<T> @this, string value) where T : struct
        {
            Require.Object(@this);

            if (value == null) { return null; }

            T result;
            return @this.Invoke(value, out result) ? result : (T?)null;
        }

        public static Maybe<T> MayInvoke<T>(this TryParser<T> @this, string value) where T : class
        {
            Require.Object(@this);

            if (value == null) { return Maybe<T>.None; }

            T result;
            return @this.Invoke(value, out result) ? Maybe.Of(result) : Maybe<T>.None;
        }
    }
}