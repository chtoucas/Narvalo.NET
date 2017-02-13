// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class Result
    {
        public static Result<T, TError> Of<T, TError>(T value)
        {
            Warrant.NotNull<Result<T, TError>>();

            return Result<T, TError>.η(value);
        }

        public static Result<T, TError> FromError<T, TError>(TError value)
        {
            Warrant.NotNull<Result<T, TError>>();

            return Result<T, TError>.η(value);
        }

        #region Generalisations of list functions (Prelude)

        public static Result<T, TError> Flatten<T, TError>(Result<Result<T, TError>, TError> square)
        {
            Expect.NotNull(square);

            return Result<T, TError>.μ(square);
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        public static Func<Result<T, TError>, Result<TResult, TError>> Lift<T, TResult, TError>(
            Func<T, TResult> thunk)
        {
            Warrant.NotNull<Func<Result<T, TError>, Maybe<TResult>>>();

            return _ => _.Select(thunk);
        }

        public static Func<Result<T1, TError>, Result<T2, TError>, Result<TResult, TError>>
            Lift<T1, T2, TResult, TError>(
            Func<T1, T2, TResult> thunk)
        {
            Warrant.NotNull<Func<Result<T1, TError>, Result<T2, TError>, Result<TResult, TError>>>();

            return (m1, m2) => m1.Zip(m2, thunk);
        }

        public static Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<TResult, TError>>
            Lift<T1, T2, T3, TResult, TError>(Func<T1, T2, T3, TResult> thunk)
        {
            Warrant.NotNull<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<TResult, TError>>>();

            return (m1, m2, m3) => m1.Zip(m2, m3, thunk);
        }

        public static Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<TResult, TError>>
            Lift<T1, T2, T3, T4, TResult, TError>(
            Func<T1, T2, T3, T4, TResult> thunk)
        {
            Warrant.NotNull<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<TResult, TError>>>();

            return (m1, m2, m3, m4) => m1.Zip(m2, m3, m4, thunk);
        }

        public static Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<TResult, TError>>
            Lift<T1, T2, T3, T4, T5, TResult, TError>(
            Func<T1, T2, T3, T4, T5, TResult> thunk)
        {
            Warrant.NotNull<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<TResult, TError>>>();

            return (m1, m2, m3, m4, m5) => m1.Zip(m2, m3, m4, m5, thunk);
        }

        #endregion
    }

    // Provides the core monadic extension methods for Result<T, TError>.
    public static partial class Result
    {
        #region Monadic lifting operators (Prelude)

        public static Result<TResult, TError> Zip<TFirst, TSecond, TResult, TError>(
            this Result<TFirst, TError> @this,
            Result<TSecond, TError> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(v1 => second.Select(v2 => resultSelector.Invoke(v1, v2)));
        }

        public static Result<TResult, TError> Zip<T1, T2, T3, TResult, TError>(
            this Result<T1, TError> @this,
            Result<T2, TError> second,
            Result<T3, TError> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, Result<TResult, TError>> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        public static Result<TResult, TError> Zip<T1, T2, T3, T4, TResult, TError>(
             this Result<T1, TError> @this,
             Result<T2, TError> second,
             Result<T3, TError> third,
             Result<T4, TError> fourth,
             Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, Result<TResult, TError>> g
                = t1 => second.Zip(
                    third,
                    fourth,
                    (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        public static Result<TResult, TError> Zip<T1, T2, T3, T4, T5, TResult, TError>(
            this Result<T1, TError> @this,
            Result<T2, TError> second,
            Result<T3, TError> third,
            Result<T4, TError> fourth,
            Result<T5, TError> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, Result<TResult, TError>> g
                = t1 => second.Zip(
                    third,
                    fourth,
                    fifth,
                    (t2, t3, t4, t5) => resultSelector.Invoke(t1, t2, t3, t4, t5));

            return @this.Bind(g);
        }

        #endregion

        #region Query Expression Pattern

        public static Result<TResult, TError> SelectMany<TSource, TMiddle, TResult, TError>(
            this Result<TSource, TError> @this,
            Func<TSource, Result<TMiddle, TError>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(valueSelector, nameof(valueSelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(
                _ => valueSelector.Invoke(_).Select(
                    middle => resultSelector.Invoke(_, middle)));
        }

        #endregion
    }
}
