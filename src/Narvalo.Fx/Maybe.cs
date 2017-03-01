// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Maybe{T}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Maybe{S}"/>.
    /// </summary>
    public static partial class Maybe
    {
        // Conversion from T? to Maybe<T>.
        public static Maybe<T> Of<T>(T? value) where T : struct
            => value.HasValue ? Of(value.Value) : Maybe<T>.None;

        // Conversion from Maybe<T> to T?.
        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
            => @this.IsSome ? (T?)@this.Value : null;
    }

    // Provides extension methods for Maybe<T?>.
    // NB: There is really no reason to use Maybe<T> or Maybe<T?> instead of T? (for value
    // types of course).
    public static partial class Maybe
    {
        public static Maybe<T> Flatten<T>(this Maybe<T?> @this) where T : struct
            => @this.IsSome ? Maybe.Of(@this.Value) : Maybe<T>.None;

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
            => @this.IsSome ? @this.Value : null;

        // REVIEW: Returning @this.Value should be enough.
        public static T Unwrap<T>(this Maybe<T?> @this) where T : struct
            => @this.ValueOrDefault() ?? default(T);

        public static T Unwrap<T>(this Maybe<T?> @this, T defaultValue) where T : struct
            => @this.ValueOrDefault() ?? defaultValue;

        public static T Unwrap<T>(this Maybe<T?> @this, Func<T> defaultValueFactory) where T : struct
        {
            Require.NotNull(defaultValueFactory, nameof(defaultValueFactory));

            return @this.ValueOrDefault() ?? defaultValueFactory();
        }

        public static T UnwrapOrThrow<T>(this Maybe<T?> @this, Exception exception) where T : struct
            => UnwrapOrThrow(@this, () => exception);

        public static T UnwrapOrThrow<T>(this Maybe<T?> @this, Func<Exception> exceptionFactory) where T : struct
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            T? m = @this.ValueOrDefault();

            if (!m.HasValue)
            {
                throw exceptionFactory();
            }

            return m.Value;
        }
    }

    // Provides static helpers for Maybe<TError> where TError is of type Exception or ExceptionDispatchInfo.
    public static partial class Maybe
    {
        public static void ThrowIfError(this Maybe<ExceptionDispatchInfo> @this)
        {
            if (@this.IsSome) { @this.Value.Throw(); }
        }

        public static void ThrowIfError<TException>(this Maybe<TException> @this) where TException : Exception
        {
            if (@this.IsSome) { throw @this.Value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static Maybe<ExceptionDispatchInfo> TryWith(Action action)
        {
            Require.NotNull(action, nameof(action));

            try
            {
                action();

                return Maybe<ExceptionDispatchInfo>.None;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Of(edi);
            }
        }

        // NB: This method is **not** the tryfinally from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static Maybe<ExceptionDispatchInfo> TryFinally(Action action, Action finallyAction)
        {
            Require.NotNull(action, nameof(action));
            Require.NotNull(finallyAction, nameof(finallyAction));

            try
            {
                action();

                return Maybe<ExceptionDispatchInfo>.None;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Of(edi);
            }
            finally
            {
                finallyAction();
            }
        }

    }

    // Provides extension methods for IEnumerable<Maybe<T>>.
    public static partial class Maybe
    {
        // Named <c>catMaybes</c> in Haskell parlance.
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CollectAnyIterator(@this);
        }

        private static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Demand.NotNull(source);

            foreach (var item in source)
            {
                if (item.IsSome) { yield return item.Value; }
            }
        }
    }
}
