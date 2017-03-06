// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public static partial class Func
    {
        public static Func<T> Return<T>(T value)
        {
            return () => value;
        }

        public static Func<T> Flatten<T>(Func<Func<T>> square)
        {
            Require.NotNull(square, nameof(square));

            return square.Invoke();
        }
    }
}
