// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Fx.Linq;

    public class Operators : IFunctorOperators, IApplicativeOperators, IMonadOperators
    {
        #region Apply

        // GHC.Base: (<**>) = liftA2 (flip ($))
        public Applicative<TResult> Apply<TSource, TResult>(
            Applicative<Func<TSource, TResult>> @this,
            Applicative<TSource> value)
            => value.Gather(@this);

        #endregion

        #region Collect

        public Monad<IEnumerable<TSource>> Collect<TSource>(IEnumerable<Monad<TSource>> @this)
        {
            var seed = Monad.Of(Enumerable.Empty<TSource>());
            Func<IEnumerable<TSource>, TSource, IEnumerable<TSource>> append = (m, item) => m.Append(item);

            return @this.Aggregate(seed, Lift(append));
        }

        #endregion

        #region Forever

        // [Control.Monad] forever a = let a' = a *> a' in a'
        public void Forever<TSource>(Monad<TSource> value)
        {
            // Explanation:
            // - let {...} in ... is the recursive let (letrec).
            // - a' is just a syntaxic convention to say that a' is something similar to a.
            // More readable form:
            // > forever m = let x = m >> x in x
            Func<Monad<TSource>, Monad<TSource>> rec = _ => value.ReplaceBy(_);
            rec.Invoke(value);
        }

        #endregion

        #region Guard

        public Monad<Unit> Guard(bool predicate)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Inject

        // ($>) = flip (<$)
        public Functor<TResult> Inject<TSource, TResult>(TResult other, Functor<TSource> value)
            => value.Replace(other);

        #endregion

        #region InvokeWith

        // (<$>) = fmap
        public Functor<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Functor<TSource> value)
            => value.Select(selector);

        // Data.Functor: (<$>) = fmap
        public Applicative<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Applicative<TSource> value)
            => value.Select(selector);

        // <$!>
        public Monad<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Monad<TSource> value)
            => value.Select(selector);

        #endregion

        #region Lift

        public Func<Monad<TSource>, Monad<TResult>> Lift<TSource, TResult>(Func<TSource, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<TResult>> Lift<T1, T2, TResult>(Func<T1, T2, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>> Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>> Lift<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>> Lift<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> thunk)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}