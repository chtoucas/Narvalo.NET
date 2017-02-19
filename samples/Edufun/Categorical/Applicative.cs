// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Edufun.Categorical.Language;

    // [Haskell] Control.Applicative
    // In-between a Functor and a Monad.
    //
    // Translation map from Haskell to .NET:
    // - pure       Applicative<T>.Pure     (required)
    //              Applicative.Of
    // - <*>        obj.Gather              (required)
    // - *>         obj.ReplaceBy
    // - <*         obj.Ignore
    //
    // Utility functions:
    // - <$>        Applicative.InvokeWith              <- Functor::<$>
    // - <$         obj.Replace                         <- Functor::<$
    // - <**>       Applicative.Apply
    // - liftA      obj.Select                          <- Functor::fmap
    // - liftA2     obj.Zip
    // - liftA3     obj.Zip

    public interface IApplicative<T>
    {
        // [Haskell] (<*>) :: f (a -> b) -> f a -> f b
        // Sequence computations and combine their results.
        Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative);
    }

    public interface IApplicative
    {
        // [Haskell] pure :: a -> f a
        // Embed pure expressions, ie lift a value.
        Applicative<T> Pure<T>(T value);
    }

    public interface IApplicativeGrammar<T>
    {
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

    public interface IApplicativeGrammar
    {
        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        Applicative<TResult> InvokeWith<T, TResult>(Func<T, TResult> selector, Applicative<T> value);

        // [Haskell] (<**>) :: Applicative f => f a -> f (a -> b) -> f b
        // A variant of <*> with the arguments reversed.
        Applicative<TResult> Apply<T, TResult>(
            Applicative<Func<T, TResult>> @this,
            Applicative<T> value);
    }
}
