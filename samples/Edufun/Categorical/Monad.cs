// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Narvalo.Fx;

    public static partial class Monad
    {
        private static readonly Monad<Unit> s_Unit = Return(Narvalo.Fx.Unit.Single);

        public static Monad<Unit> Unit => s_Unit;

        // [Haskell] return
        public static Monad<T> Return<T>(T value) => Monad<T>.η(value);
    }

    public sealed class Monad<T>
    {
        // [Haskell] mzero
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
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#else
            throw new NotImplementedException();
#endif
        }

        // [Haskell] fmap
        public Monad<TResult> Map<TResult>(Func<T, TResult> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            return Bind(_ => Monad<TResult>.η(selector.Invoke(_)));
#endif
        }

        // [Haskell] >>
        public Monad<TResult> Then<TResult>(Monad<TResult> other)
            => Bind(_ => other);

        // [Haskell] return
        internal static Monad<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] join
        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new NotImplementedException();
#else
            Kunc<Monad<T>, T> id = _ => _;

            return square.Bind(id);
#endif
        }
    }
}
