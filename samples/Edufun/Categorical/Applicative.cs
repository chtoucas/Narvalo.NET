// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Edufun.Categorical.Impl;

    // [Haskell] Control.Applicative
    // In-between a Functor and a Monad.
    //
    // Translation map from Haskell to .NET:
    // - pure       Applicative.Of          (required)
    // - <*>        obj.Gather              (required)
    // - *>         obj.ReplaceBy
    // - <*         obj.Ignore
    //
    // Utility functions:
    // - <$>        Operators.InvokeWith                <- Functor::<$>
    // - <$         obj.Replace                         <- Functor::<$
    // - <**>       Operators.Apply
    // - liftA      obj.Select                          <- Functor::fmap
    // - liftA2     obj.Zip
    // - liftA3     obj.Zip

    public interface IApplicative<T>
    {
        // [Haskell] (<*>) :: f (a -> b) -> f a -> f b
        // Sequence computations and combine their results.
        Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative);

        // [Haskell] pure :: a -> f a
        // Embed pure expressions, ie lift a value.
        Applicative<TSource> Of_<TSource>(TSource value);
    }

    public interface IApplicativeSyntax<T>
    {
        // [Haskell] (<*) :: f a -> f b -> f a
        // Sequence actions, discarding the value of the second argument.
        Applicative<T> Ignore<TResult>(Applicative<TResult> other);

        // [Haskell] (<$) :: Functor f => a -> f b -> f a
        // Replace all locations in the input with the same value.
        Applicative<TResult> Replace<TResult>(TResult other);

        // [Haskell] (*>) :: f a -> f b -> f b
        // Sequence actions, discarding the value of the first argument.
        Applicative<TResult> ReplaceBy<TResult>(Applicative<TResult> other);

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

    public interface IApplicativeOperators
    {
        // [Haskell] (<**>) :: Applicative f => f a -> f (a -> b) -> f b
        // A variant of <*> with the arguments reversed.
        Applicative<TResult> Apply<TSource, TResult>(
            Applicative<Func<TSource, TResult>> @this,
            Applicative<TSource> value);

        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        Applicative<TResult> InvokeWith<TSource, TResult>(
            Func<TSource, TResult> selector,
            Applicative<TSource> value);
    }
}
