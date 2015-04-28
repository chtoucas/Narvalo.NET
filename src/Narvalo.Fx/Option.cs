// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Option{T}"/>.
    /// </summary>
    public static partial class Option
    {
        public static Option<T> Of<T>(T? value) where T : struct
        {
            return value.HasValue ? Option<T>.η(value.Value) : Option<T>.None;
        }
    }

    /// <content>
    /// Provides extension methods for <see cref="Option{T}"/> when <c>T</c> is a struct.
    /// </content>
    public static partial class Option
    {
        public static T? ToNullable<T>(this Option<T?> @this) where T : struct
        {
            return @this.ValueOrDefault();
        }

        public static T? ToNullable<T>(this Option<T> @this) where T : struct
        {
            return @this.IsSome ? (T?)@this.Value : null;
        }

        public static T UnpackOrDefault<T>(this Option<T?> @this) where T : struct
        {
            Acknowledge.Object(@this);

            return UnpackOrElse(@this, default(T));
        }

        public static T UnpackOrElse<T>(this Option<T?> @this, T defaultValue) where T : struct
        {
            Acknowledge.Object(@this);

            return UnpackOrElse(@this, () => defaultValue);
        }

        public static T UnpackOrElse<T>(this Option<T?> @this, Func<T> defaultValueFactory) where T : struct
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.ValueOrDefault() ?? defaultValueFactory.Invoke();
        }

        public static T UnpackOrThrow<T>(this Option<T?> @this, Exception exception) where T : struct
        {
            Acknowledge.Object(@this);

            return UnpackOrThrow(@this, () => exception);
        }

        public static T UnpackOrThrow<T>(this Option<T?> @this, Func<Exception> exceptionFactory) where T : struct
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            T? m = @this.ValueOrDefault();

            if (!m.HasValue)
            {
                throw exceptionFactory.Invoke();
            }

            return m.Value;
        }
    }
}
