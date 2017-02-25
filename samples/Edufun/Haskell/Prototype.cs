// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define APPLICATIVE_USE_GHC_BASE
//#define MONAD_VIA_MAP_MULTIPLY
//#define STRICT_HASKELL

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Fx.Linq;

    public partial class Prototype<T>
        : IFunctor<T>, IApplicative<T>, IMonad<T>, IAlternative<T>, IMonadPlus<T>
    {
        #region IFunctor<T>

        // Data.Functor: fmap
        Prototype<TResult> IFunctor<T>.Select<TResult>(Func<T, TResult> selector)
        {
            throw new PrototypeException();
        }

        #endregion

        #region IApplicative<T>

        // Control.Applicative: <*>
        Prototype<TResult> IApplicative<T>.Gather<TResult>(Prototype<Func<T, TResult>> applicative)
        {
            throw new PrototypeException();
        }

        // Control.Applicative: pure
        Prototype<TSource> IApplicative<T>.Of_<TSource>(TSource value) => Prototype.Of(value);

        #endregion

        #region IMonad<T>

        // Control.Monad: >>=
        public Prototype<TResult> Bind<TResult>(Func<T, Prototype<TResult>> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Prototype.Flatten(Select(selector));
#else
            throw new PrototypeException();
#endif
        }

        // Control.Monad: fmap f xs = xs >>= return . f
        public Prototype<TResult> Select<TResult>(Func<T, TResult> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new PrototypeException();
#else
            return Bind(_ => Prototype.Of(selector(_)));
#endif
        }

        // Control.Monad: return
        public Prototype<TSource> Of_<TSource>(TSource value) => Prototype.Of(value);

        // Control.Monad: join
        public Prototype<TSource> Flatten_<TSource>(Prototype<Prototype<TSource>> square)
            => Prototype.Flatten(square);

        #endregion

        #region IAlternative<T>

        // GHC.Base: empty = Nothing
        public Prototype<T> Empty_() => Prototype.Empty<T>();

        // GHC.Base:
        // Nothing <|> r = r
        // l <|> _ = l
        public Prototype<T> Append(Prototype<T> value)
        {
            throw new PrototypeException();
        }

        #endregion

        #region IMonadPlus<T>

        // [GHC.Base] mplus = (<|>)
        public Prototype<T> Plus(Prototype<T> value) => Append(value);

        // [GHC.Base] mzero = empty
        public Prototype<T> Zero_() => Prototype.Zero<T>();

        #endregion
    }

    public partial class Prototype<T> : IFunctorSyntax<T>, IApplicativeSyntax<T>, IMonadSyntax<T>
    {
        #region IFunctorSyntax<T>

        // [GHC.Base] (<$) = fmap . const
        Prototype<TResult> IFunctorSyntax<T>.ReplaceBy<TResult>(TResult other) => Select(_ => other);

        // [Data.Functor] void x = () <$ x
        Prototype<Unit> IFunctorSyntax<T>.Skip() => ReplaceBy(Unit.Single);

        #endregion

        #region IApplicativeSyntax<T>

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
        public Prototype<TResult> ReplaceBy<TResult>(TResult other) => Select(_ => other);

        // [GHC.Base] a1 *> a2 = (id <$ a1) <*> a2
        // Control.Applicative: u *> v = pure (const id) <*> u <*> v
        Prototype<TResult> IApplicativeSyntax<T>.ReplaceBy<TResult>(Prototype<TResult> other)
        {
#if APPLICATIVE_USE_GHC_BASE
            Func<TResult, TResult> id = _ => _;

            return other.Gather(ReplaceBy(id));
#else
            Func<T, Func<TResult, TResult>> id = _ => r => r;

            return other.Gather(Gather(Prototype.Of(id)));
#endif
        }

        // [GHC.Base] liftA f a = pure f <*> a
        // Control.Applicative: fmap f x = pure f <*> x
        Prototype<TResult> IApplicativeSyntax<T>.Select<TResult>(Func<T, TResult> selector)
           => Gather(Prototype.Of(selector));

        // [GHC.Base] liftA2 f a b = fmap f a <*> b
        public Prototype<TResult> Zip<TSecond, TResult>(
            Prototype<TSecond> second,
            Func<T, TSecond, TResult> zipper)
        {
#if STRICT_HASKELL
            Func<T, Func<TSecond, TResult>> selector
                = arg1 => arg2 => zipper(arg1, arg2);

            return second.Gather(Select(selector));
#else
            return Bind(
                arg1 => second.Select(
                    arg2 => zipper(arg1, arg2)));
#endif
        }

        // [GHC.Base] liftA3 f a b c = fmap f a <*> b <*> c
        public Prototype<TResult> Zip<T2, T3, TResult>(
            Prototype<T2> second,
            Prototype<T3> third,
            Func<T, T2, T3, TResult> zipper)
        {
#if STRICT_HASKELL
            Func<T, Func<T2, Func<T3, TResult>>> selector
                = arg1 => arg2 => arg3 => zipper(arg1, arg2, arg3);

            return third.Gather(second.Gather(Select(selector)));
#else
            // NB: We could also use the previous Zip.
            // > Bind(
            // >     arg1 => second.Zip(
            // >         third, (arg2, arg3) => zipper(arg1, arg2, arg3)));

            return Bind(
                arg1 => second.Bind(
                    arg2 => third.Select(
                        arg3 => zipper(arg1, arg2, arg3))));
#endif
        }

        #endregion

        #region IMonadSyntax<T>

        // [GHC.Base] ap m1 m2 = do { x1 <- m1; x2 <- m2; return (x1 x2) }
        // Control.Monad: <*> = ap
        public Prototype<TResult> Gather<TResult>(Prototype<Func<T, TResult>> applicative)
        {
            // In Haskell, m1 is of type m (a -> b) and m2 of type m a.
            // WARNING: return is not the "return" of C# but Applicative::pure.
#if STRICT_HASKELL
            return applicative.Bind(func => Bind(_ => Prototype.Of(func(_))));
#else
            return applicative.Bind(func => Select(func));
#endif
        }

        // [Control.Monad]
        //  replicateM cnt0 f =
        //    loop cnt0
        //  where
        //    loop cnt
        //      | cnt <= 0  = pure[]
        //      | otherwise = liftA2(:) f(loop (cnt - 1))
        public Prototype<IEnumerable<T>> Repeat(int count) => Select(_ => Enumerable.Repeat(_, count));

        // [Data.Functor] void x = () <$ x
        public Prototype<Unit> Skip() => ReplaceBy(Unit.Single);

        // [GHC.Base] m >> k = m >>= \_ -> k
        public Prototype<TResult> Then<TResult>(Prototype<TResult> other) => Bind(_ => other);

        #endregion

        #region IMonadPlusSyntax<T>

        // [Control.Monad]
        // mfilter p ma = do
        //   a <- ma
        //   if p a then return a else mzero
        public Prototype<T> Where(Func<T, bool> predicate)
            => Bind(_ => predicate(_) ? Prototype.Of(_) : Prototype.Zero<T>());

        #endregion
    }

    // Purely static methods.
    public partial class Prototype
    {
        public static Prototype<T> Of<T>(T value) { throw new PrototypeException(); }

        public static Prototype<T> Empty<T>() { throw new PrototypeException(); }

        public static Prototype<T> Zero<T>() => Empty<T>();

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

    public partial class Prototype
        : IFunctorOperators, IApplicativeOperators, IMonadOperators, IAlternativeOperators, IMonadPlusOperators
    {
        #region IFunctorOperators

        // [Data.Functor] (<$>) = fmap
        Prototype<TResult> IFunctorOperators.InvokeWith<TSource, TResult>(
            Func<TSource, TResult> func,
            Prototype<TSource> functor)
            => functor.Select(func);

        #endregion

        #region IApplicativeOperators

        // [GHC.Base] (<**>) = liftA2 (flip ($))
        public Prototype<TResult> Apply<TSource, TResult>(
            Prototype<Func<TSource, TResult>> applicative,
            Prototype<TSource> value)
            => value.Gather(applicative);

        #endregion

        #region IMonadOperators

        #region Collect

        // [Data.Traversable] sequence = sequenceA
        public Prototype<IEnumerable<TSource>> Collect<TSource>(IEnumerable<Prototype<TSource>> source)
        {
#if STRICT_HASKELL
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
#else
            // This version does not use Append() which is quite expensive.
            return Prototype.Of(CollectIterator(source));
#endif
        }

        private static IEnumerable<TSource> CollectIterator<TSource>(IEnumerable<Prototype<TSource>> source)
        {
            var item = default(TSource);

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var append = false;

                    iter.Current.Bind(
                        val =>
                        {
                            append = true;
                            item = val;

                            return Prototype.Of(Unit.Single);
                        });

                    if (append) { yield return item; }
                }
            }
        }

        #endregion

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
            next = source.Then(next);
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
                = f => next => f(value.Then(next));

            return YCombinator.Fix(g);
        }

        #endregion

        // [Control.Monad]
        // f <$!> m = do
        //   x <- m
        //   let z = f x
        //   z `seq` return z
        public Prototype<TResult> InvokeWith<TSource, TResult>(
            Func<TSource, TResult> selector,
            Prototype<TSource> value)
            => value.Select(selector);

        #region Lift

        // [GHC.Base] liftM f m1 = do { x1 <- m1; return (f x1) }
        public Func<Prototype<TSource>, Prototype<TResult>> Lift<TSource, TResult>(Func<TSource, TResult> func)
        {
#if STRICT_HASKELL
            return m => m.Bind(arg => Prototype.Of(func(arg)));
#else
            return m => m.Select(func);
#endif
        }

        // [GHC.Base] liftM2 f m1 m2 = do { x1 <- m1; x2 <- m2; return (f x1 x2) }
        public Func<Prototype<T1>, Prototype<T2>, Prototype<TResult>> Lift<T1, T2, TResult>(Func<T1, T2, TResult> func)
        {
#if STRICT_HASKELL
            return (m1, m2) => m1.Bind(
                arg1 => m2.Bind(
                    arg2 => Prototype.Of(func(arg1, arg2))));
#else
            return (arg1, arg2) => arg1.Zip(arg2, func);
#endif
        }

        // [GHC.Base] liftM3 f m1 m2 m3 = do { x1 <- m1; x2 <- m2; x3 <- m3; return (f x1 x2 x3) }
        public Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<TResult>> Lift<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> func)
        {
#if STRICT_HASKELL
            return (m1, m2, m3) => m1.Bind(
                arg1 => m2.Bind(
                    arg2 => m3.Bind(
                        arg3 => Prototype.Of(func(arg1, arg2, arg3)))));
#else
            return (arg1, arg2, arg3) => arg1.Zip(arg2, arg3, func);
#endif
        }

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

        #endregion

        #region IAlternativeOperators

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

        #endregion

        #region IMonadPlusOperators

        // [Control.Monad]
        //  guard True = pure()
        //  guard False = empty
        public Prototype<Unit> Guard(bool predicate)
           // NB: Using IAlternative<T>:
           // > predicate ? Prototype.Of(Unit.Single) : Prototype.Empty<Unit>();
           => predicate ? Prototype.Of(Unit.Single) : Prototype.Zero<Unit>();

        // [Data.Foldable] msum = asum
        // asum :: (Foldable t, Alternative f) => t (f a) -> f a
        // asum = foldr (<|>) empty
        public Prototype<TSource> Sum<TSource>(IEnumerable<Prototype<TSource>> source)
            // NB: Using IAlternative<T>:
            // > source.Aggregate(Prototype.Empty<TSource>(), (m, n) => m.Append(n));
            => source.Aggregate(Prototype.Zero<TSource>(), (m, n) => m.Plus(n));

        #endregion
    }
}
