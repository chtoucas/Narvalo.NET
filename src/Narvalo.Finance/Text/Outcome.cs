// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Text
{
    using System;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Outcome{T}"/>.
    /// </summary>
    public static partial class Outcome
    {
        public static Outcome<T> Success<T>(T value)
        {
            Warrant.NotNull<Outcome<T>>();

            return Outcome<T>.η(value);
        }

        public static Outcome<T> Failure<T>(string message)
        {
            Expect.NotNull(message);
            Warrant.NotNull<Outcome<T>>();

            return Outcome<T>.η(message);
        }

        public static Outcome<T> Where<T>(this Outcome<T> @this, Func<T, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.Success && predicate.Invoke(@this.Value) ? @this : Failure<T>("XXX");
        }
    }
}
