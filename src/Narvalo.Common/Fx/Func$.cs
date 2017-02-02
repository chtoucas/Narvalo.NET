// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides extension methods for <see cref="Func{T}"/>.
    /// </summary>
    public static partial class FuncExtensions
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
    }
}
