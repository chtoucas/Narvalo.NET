// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System.Diagnostics;

    using Narvalo.Applicative;

    internal static class TryParserExtensions
    {
        public static T? NullInvoke<T>(this TryParser<T> @this, string value) where T : struct
        {
            Debug.Assert(@this != null);

            if (value == null) { return null; }

            return @this.Invoke(value, out T result) ? result : (T?)null;
        }

        public static Maybe<T> MayInvoke<T>(this TryParser<T> @this, string value) where T : class
        {
            Debug.Assert(@this != null);

            if (value == null) { return Maybe<T>.None; }

            return @this.Invoke(value, out T result) ? Maybe.Of(result) : Maybe<T>.None;
        }
    }
}
