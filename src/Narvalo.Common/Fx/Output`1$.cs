// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides extension methods for <see cref="Narvalo.Fx.Maybe{T}"/>.
    /// </summary>
    public static partial class OutputExtensions
    {
        #region Monad Prelude

        public static Output<TResult> Zip<TFirst, TSecond, TResult>(
            this Output<TFirst> @this,
            Output<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            return @this.Bind(firstValue => second.Map(secondValue => resultSelector.Invoke(firstValue, secondValue)));
        }

        #endregion

        #region Weak MonadZero

        public static Output<TSource> Run<TSource>(this Output<TSource> @this, Action<TSource> action)
        {
            return OnSuccess(@this, action);
        }

        public static Output<TSource> OnZero<TSource>(this Output<TSource> @this, Action<ExceptionDispatchInfo> action)
        {
            return OnFailure(@this, action);
        }

        public static Output<TResult> Coalesce<TSource, TResult>(
            this Output<TSource> @this,
            Output<TResult> whenSuccess,
            Output<TResult> whenFailure)
        {
            Require.Object(@this);

            return @this.IsSuccess ? whenSuccess : whenFailure;
        }

        #endregion

        //// OnSuccess & OnFailure

        public static Output<TSource> OnSuccess<TSource>(this Output<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            if (@this.IsSuccess) {
                action.Invoke(@this.Value);
            }

            return @this;
        }

        public static Output<TSource> OnFailure<TSource>(this Output<TSource> @this, Action<ExceptionDispatchInfo> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            if (@this.IsFailure) {
                action.Invoke(@this.ExceptionInfo);
            }

            return @this;
        }
    }
}
