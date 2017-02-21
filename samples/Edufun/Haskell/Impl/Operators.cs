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
        public Monad<TResult> Forever<TSource, TResult>(Monad<TSource> source)
        {
            // Explanation:
            // - let {...} in ... is a let as in F# with a recursive code (let rec in F#).
            // - a' is just a syntaxic convention to say that a' is something similar to a.
            // More readable form ("a" being a monad,we replace *> by >>):
            // > forever m = let x = m >> x in x
            // that is
            // > forever m = m >> m >> m >> m >> m >> ...
            // Translated into C#:
            // > Monad<TResult> next = ReplaceBy(next);
            // > return next;
            // To make it work, we must split the initialization into two steps:
            // > Monad<TResult> next = null;
            // > next = ReplaceBy(next);
            // > return next;
            // Another way of seeing this is:
            // > forever m = m >> forever m
            // In C#:
            // > return ReplaceBy(Forever<TResult>());
            // I think that the last one won't work as expected since the inner Forever
            // will be evaluated before ReplaceBy (but it works in Haskell due to lazy evaluation).
            // Remember that ReplaceBy(next) is just Bind(_ => next). If Bind is doing nothing,
            // Forever() is useless, it just loops forever.
            Monad<TResult> next = null;
            next = source.ReplaceBy(next);
            return next;
        }

        // This one works if Monad<TResult> is a struct.
        public Monad<TResult> Forever_<TSource, TResult>(Monad<TSource> source)
        {
            var next = __ReplaceBy<TSource, TResult>(source);

            return next(source);
        }

        private static Func<Monad<TSource>, Monad<TResult>> __ReplaceBy<TSource, TResult>(Monad<TSource> value)
        {
            Func<Func<Monad<TSource>, Monad<TResult>>, Func<Monad<TSource>, Monad<TResult>>> g
                = f => next => f(value.ReplaceBy(next));

            return YCombinator.Fix(g);
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

        public Func<Monad<TSource>, Monad<TResult>> Lift<TSource, TResult>(Func<TSource, TResult> func)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<TResult>> Lift<T1, T2, TResult>(Func<T1, T2, TResult> func)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>> Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>> Lift<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
        {
            throw new NotImplementedException();
        }

        public Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>> Lift<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}