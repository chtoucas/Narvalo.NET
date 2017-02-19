// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    // [Haskell] Control.Applicative
    // In-between a Functor and a Monad.
    //
    // Translation map from Haskell to .NET:
    // - pure       Applicative.Pure        (required)
    //              Applicative.Of
    // - <*>        obj.Gather              (required)
    // - *>         obj.ReplaceBy
    // - <*         obj.Ignore
    //
    // Utility functions:
    // - <$>        Applicative.InvokeWith  <- Functor::<$>
    // - <$         obj.Replace             <- Functor::<$
    // - <**>       Applicative.Apply
    // - liftA      obj.Select              <- Functor::fmap
    // - liftA2     obj.Zip
    // - liftA3     obj.Zip

    public interface IApplicative<T>
    {
        // [Haskell] (<*>) :: f (a -> b) -> f a -> f b
        // Sequence computations and combine their results.
        Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative);

        // [Haskell] (*>) :: f a -> f b -> f b
        // Sequence actions, discarding the value of the first argument.
        Applicative<TResult> ReplaceBy<TResult>(Applicative<TResult> other);

        // [Haskell] (<*) :: f a -> f b -> f a
        // Sequence actions, discarding the value of the second argument.
        Applicative<T> Ignore<TResult>(Applicative<TResult> other);

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
    }

    public interface IApplicative
    {
        // [Haskell] pure :: a -> f a
        // Embed pure expressions, ie lift a value.
        Applicative<T> Pure<T>(T value);

        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        Applicative<TResult> InvokeWith<T, TResult>(Func<T, TResult> selector, Applicative<T> value);

        // [Haskell] (<**>) :: Applicative f => f a -> f (a -> b) -> f b
        // A variant of <*> with the arguments reversed.
        Applicative<TResult> Apply<T, TResult>(
            Applicative<Func<T, TResult>> @this,
            Applicative<T> value);
    }

    public partial class Applicative<T> : IApplicative<T>
    {
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

        // u <* v = pure const <*> u <*> v
        // GHC.Base: (<*) = liftA2 const
        public Applicative<T> Ignore<TResult>(Applicative<TResult> other)
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
    }

    public class Applicative : IApplicative
    {
        public static Applicative<T> Of<T>(T value) { throw new FakeClassException(); }

        public Applicative<T> Pure<T>(T value) => Applicative.Of(value);

        // Data.Functor: (<$>) = fmap
        public Applicative<TResult> InvokeWith<T, TResult>(
            Func<T, TResult> selector,
            Applicative<T> value)
            => value.Select(selector);

        // GHC.Base: (<**>) = liftA2 (flip ($))
        public Applicative<TResult> Apply<T, TResult>(
            Applicative<Func<T, TResult>> @this,
            Applicative<T> value)
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
