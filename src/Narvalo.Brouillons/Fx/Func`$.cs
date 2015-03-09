// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public static partial class FuncExtensions
    {
        public static Func<TResult> Bind<TSource, TResult>(this Func<TSource> @this, Func<TSource, Func<TResult>> selector)
        {
            Require.NotNull(selector, "selector");

            return selector.Invoke(@this.Invoke());
        }

        public static Func<TResult> Select<TSource, TResult>(this Func<TSource> @this, Func<TSource, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return () => selector.Invoke(@this.Invoke());
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this")]
        public static Func<TResult> Then<TSource, TResult>(this Func<TSource> @this, Func<TResult> other)
        {
            return other;
        }

        #region Basic Monad functions for Nullable

        public static Func<TSource, TResult?> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, TMiddle?> @this,
            Func<TMiddle, TResult?> kun)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.Object(@this);

            return _ => @this.Invoke(_).Bind(kun);
        }

        public static Func<TSource, TResult?> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, TResult?> @this,
            Func<TSource, TMiddle?> kun)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => kun.Invoke(_).Bind(@this);
        }

        #endregion
    }

    public static partial class FuncExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<TResult> Catch<TException, TResult>(this Func<TResult> @this)
            where TException : Exception
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Output<TResult>>() != null);

            return Make<TResult>.Catch<TException>(@this);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<TResult> Catch<T1Exception, T2Exception, TResult>(this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Output<TResult>>() != null);

            return Make<TResult>.Catch<T1Exception, T2Exception>(@this);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<TResult> Catch<T1Exception, T2Exception, T3Exception, TResult>(this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Output<TResult>>() != null);

            return Make<TResult>.Catch<T1Exception, T2Exception, T3Exception>(@this);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "There is no way we can achieve the same thing with type inference.")]
        public static Output<TResult> Catch<T1Exception, T2Exception, T3Exception, T4Exception, TResult>(this Func<TResult> @this)
            where T1Exception : Exception
            where T2Exception : Exception
            where T3Exception : Exception
            where T4Exception : Exception
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Output<TResult>>() != null);

            return Make<TResult>.Catch<T1Exception, T2Exception, T3Exception, T4Exception>(@this);
        }
    }
}
