// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.Contracts;

    public static partial class Maybe
    {
        public static Maybe<T> Create<T>(T? value) where T : struct
        {
            return value.HasValue ? Maybe<T>.η(value.Value) : Maybe<T>.None;
        }
    }

    // Extensions for Maybe<T>
    public static partial class Maybe
    {
        #region ToNullable

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
        {
            Require.Object(@this);

            return @this.ValueOrDefault();
        }

        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
        {
            Require.Object(@this);

            return @this.IsSome ? (T?)@this.Value : null;
        }

        #endregion

        #region UnpackOr...

        public static T UnpackOrDefault<T>(this Maybe<T?> @this) where T : struct
        {
            Contract.Requires(@this != null);

            return UnpackOrElse(@this, default(T));
        }

        public static T UnpackOrElse<T>(this Maybe<T?> @this, T defaultValue) where T : struct
        {
            Contract.Requires(@this != null);

            return UnpackOrElse(@this, () => defaultValue);
        }

        public static T UnpackOrElse<T>(this Maybe<T?> @this, Func<T> defaultValueFactory) where T : struct
        {
            Require.Object(@this);
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.ValueOrDefault() ?? defaultValueFactory.Invoke();
        }

        public static T UnpackOrThrow<T>(this Maybe<T?> @this, Exception exception) where T : struct
        {
            Contract.Requires(@this != null);

            return UnpackOrThrow(@this, () => exception);
        }

        public static T UnpackOrThrow<T>(this Maybe<T?> @this, Func<Exception> exceptionFactory) where T : struct
        {
            Require.Object(@this);

            var m = @this.ValueOrDefault().OnNull(() => { throw exceptionFactory.Invoke(); });

            Contract.Assume(m.HasValue, "If it was not the case, we would have throw an exception just above.");

            return m.Value;
        }

        #endregion
    }
}
