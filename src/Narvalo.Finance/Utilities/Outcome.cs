// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    /// <summary>
    /// Provides extension methods for <see cref="Outcome{T}"/>.
    /// </summary>
    public static class Outcome
    {
        public static Outcome<T> Success<T>(T value) where T : struct
        {
            Warrant.NotNull<Outcome<T>>();

            return Outcome<T>.Return(value);
        }
    }
}
