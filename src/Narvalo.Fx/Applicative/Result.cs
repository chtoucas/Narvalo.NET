// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static partial class Result
    {
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "[Intentionally] Fluent API.")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "[Intentionally] Matches the property IsError.")]
        public static class OfError<TError>
        {
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
            public static Result<T, TError> Return<T>(T value) => Result<T, TError>.Of(value);
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "[Intentionally] Fluent API.")]
        public static class OfType<T>
        {
            [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
            public static Result<T, TError> FromError<TError>(TError error) => Result<T, TError>.FromError(error);
        }
    }

    public static partial class Result
    {
        public static Result<T, TError> FlattenError<T, TError>(Result<T, Result<T, TError>> square)
            => Result<T, TError>.FlattenError(square);
    }

    // Provides extension methods for Result<T, TError> where TError is of type Exception.
    public static partial class Result
    {
        public static void ThrowIfError<T, TException>(this Result<T, TException> @this)
            where TException : Exception
        {
            // NB: The Error property is never null.
            if (@this.IsError) { throw @this.Error; }
        }
    }
}
