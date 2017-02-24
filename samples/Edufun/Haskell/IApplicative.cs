// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;

    // [Haskell] Control.Applicative
    // In-between a Functor and a Monad.
    //
    // Rules
    // -----
    // - Applicative::liftA = Functor::fmap
    //
    // API
    // ---
    // - pure       Applicative.Of          (required)
    // - <*>        obj.Gather              (required)
    // - *>         obj.ReplaceBy
    // - <*         obj.Ignore
    //
    // Utility functions:
    // - <$>        Operators.InvokeWith        <- Functor::<$>
    // - <$         obj.ReplaceBy               <- Functor::<$
    // - <**>       Operators.Apply
    // - liftA      obj.Select                  <- Functor::fmap (required)
    // - liftA2     obj.Zip
    // - liftA3     obj.Zip
    //
    // Inherited:
    // - void       obj.Skip                    <- Functor::void

    public interface IApplicative<T>
    {
        // [Haskell] (<*>) :: f (a -> b) -> f a -> f b
        // Sequence computations and combine their results.
        Prototype<TResult> Gather<TResult>(Prototype<Func<T, TResult>> applicative);

        // [Haskell] pure :: a -> f a
        // Embed pure expressions, ie lift a value.
        Prototype<TSource> Of_<TSource>(TSource value);
    }

    public interface IApplicativeSyntax<T>
    {
        // [Haskell] (<*) :: f a -> f b -> f a
        // Sequence actions, discarding the value of the second argument.
        Prototype<T> Ignore<TOther>(Prototype<TOther> other);

        // [Haskell] (<$) :: Functor f => a -> f b -> f a
        // Replace all locations in the input with the same value.
        Prototype<TResult> ReplaceBy<TResult>(TResult other);

        // [Haskell] (*>) :: f a -> f b -> f b
        // Sequence actions, discarding the value of the first argument.
        Prototype<TResult> ReplaceBy<TResult>(Prototype<TResult> other);

        // [Haskell] liftA :: Applicative f => (a -> b) -> f a -> f b
        // Lift a function to actions. A synonym of fmap for a functor.
        Prototype<TResult> Select<TResult>(Func<T, TResult> selector);

        // [Haskell] liftA2 :: Applicative f => (a -> b -> c) -> f a -> f b -> f c
        // Lift a binary function to actions.
        Prototype<TResult> Zip<TSecond, TResult>(
            Prototype<TSecond> second,
            Func<T, TSecond, TResult> zipper);

        // [Haskell] liftA3 :: Applicative f => (a -> b -> c -> d) -> f a -> f b -> f c -> f d
        // Lift a ternary function to actions.
        Prototype<TResult> Zip<T2, T3, TResult>(
            Prototype<T2> second,
            Prototype<T3> third,
            Func<T, T2, T3, TResult> zipper);
    }

    public interface IApplicativeOperators
    {
        // [Haskell] (<**>) :: Applicative f => f a -> f (a -> b) -> f b
        // A variant of <*> with the arguments reversed.
        Prototype<TResult> Apply<TSource, TResult>(
            Prototype<Func<TSource, TResult>> applicative,
            Prototype<TSource> value);

        // [Haskell] (<$>) :: Functor f => (a -> b) -> f a -> f b
        // An infix synonym for fmap.
        Prototype<TResult> InvokeWith<TSource, TResult>(
            Func<TSource, TResult> func,
            Prototype<TSource> value);
    }
}
