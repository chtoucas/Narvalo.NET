// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="Narvalo.Fx.Identity{T}"/>.
    /// </summary>
    static partial class IdentityExtensions
    {
        #region Monad extensions

        public static Identity<TResult> Zip<TFirst, TSecond, TResult>(
            this Identity<TFirst> @this,
            Identity<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            return @this.Bind(m1 => second.Map(m2 => resultSelector.Invoke(m1, m2)));
        }

        public static Identity<TSource> Run<TSource>(this Identity<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Bind(_ => { action.Invoke(_); return Identity.Unit; }).Then(@this);
        }

        public static Identity<TResult> Then<TSource, TResult>(this Identity<TSource> @this, Identity<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }

        #endregion
    }
}
