// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static partial class Result
    {
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "[Intentionally] Fluent API.")]
        public static class OfTError<TError>
        {
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
            public static Result<T, TError> Of<T>(T value) => Result<T, TError>.Of(value);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "[Intentionally] Fluent API.")]
        public static class OfType<T>
        {
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
            public static Result<T, TError> FromError<TError>(TError error) => Result<T, TError>.FromError(error);
        }
    }

    public static partial class ResultL
    {
        public static Result<T, TError> Flatten<T, TError>(
            this Result<T, Result<T, TError>> square)
            => Result<T, TError>.FlattenError(square);

        public static void ThrowIfError<T, TException>(this Result<T, TException> @this)
            where TException : Exception
        {
            // NB: The Error property is never null.
            if (@this.IsError) { throw @this.Error; }
        }
    }
}
