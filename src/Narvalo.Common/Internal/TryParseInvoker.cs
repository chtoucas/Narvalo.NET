// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using Narvalo.Fx;

    static class TryParseInvoker
    {
        public static T? Invoke<T>(string value, TryParse<T> tryParser) where T : struct
        {
            if (value == null) { return null; }

            T result;
            return tryParser.Invoke(value, out result) ? result : (T?)null;
        }

        public static Maybe<T> MayInvoke<T>(string value, TryParse<T> tryParser) where T : class
        {
            if (value == null) { return Maybe<T>.None; }

            T result;
            return tryParser.Invoke(value, out result) ? Maybe.Create(result) : Maybe<T>.None;
        }
    }
}
