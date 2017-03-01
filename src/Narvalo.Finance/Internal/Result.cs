// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    /// <summary>
    /// Provides extension methods for <see cref="Result{T}"/>.
    /// </summary>
    internal static class Result
    {
        public static Result<T> Of<T>(T value) where T : struct
            => Result<T>.Return(value);
    }
}
