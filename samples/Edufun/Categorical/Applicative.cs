// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Fx.Linq;

    // [Haskell] Control.Applicative
    // In-between a Functor and a Monad.
    //
    // Translation map from Haskell to .NET.
    // - pure       Applicative<T>.Pure     (required)
    //              Applicative.Of
    // - <*>        obj.Gather              (required)
    // - *>         obj.ReplaceBy
    // - <*         obj.Ignore_
    // Alternative:
    // - empty      Applicative<T>.Empty    (required)
    //              Applicative.Empty       (required)
    // - <|>        obj.Append              (required)
    // - some
    // - many
    // Utility functions:
    // - <$>        Applicative.InvokeWith  <- Functor
    // - <$         obj.Replace             <- Functor
    // - <**>       Applicative.Apply
    // - liftA      obj.Select              <- Functor::fmap
    // - liftA2     obj.Zip
    // - liftA3     obj.Zip
    // - optional   obj.Optional

    public partial interface IApplicative<T>
    {
        // [Haskell] pure :: a -> f a
        // Embed pure expressions, ie lift a value.
        Applicative<TSource> Pure<TSource>(TSource value);

        // [Haskell] (<*>) :: f (a -> b) -> f a -> f b
        // Sequence computations and combine their results.
        Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative);

        // [Haskell] (*>) :: f a -> f b -> f b
        // Sequence actions, discarding the value of the first argument.
        Applicative<TResult> ReplaceBy<TResult>(Applicative<TResult> other);

        // [Haskell] empty :: f a
        // The identity of <|>.
        Applicative<TSource> Empty<TSource>();

        // [Haskell] (<|>) :: f a -> f a -> f a
        // An associative binary operation.
        Applicative<T> Append(Applicative<T> other);

        // [Haskell] some :: f a -> f [a]
        // One or more.
        Applicative<IEnumerable<T>> Some();

        // [Haskell] many :: f a -> f [a]
        // Zero or more.
        Applicative<IEnumerable<T>> Many();

        // [Haskell] (<*) :: f a -> f b -> f a
        // Sequence actions, discarding the value of the second argument.
        Applicative<T> Ignore_<TResult>(Applicative<TResult> other);

        // [Haskell] (<$) :: Functor f => a -> f b -> f a
        // Replace all locations in the input with the same value.
        Applicative<TResult> Replace<TResult>(TResult other);

        // [Haskell] liftA :: Applicative f => (a -> b) -> f a -> f b
        // Lift a function to actions. A synonym of fmap for a functor.
        Applicative<TResult> Select<TResult>(Func<T, TResult> selector);

        // [Haskell] liftA2 :: Applicative f => (a -> b -> c) -> f a -> f b -> f c
        // Lift a binary function to actions.
        Applicative<TResult> Zip<TSecond, TResult>(
            Applicative<TSecond> second,
            Func<T, TSecond, TResult> resultSelector);

        // [Haskell] liftA3 :: Applicative f => (a -> b -> c -> d) -> f a -> f b -> f c -> f d
        // Lift a ternary function to actions.
        Applicative<TResult> Zip<T2, T3, TResult>(
            Applicative<T2> second,
            Applicative<T3> third,
            Func<T, T2, T3, TResult> resultSelector);

        // [Haskell] optional :: Alternative f => f a -> f (Maybe a)
        Applicative<Maybe<T>> Optional();
    }

    public interface IApplicative
    {
        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        Applicative<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> thunk, Applicative<TSource> value);

        // [Haskell] (<**>) :: Applicative f => f a -> f (a -> b) -> f b
        // A variant of <*> with the arguments reversed.
        Applicative<TResult> Apply<TSource, TResult>(
            Applicative<Func<TSource, TResult>> @this,
            Applicative<TSource> value);
    }

    public partial class Applicative<T> : IApplicative<T>
    {
        public Applicative<TSource> Pure<TSource>(TSource value) => Applicative.Of(value);

        public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative)
        {
            throw new FakeClassException();
        }

        // u *> v = pure (const id) <*> u <*> v
        // GHC.Base: a1 *> a2 = (id <$ a1) <*> a2
        public Applicative<TResult> ReplaceBy<TResult>(Applicative<TResult> other)
        {
            Func<TResult, TResult> id = _ => _;
            Func<T, Func<TResult, TResult>> value = _ => id;

            var applicative = Applicative.Of(value);
            Applicative<Func<TResult, TResult>> second = this.Gather(applicative);

            return other.Gather(second);
        }

        // GHC.Base: empty = Nothing
        public Applicative<TSource> Empty<TSource>() { throw new FakeClassException(); }

        // GHC.Base:
        // Nothing <|> r = r
        // l <|> _ = l
        public Applicative<T> Append(Applicative<T> other)
        {
            throw new FakeClassException();
        }

        // some v = (:) <$> v <*> many v
        // GHC.Base:
        // some v = some_v
        //   where
        //     many_v = some_v <|> pure []
        //     some_v = (fmap(:) v) <*> many_v
        public Applicative<IEnumerable<T>> Some()
        {
            Func<T, Func<IEnumerable<T>, IEnumerable<T>>> append = _ => seq => Qperators.Append(seq, _);

            return Many().Gather(Select(append));
        }

        // many v = some v <|> pure []
        // GHC.Base:
        // many v = many_v
        //   where
        //     many_v = some_v <|> pure []
        //     some_v = (fmap(:) v) <*> many_v
        public Applicative<IEnumerable<T>> Many()
            => Some().Append(Applicative.Of(Enumerable.Empty<T>()));

        // u <* v = pure const <*> u <*> v
        // GHC.Base: (<*) = liftA2 const
        public Applicative<T> Ignore_<TResult>(Applicative<TResult> other)
        {
            //Func<TResult, Applicative<T>> me = _ => this;
            //Func<T, Func<TResult, Applicative<T>>> always1 = _ => me;

            //var applicative = Applicative.Of(always1);
            //Applicative<Func<TResult, Applicative<T>>> second = this.Gather(applicative);
            //Applicative<Applicative<T>> third = other.Gather(second);

            throw new NotImplementedException();
        }

        // GHC.Base: (<$) = fmap . const
        public Applicative<TResult> Replace<TResult>(TResult other) => Select(_ => other);

        // GHC.Base: liftA f a = pure f <*> a
        // fmap f x = pure f <*> x
        public Applicative<TResult> Select<TResult>(Func<T, TResult> selector)
            => Gather(Applicative.Of(selector));

        // GHC.Base: liftA2 f a b = fmap f a <*> b
        public Applicative<TResult> Zip<TSecond, TResult>(
            Applicative<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Func<T, Func<TSecond, TResult>> selector
                = (T x) => (TSecond y) => resultSelector.Invoke(x, y);

            return second.Gather(Select(selector));
        }

        // GHC.Base: liftA3 f a b c = fmap f a <*> b <*> c
        public Applicative<TResult> Zip<T2, T3, TResult>(
            Applicative<T2> second,
            Applicative<T3> third,
            Func<T, T2, T3, TResult> resultSelector)
        {
            Func<T, Func<T2, Func<T3, TResult>>> selector
                = (T x) => (T2 y) => (T3 z) => resultSelector.Invoke(x, y, z);

            Applicative<Func<T3, TResult>> app = second.Gather(Select(selector));

            return third.Gather(app);
        }

        // optional v = Just <$> v <|> pure Nothing
        public Applicative<Maybe<T>> Optional()
            => Select(Maybe.Of).Append(Applicative.Of(Maybe<T>.None));
    }

    public class Applicative : IApplicative
    {
        public static Applicative<TSource> Empty<TSource>() { throw new FakeClassException(); }

        public static Applicative<TSource> Of<TSource>(TSource value) { throw new FakeClassException(); }

        // Data.Functor: (<$>) = fmap
        public Applicative<TResult> InvokeWith<TSource, TResult>(
            Func<TSource, TResult> thunk,
            Applicative<TSource> value)
            => value.Select(thunk);

        // GHC.Base: (<**>) = liftA2 (flip ($))
        public Applicative<TResult> Apply<TSource, TResult>(
            Applicative<Func<TSource, TResult>> @this,
            Applicative<TSource> value)
            => value.Gather(@this);
    }

    // If an applicative functor is also a monad, it should satisfy:
    // - pure = return
    // - (<*>) = ap
    public static class ApplicativeRules
    {
        // pure id <*> v = v
        public static bool Identity<X>(Applicative<X> me)
        {
            Func<X, X> id = _ => _;

            return me.Gather(Applicative.Of(id)) == me;
        }

        // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
        public static bool Composition()
        {
            return true;
        }

        // pure f <*> pure x = pure (f x)
        public static bool Homomorphism<X, Y>(Func<X, Y> f, X value)
        {
            return Applicative.Of(value).Gather(Applicative.Of(f))
                == Applicative.Of(f(value));
        }

        // u <*> pure y = pure ($ y) <*> u
        public static bool Interchange()
        {
            throw new NotImplementedException();
        }
    }
}
