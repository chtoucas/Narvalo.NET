// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using Narvalo.Fx;

    internal static class TryParserExtensions
    {
        public static T? NullInvoke<T>(this TryParser<T> @this, string value) where T : struct
        {
            Require.NotNull(@this, nameof(@this));

            if (value == null) { return null; }

            T result;
            return @this.Invoke(value, out result) ? result : (T?)null;
        }

        public static Maybe<T> MayInvoke<T>(this TryParser<T> @this, string value) where T : class
        {
            Require.NotNull(@this, nameof(@this));

            if (value == null) { return Maybe<T>.None; }

            T result;
            return @this.Invoke(value, out result) ? Maybe.Of(result) : Maybe<T>.None;
        }
    }
}
