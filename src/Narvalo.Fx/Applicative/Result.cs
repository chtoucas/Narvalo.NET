// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public static partial class Result
    {
        public static Result<T, TError> FlattenError<T, TError>(Result<T, Result<T, TError>> square)
            => Result<T, TError>.FlattenError(square);
    }

    // Provides extension methods for Result<T, TError> where TError is of type Exception.
    public static partial class Result
    {
        public static void ThrowIfError<T, TException>(this Result<T, TException> @this) where TException : Exception
        {
            // NB: The Error property is never null.
            if (@this.IsError) { throw @this.Error; }
        }
    }
}
