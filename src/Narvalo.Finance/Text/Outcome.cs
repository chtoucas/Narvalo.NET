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

            return Outcome<T>.ReturnSuccess(value);
        }

        public static Outcome<T> Failure<T>(string message)
        {
            Expect.NotNullOrEmpty(message);
            Warrant.NotNull<Outcome<T>>();

            return Outcome<T>.ReturnFailure(message);
        }
    }
}
