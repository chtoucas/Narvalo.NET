﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Advanced
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides extension methods for <see cref="Func{T}"/>.
    /// </summary>
    public static class FuncExtensions
    {
        public static Func<TResult> Bind<TSource, TResult>(
            this Func<TSource> @this,
            Func<TSource, Func<TResult>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return selector.Invoke(@this.Invoke());
        }

        public static Func<TResult> Select<TSource, TResult>(this Func<TSource> @this, Func<TSource, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            Warrant.NotNull<Func<TResult>>();

            return () => selector.Invoke(@this.Invoke());
        }

        #region Extensions for Func<Nullable<T>> - Basic Monad functions (Prelude)

        public static TResult? Invoke<TSource, TResult>(
            this Func<TSource, TResult?> @this,
            TSource? value)
            where TSource : struct
            where TResult : struct
        {
            Expect.NotNull(@this);

            return value.Bind(@this);
        }

        public static Func<TSource, TResult?> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, TMiddle?> @this,
            Func<TMiddle, TResult?> funM)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Func<TSource, TResult?>>();

            return _ => @this.Invoke(_).Bind(funM);
        }

        public static Func<TSource, TResult?> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, TResult?> @this,
            Func<TSource, TMiddle?> funM)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(funM, nameof(funM));
            Warrant.NotNull<Func<TSource, TResult?>>();

            return _ => funM.Invoke(_).Bind(@this);
        }

        #endregion

        #region Extensions for Func<Outcome<T>>

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> Catch<TResult, TException>(this Func<TResult> @this) where TException : Exception
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Outcome<TResult>>();

            try
            {
                TResult value = @this.Invoke();

                return Outcome.Success(value);
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Outcome.Failure<TResult>(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-17-0", Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public static Outcome<TResult> Catch<TResult, T1Exception, T2Exception>(this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult value = @this.Invoke();

                return Outcome.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.Failure<TResult>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-17-0", Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public static Outcome<TResult> Catch<TResult, T1Exception, T2Exception, T3Exception>(this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult value = @this.Invoke();

                return Outcome.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.Failure<TResult>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        [SuppressMessage("Microsoft.Contracts", "Suggestion-17-0", Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public static Outcome<TResult> Catch<TResult, T1Exception, T2Exception, T3Exception, T4Exception>(
            this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult value = @this.Invoke();

                return Outcome.Success(value);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.Failure<TResult>(edi);
        }

        #endregion
    }
}
