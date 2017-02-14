// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Func{T}"/> and <see cref="Action"/>.
    /// </summary>
    public static partial class Func
    {
        #region TryInvoke for Action.

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static VoidOrError TryInvoke<TException>(Action action) where TException : Exception
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return VoidOrError.FromError(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static VoidOrError TryInvoke<T1Exception, T2Exception>(Action action)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            ExceptionDispatchInfo edi;

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.FromError(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static VoidOrError TryInvoke<T1Exception, T2Exception, T3Exception>(Action action)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            ExceptionDispatchInfo edi;

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.FromError(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static VoidOrError TryInvoke<T1Exception, T2Exception, T3Exception, T4Exception>(Action action)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            ExceptionDispatchInfo edi;

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.FromError(edi);
        }

        #endregion

        #region TryInvoke for Func<T>.

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> TryInvoke<TResult, TException>(Func<TResult> thunk) where TException : Exception
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            try
            {
                TResult result = thunk.Invoke();

                return Outcome.Of(result);
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Outcome.FromError<TResult>(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> TryInvoke<TResult, T1Exception, T2Exception>(Func<TResult> thunk)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke();

                return Outcome.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.FromError<TResult>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> TryInvoke<TResult, T1Exception, T2Exception, T3Exception>(Func<TResult> thunk)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke();

                return Outcome.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.FromError<TResult>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> TryInvoke<TResult, T1Exception, T2Exception, T3Exception, T4Exception>(
            Func<TResult> thunk)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke();

                return Outcome.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.FromError<TResult>(edi);
        }

        #endregion

        #region TryInvoke for Func<T1, T2>.

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> TryInvoke<TSource, TResult, TException>(
            Func<TSource, TResult> thunk,
            TSource value)
            where TException : Exception
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            try
            {
                TResult result = thunk.Invoke(value);

                return Outcome.Of(result);
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return Outcome.FromError<TResult>(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> TryInvoke<TSource, TResult, T1Exception, T2Exception>(
            Func<TSource, TResult> thunk,
            TSource value)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke(value);

                return Outcome.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.FromError<TResult>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> TryInvoke<TSource, TResult, T1Exception, T2Exception, T3Exception>(
            Func<TSource, TResult> thunk,
            TSource value)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke(value);

                return Outcome.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.FromError<TResult>(edi);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "[Intentionally] There is no way we can achieve the same thing with type parameter inference.")]
        public static Outcome<TResult> TryInvoke<TSource, TResult, T1Exception, T2Exception, T3Exception, T4Exception>(
            Func<TSource, TResult> thunk,
            TSource value)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Outcome<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke(value);

                return Outcome.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T3Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T4Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return Outcome.FromError<TResult>(edi);
        }

        #endregion
    }

    // Provides extension methods for Func<..., Result<T, TError>> in the Kleisli category.
    public static partial class Func
    {
        #region Applicative

        public static Result<TResult, TError> Apply<TSource, TResult, TError>(
            this Result<Func<TSource, TResult>, TError> @this,
            Result<TSource, TError> value)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(value, nameof(value));

            return @this.Bind(thunk => value.Select(v => thunk.Invoke(v)));
        }

        #endregion

        #region Basic Monad functions (Prelude)

        public static Result<IEnumerable<TResult>, TError> ForEach<TSource, TResult, TError>(
            this Func<TSource, Result<TResult, TError>> @this,
            IEnumerable<TSource> seq)
        {
            Expect.NotNull(@this);
            Expect.NotNull(seq);

            return seq.Select(@this).EmptyIfNull().Collect();
        }

        public static Result<TResult, TError> Invoke<TSource, TResult, TError>(
            this Func<TSource, Result<TResult, TError>> @this,
            Result<TSource, TError> value)
        {
            Require.NotNull(value, nameof(value));
            Expect.NotNull(@this);

            return value.Bind(@this);
        }

        public static Func<TSource, Result<TResult, TError>> Compose<TSource, TMiddle, TResult, TError>(
            this Func<TSource, Result<TMiddle, TError>> @this,
            Func<TMiddle, Result<TResult, TError>> thunk)
        {
            Require.NotNull(@this, nameof(@this));
            Expect.NotNull(thunk);
            Warrant.NotNull<Func<TSource, Result<TResult, TError>>>();

            return _ => @this.Invoke(_).Bind(thunk);
        }

        public static Func<TSource, Result<TResult, TError>> ComposeBack<TSource, TMiddle, TResult, TError>(
            this Func<TMiddle, Result<TResult, TError>> @this,
            Func<TSource, Result<TMiddle, TError>> thunk)
        {
            Expect.NotNull(@this);
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Func<TSource, Result<TResult, TError>>>();

            return _ => thunk.Invoke(_).Bind(@this);
        }

        #endregion
    }
}
