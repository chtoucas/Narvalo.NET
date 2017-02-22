// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define MONAD_VIA_MAP_MULTIPLY

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Fx.Linq;

    public partial class Monad
    {
        public static Monad<T> Of<T>(T value) { throw new FakeClassException(); }

        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new FakeClassException();
#else
            Func<Monad<T>, Monad<T>> id = _ => _;

            return square.Bind(id);
#endif
        }
    }

    public partial class Monad<T> : IMonad<T>
    {
        // Control.Monad: >>=
        public Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad.Flatten(Select(_ => selector.Invoke(_)));
#else
            throw new FakeClassException();
#endif
        }

        // Control.Monad: fmap f xs = xs >>= return . f
        public Monad<TResult> Select<TResult>(Func<T, TResult> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new FakeClassException();
#else
            return Bind(_ => Monad.Of(selector.Invoke(_)));
#endif
        }

        // Control.Monad: return
        public Monad<TSource> Of_<TSource>(TSource value) => Monad.Of(value);

        // Control.Monad: join
        public Monad<TSource> Flatten_<TSource>(Monad<Monad<TSource>> square) => Monad.Flatten(square);
    }

    public partial class Monad<T> : IMonadSyntax<T>
    {
        // [GHC.Base] ap m1 m2 = do { x1 <- m1; x2 <- m2; return (x1 x2) }
        // Control.Monad: <*> = ap
        public Monad<TResult> Gather<TResult>(Monad<Func<T, TResult>> applicative)
            => Bind(arg => applicative.Select(func => func(arg)));

        // [Control.Monad]
        //  replicateM cnt0 f =
        //    loop cnt0
        //  where
        //    loop cnt
        //      | cnt <= 0  = pure[]
        //      | otherwise = liftA2(:) f(loop (cnt - 1))
        public Monad<IEnumerable<T>> Repeat(int count) => Select(_ => Enumerable.Repeat(_, count));

        // [GHC.Base] m >> k = m >>= \_ -> k
        public Monad<TResult> ReplaceBy<TResult>(Monad<TResult> other) => Bind(_ => other);

        // [Data.Functor] void x = () <$ x
        // Strictly speaking, this should be Replace(Unit.Single); but we do not have Replace.
        public Monad<Unit> Skip() => Select(_ => Unit.Single);
    }

    public partial class Monad : IMonadOperators
    {
        public Monad<IEnumerable<TSource>> Collect<TSource>(IEnumerable<Monad<TSource>> @this)
        {
            var seed = Monad.Of(Enumerable.Empty<TSource>());
            Func<IEnumerable<TSource>, TSource, IEnumerable<TSource>> append = (m, item) => m.Append(item);

            return @this.Aggregate(seed, Lift(append));
        }

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
            // (NB: The previous code does not work if Monad<T> is a struct; see Forever_().)
            // Another way of seeing forever is:
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

        // This one works if Monad<T> is a struct.
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

        // <$!>
        public Monad<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Monad<TSource> value)
            => value.Select(selector);

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

        // [Control.Monad] when p s = if p then pure () else s
        public Monad<Unit> Unless(bool predicate, Monad<Unit> value)
            => predicate ? Monad.Of(Unit.Single) : value;

        // [GHC.Base] when p s = if p then s else pure ()
        public Monad<Unit> When(bool predicate, Monad<Unit> value)
            => predicate ? value : Monad.Of(Unit.Single);
    }
}
