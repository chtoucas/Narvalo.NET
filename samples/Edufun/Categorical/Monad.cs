// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define MONAD_VIA_MAP_MULTIPLY

namespace Edufun.Categorical
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    public sealed class Monad<T>
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static Monad<T> Zero { get { throw new NotImplementedException(); } }

        // [Haskell] mplus
        public Monad<T> Plus(Monad<T> other)
        {
            throw new NotImplementedException();
        }

        // [Haskell] >>=
        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad<TResult>.Join(Select(_ => kun.Invoke(_)));
#else
            throw new NotImplementedException();
#endif
        }

        // [Haskell] fmap
        public Monad<TResult> Select<TResult>(Func<T, TResult> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            return Bind(_ => Monad<TResult>.Return(selector.Invoke(_)));
#endif
        }

        // [Haskell] >>
        public Monad<TResult> ReplaceBy<TResult>(Monad<TResult> other)
            => Bind(_ => other);

        // [Haskell] return (η)
        internal static Monad<T> Return(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join (μ)
        internal static Monad<T> Join(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            Kunc<Monad<T>, T> id = _ => _;

            return square.Bind(id);
#endif
        }
    }

    public static partial class Monad
    {
        public static readonly Monad<Unit> Unit = Of(Narvalo.Fx.Unit.Single);

        public static Monad<T> Of<T>(T value) => Monad<T>.Return(value);

        public static Monad<T> Flatten<T>(Monad<Monad<T>> square) => Monad<T>.Join(square);
    }

    // Extension methods
    public static partial class Monad
    {
        // [Haskell] void
        public static Monad<Unit> Skip<T>(this Monad<T> @this)
            => @this.Select(_ => Narvalo.Fx.Unit.Single);
    }
}
