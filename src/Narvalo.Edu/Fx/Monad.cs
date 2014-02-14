// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Diagnostics.CodeAnalysis;

    static class Monad
    {
        static readonly Monad<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);
        static readonly Monad<Unit> Zero_ = Monad<Unit>.Zero;

        public static Monad<Unit> Unit { get { return Unit_; } }

        // WARNING: Only for Monads with a Zero.
        public static Monad<Unit> Zero { get { return Zero_; } }

        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }

        #region Generalisations of list functions

        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
        {
            return Monad<T>.μ(square);
        }

        #endregion

        #region Conditional execution of monadic expressions

        // [Haskell] guard
        // guard b is return () if b is True, and mzero if b is False.
        // WARNING: Private since it won't be implemented for a concrete Monad.
        // WARNING: Only for Monads with a Zero.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Monad template definition.")]
        static Monad<Unit> Guard<TSource>(bool predicate)
        {
            return predicate ? Unit : Zero;
        }

        // [Haskell] when
        // Conditional execution of monadic expressions.
        // WARNING: Private since it won't be implemented for a concrete Monad.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Monad template definition.")]
        static Monad<Unit> When<TSource>(bool predicate, Kunc<Unit, Unit> action)
        {
            Require.NotNull(action, "action");

            return predicate ? action.Invoke(Narvalo.Fx.Unit.Single) : Unit;
        }

        // [Haskell] unless
        // The reverse of when.
        // WARNING: Private since it won't be implemented for a concrete Monad.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "Monad template definition.")]
        static Monad<Unit> Unless<TSource>(bool predicate, Kunc<Unit, Unit> action)
        {
            return When<TSource>(!predicate, action);
        }

        #endregion
    }
}
