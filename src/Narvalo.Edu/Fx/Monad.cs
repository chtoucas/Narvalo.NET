// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    static class Monad
    {
        static readonly Monad<Unit> Unit_ = Return(Narvalo.Edu.Fx.Unit.Single);
#if !MONAD_DISABLE_ZERO
        static readonly Monad<Unit> Zero_ = Monad<Unit>.Zero;
#endif

        public static Monad<Unit> Unit { get { return Unit_; } }

#if !MONAD_DISABLE_ZERO
        // [Haskell] mzero
        public static Monad<Unit> Zero { get { return Zero_; } }
#endif

        // [Haskell] return
        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }

        #region Generalisations of list functions

        // [Haskell] join
        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
        {
            return Monad<T>.μ(square);
        }

        #endregion

        #region Conditional execution of monadic expressions

#if !MONAD_DISABLE_ZERO
        // [Haskell] guard
        public static Monad<Unit> Guard<TSource>(bool predicate)
        {
            return predicate ? Unit : Zero;
        }
#endif

        // [Haskell] when
        public static Monad<Unit> When<TSource>(bool predicate, Kunc<Unit, Unit> action)
        {
            Require.NotNull(action, "action");

            return predicate ? action.Invoke(Narvalo.Edu.Fx.Unit.Single) : Unit;
        }

        // [Haskell] unless
        public static Monad<Unit> Unless<TSource>(bool predicate, Kunc<Unit, Unit> action)
        {
            return When<TSource>(!predicate, action);
        }

        #endregion
    }
}
