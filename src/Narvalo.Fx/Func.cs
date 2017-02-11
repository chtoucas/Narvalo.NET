// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class Func
    {
        public static Func<T> Pure<T>(T value)
        {
            Warrant.NotNull<Func<T>>();

            return () => value;
        }

        public static Func<T> Flatten<T>(Func<Func<T>> square)
        {
            Require.NotNull(square, nameof(square));

            return square.Invoke();
        }
    }
}
