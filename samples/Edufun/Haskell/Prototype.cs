// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define APPLICATIVE_USE_GHC_BASE
//#define MONAD_VIA_MAP_MULTIPLY

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Fx.Linq;

    public partial class Prototype
    {
        public static Prototype<T> Of<T>(T value) { throw new PrototypeException(); }

        public static Prototype<T> Empty<T>() { throw new PrototypeException(); }

        // [GHC.Base] join x = x >>= id
        public static Prototype<T> Flatten<T>(Prototype<Prototype<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new PrototypeException();
#else
            Func<Prototype<T>, Prototype<T>> id = _ => _;

            return square.Bind(id);
#endif
        }
    }

    // Implements the IFunctor<T> interface.
    public partial class Prototype<T> : IFunctor<T>
    {
        // Data.Functor: fmap
        // NB: Implemented via IApplicativeSyntax<T>.
        Prototype<TResult> IFunctor<T>.Select<TResult>(Func<T, TResult> selector)
        {
            throw new PrototypeException();
        }
    }

    // Implements the IApplicative<T> interface.
    public partial class Prototype<T> : IApplicative<T>
    {
        // Control.Applicative: <*>
        // NB: Implemented via IFunctorSyntax<T>.
        Prototype<TResult> IApplicative<T>.Gather<TResult>(Prototype<Func<T, TResult>> applicative)
        {
            throw new PrototypeException();
        }

        // Control.Applicative: pure
        public Prototype<TSource> Of_<TSource>(TSource value) => Prototype.Of(value);
    }

    // Implements the IMonad<T> interface.
    public partial class Prototype<T> : IMonad<T>
    {
        // Control.Monad: >>=
        public Prototype<TResult> Bind<TResult>(Func<T, Prototype<TResult>> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Prototype.Flatten(Select(_ => selector.Invoke(_)));
#else
            throw new PrototypeException();
#endif
        }

        // Control.Monad: fmap f xs = xs >>= return . f
#if MONAD_VIA_MAP_MULTIPLY
        // NB: Implemented via IApplicativeSyntax<T>.
        Prototype<TResult> IMonad<T>.Select<TResult>(Func<T, TResult> selector)
        {
            throw new PrototypeException();
        }
#else
        public Prototype<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            return Bind(_ => Prototype.Of(selector.Invoke(_)));
        }
#endif

        // Control.Monad: return
        // NB: Already implemented via IApplicative<T>.
        Prototype<TSource> IMonad<T>.Of_<TSource>(TSource value) => Prototype.Of(value);

        // Control.Monad: join
        public Prototype<TSource> Flatten_<TSource>(Prototype<Prototype<TSource>> square) => Prototype.Flatten(square);
    }

    // Implements the IAlternative<T> interface.
    public partial class Prototype<T> : IAlternative<T>
    {
        // GHC.Base: empty = Nothing
        public Prototype<T> Empty_() => Prototype.Empty<T>();

        // GHC.Base:
        // Nothing <|> r = r
        // l <|> _ = l
        public Prototype<T> Append(Prototype<T> value)
        {
            throw new PrototypeException();
        }
    }

    // Implements the IFunctorSyntax<T> interface.
    public partial class Prototype<T> : IFunctorSyntax<T>
    {
        // [GHC.Base] (<$) = fmap . const
        public Prototype<TResult> Replace<TResult>(TResult other) => Select(_ => other);

        // [Data.Functor] void x = () <$ x
        public Prototype<Unit> Skip() => Replace(Unit.Single);
    }

    // Implements the IApplicativeSyntax<T> interface.
    public partial class Prototype<T> : IApplicativeSyntax<T>
    {
        // [GHC.Base] (<*) = liftA2 const
        // Control.Applicative: u <* v = pure const <*> u <*> v
        public Prototype<T> Ignore<TOther>(Prototype<TOther> other)
        {
#if APPLICATIVE_USE_GHC_BASE
            Func<T, TOther, T> ignorethat = (_, that) => _;

            return Zip(other, ignorethat);
#else
            Func<T, Func<TOther, T>> ignorethat = _ => that => _;

            return other.Gather(Gather(Prototype.Of(ignorethat)));
#endif
        }

        // [GHC.Base] (<$) = fmap . const
        // NB: Already implemented via IFunctorSyntax<T>.
        Prototype<TResult> IApplicativeSyntax<T>.Replace<TResult>(TResult other) => Select(_ => other);

        // [GHC.Base] a1 *> a2 = (id <$ a1) <*> a2
        // Control.Applicative: u *> v = pure (const id) <*> u <*> v
        // NB: Better implemented via IMonadSyntax<T>.
        Prototype<TResult> IApplicativeSyntax<T>.ReplaceBy<TResult>(Prototype<TResult> other)
        {
#if APPLICATIVE_USE_GHC_BASE
            Func<TResult, TResult> id = _ => _;

            return other.Gather(Replace(id));
#else
            Func<T, Func<TResult, TResult>> id = _ => r => r;

            return other.Gather(Gather(Prototype.Of(id)));
#endif
        }

        // [GHC.Base] liftA f a = pure f <*> a
        // Control.Applicative: fmap f x = pure f <*> x
#if MONAD_VIA_MAP_MULTIPLY
        public Prototype<TResult> Select<TResult>(Func<T, TResult> selector)
            => Gather(Prototype.Of(selector));
#else
        // NB: Already implemented via IMonad<T>.
        Prototype<TResult> IApplicativeSyntax<T>.Select<TResult>(Func<T, TResult> selector)
           => Gather(Prototype.Of(selector));
#endif

        // [GHC.Base] liftA2 f a b = fmap f a <*> b
        public Prototype<TResult> Zip<TSecond, TResult>(
            Prototype<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Func<T, Func<TSecond, TResult>> selector = x => y => resultSelector.Invoke(x, y);

            return second.Gather(Select(selector));
        }

        // [GHC.Base] liftA3 f a b c = fmap f a <*> b <*> c
        public Prototype<TResult> Zip<T2, T3, TResult>(
            Prototype<T2> second,
            Prototype<T3> third,
            Func<T, T2, T3, TResult> resultSelector)
        {
            Func<T, Func<T2, Func<T3, TResult>>> selector = x => y => z => resultSelector.Invoke(x, y, z);

            return third.Gather(second.Gather(Select(selector)));
        }
    }

    // Implements the IMonadSyntax<T> interface.
    public partial class Prototype<T> : IMonadSyntax<T>
    {
        // [GHC.Base] ap m1 m2 = do { x1 <- m1; x2 <- m2; return (x1 x2) }
        // Control.Monad: <*> = ap
        public Prototype<TResult> Gather<TResult>(Prototype<Func<T, TResult>> applicative)
        {
            // In Haskell, m1 is of type m (a -> b) and m2 of type m a;
            // return is not the "return" of C# but the Applicative::pure.
            // Using Select this can be simplified:
            // > return applicative.Bind(func => Select(func));
            return applicative.Bind(func => Bind(_ => Prototype.Of(func(_))));
        }

        // [Control.Monad]
        //  replicateM cnt0 f =
        //    loop cnt0
        //  where
        //    loop cnt
        //      | cnt <= 0  = pure[]
        //      | otherwise = liftA2(:) f(loop (cnt - 1))
        public Prototype<IEnumerable<T>> Repeat(int count) => Select(_ => Enumerable.Repeat(_, count));

        // [GHC.Base] m >> k = m >>= \_ -> k
        public Prototype<TResult> ReplaceBy<TResult>(Prototype<TResult> other) => Bind(_ => other);

        // [Data.Functor] void x = () <$ x
        // NB: Already implemented via IFunctorSyntax<T>.
        Prototype<Unit> IMonadSyntax<T>.Skip() => Replace(Unit.Single);
    }

    // Implements the IFunctorOperators interface.
    public partial class Prototype : IFunctorOperators
    {
        // [Data.Functor] ($>) = flip (<$)
        public Prototype<TResult> Inject<TSource, TResult>(TResult value, Prototype<TSource> functor)
            => functor.Replace(value);

        // [Data.Functor] (<$>) = fmap
        public Prototype<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> func, Prototype<TSource> functor)
            => functor.Select(func);
    }

    // Implements the IMonadOperators interface.
    public partial class Prototype : IMonadOperators
    {
        // [Data.Traversable] sequence = sequenceA
        public Prototype<IEnumerable<TSource>> Collect<TSource>(IEnumerable<Prototype<TSource>> source)
        {
            // The signature of sequence is
            //   sequence :: (Traversable t, Monad m) => t (m a) -> m (t a)
            // Here we interpret Traversable as IEnumerable.
            // The definition for sequenceA for lists is:
            //   [Haskell] sequenceA :: Applicative f => [f a] -> f [a]
            //   sequenceA = foldr (liftA2 (:)) (pure [])
            var seed = Prototype.Of(Enumerable.Empty<TSource>());
            Func<IEnumerable<TSource>, TSource, IEnumerable<TSource>> append
                = (seq, item) => seq.Append(item);

            return source.Aggregate(seed, Lift(append));
        }

        #region Forever

        // [Control.Monad] forever a = let a' = a *> a' in a'
        public Prototype<TResult> Forever<TSource, TResult>(Prototype<TSource> source)
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
            Prototype<TResult> next = null;
            next = source.ReplaceBy(next);
            return next;
        }

        // This one works even if Monad<T> is a struct.
        public Prototype<TResult> Forever_<TSource, TResult>(Prototype<TSource> source)
        {
            var next = __ReplaceBy<TSource, TResult>(source);

            return next(source);
        }

        private static Func<Prototype<TSource>, Prototype<TResult>> __ReplaceBy<TSource, TResult>(Prototype<TSource> value)
        {
            Func<Func<Prototype<TSource>, Prototype<TResult>>, Func<Prototype<TSource>, Prototype<TResult>>> g
                = f => next => f(value.ReplaceBy(next));

            return YCombinator.Fix(g);
        }

        #endregion

        // [Control.Monad]
        // f <$!> m = do
        //   x <- m
        //   let z = f x
        //   z `seq` return z
        // NB: Already implemented via IFunctorOperators.
        Prototype<TResult> IMonadOperators.InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Prototype<TSource> value)
            => value.Select(selector);

        #region Lift

        // [GHC.Base] liftM f m1 = do { x1 <- m1; return (f x1) }
        public Func<Prototype<TSource>, Prototype<TResult>> Lift<TSource, TResult>(Func<TSource, TResult> func)
            => m => m.Bind(arg => Prototype.Of(func(arg)));

        // [GHC.Base] liftM2 f m1 m2 = do { x1 <- m1; x2 <- m2; return (f x1 x2) }
        public Func<Prototype<T1>, Prototype<T2>, Prototype<TResult>> Lift<T1, T2, TResult>(Func<T1, T2, TResult> func)
            => (m1, m2) => m1.Bind(
                arg1 => m2.Bind(
                    arg2 => Prototype.Of(func(arg1, arg2))));

        // [GHC.Base] liftM3 f m1 m2 m3 = do { x1 <- m1; x2 <- m2; x3 <- m3; return (f x1 x2 x3) }
        public Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<TResult>> Lift<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> func)
            => (m1, m2, m3) => m1.Bind(
                arg1 => m2.Bind(
                    arg2 => m3.Bind(
                        arg3 => Prototype.Of(func(arg1, arg2, arg3)))));

        // [GHC.Base] liftM4 f m1 m2 m3 m4 = do { x1 <- m1; x2 <- m2; x3 <- m3; x4 <- m4; return (f x1 x2 x3 x4) }
        public Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<T4>, Prototype<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> func)
            => (m1, m2, m3, m4) => m1.Bind(
                arg1 => m2.Bind(
                    arg2 => m3.Bind(
                        arg3 => m4.Bind(
                            arg4 => Prototype.Of(func(arg1, arg2, arg3, arg4))))));

        // [GHC.Base] liftM5 f m1 m2 m3 m4 m5 = do { x1 <- m1; x2 <- m2; x3 <- m3; x4 <- m4; x5 <- m5; return (f x1 x2 x3 x4 x5) }
        public Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<T4>, Prototype<T5>, Prototype<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func)
            => (m1, m2, m3, m4, m5) => m1.Bind(
                arg1 => m2.Bind(
                    arg2 => m3.Bind(
                        arg3 => m4.Bind(
                            arg4 => m5.Bind(
                                arg5 => Prototype.Of(func(arg1, arg2, arg3, arg4, arg5)))))));

        #endregion

        // [Control.Monad] when p s = if p then pure () else s
        public Prototype<Unit> Unless(bool predicate, Prototype<Unit> value)
            => predicate ? Prototype.Of(Unit.Single) : value;

        // [GHC.Base] when p s = if p then s else pure ()
        public Prototype<Unit> When(bool predicate, Prototype<Unit> value)
            => predicate ? value : Prototype.Of(Unit.Single);
    }

    // Implements the IAlternativeOperators interface.
    public partial class Prototype : IAlternativeOperators
    {
        // some v = (:) <$> v <*> many v
        // GHC.Base:
        // some v = some_v
        //   where
        //     many_v = some_v <|> pure []
        //     some_v = (fmap(:) v) <*> many_v
        public Prototype<IEnumerable<T>> Some<T>(Prototype<T> value)
        {
            Func<T, Func<IEnumerable<T>, IEnumerable<T>>> append
                = _ => seq => Qperators.Append(seq, _);

            return Many(value).Gather(value.Select(append));
        }

        // many v = some v <|> pure []
        // GHC.Base:
        // many v = many_v
        //   where
        //     many_v = some_v <|> pure []
        //     some_v = (fmap(:) v) <*> many_v
        public Prototype<IEnumerable<T>> Many<T>(Prototype<T> value)
            => Some(value).Append(Prototype.Of(Enumerable.Empty<T>()));

        // optional v = Just <$> v <|> pure Nothing
        public Prototype<Maybe<T>> Optional<T>(Prototype<T> value)
            => value.Select(Maybe.Of).Append(Prototype.Of(Maybe<T>.None));
    }
}
