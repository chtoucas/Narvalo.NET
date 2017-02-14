// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Result{T, TError}"/>.
    /// </summary>
    public static partial class Result
    {
        public static Result<T, TError> FromError<T, TError>(TError value)
        {
            Warrant.NotNull<Result<T, TError>>();

            return Result<T, TError>.η(value);
        }

        public static void ThrowIfError<T>(this Result<T, ExceptionDispatchInfo> @this)
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                @this.Error.Throw();
            }
        }

        public static void ThrowIfError<T, TException>(this Result<T, TException> @this) where TException : Exception
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                throw @this.Error;
            }
        }
    }
}
