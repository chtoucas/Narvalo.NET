﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Maybe{T}"/>.
    /// </summary>
    public static partial class Maybe
    {
        public static Maybe<T> Of<T>(T? value) where T : struct
            => value.HasValue ? Maybe<T>.η(value.Value) : Maybe<T>.None;

        #region Extension methods when T is a struct

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
            => @this.ValueOrDefault();

        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
            => @this.IsSome ? (T?)@this.Value : null;

        public static T UnpackOrDefault<T>(this Maybe<T?> @this) where T : struct
            => UnpackOrElse(@this, default(T));

        public static T UnpackOrElse<T>(this Maybe<T?> @this, T defaultValue) where T : struct
            => UnpackOrElse(@this, () => defaultValue);

        public static T UnpackOrElse<T>(this Maybe<T?> @this, Func<T> defaultValueFactory) where T : struct
        {
            Require.NotNull(defaultValueFactory, nameof(defaultValueFactory));

            return @this.ValueOrDefault() ?? defaultValueFactory.Invoke();
        }

        public static T UnpackOrThrow<T>(this Maybe<T?> @this, Exception exception) where T : struct
            => UnpackOrThrow(@this, () => exception);

        public static T UnpackOrThrow<T>(this Maybe<T?> @this, Func<Exception> exceptionFactory) where T : struct
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            T? m = @this.ValueOrDefault();

            if (!m.HasValue)
            {
                throw exceptionFactory.Invoke();
            }

            return m.Value;
        }

        #endregion
    }
}
