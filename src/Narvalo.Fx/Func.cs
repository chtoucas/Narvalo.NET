// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    using static System.Diagnostics.Contracts.Contract;

    public static class Func
    {
        public static Func<T> Return<T>(T value)
        {
            Ensures(Result<Func<T>>() != null);

            return () => value;
        }

        public static Func<T> Flatten<T>(Func<Func<T>> square)
        {
            Require.NotNull(square, nameof(square));

            return square.Invoke();
        }
    }
}
