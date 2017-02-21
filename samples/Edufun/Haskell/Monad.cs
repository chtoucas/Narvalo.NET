// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;

    using Edufun.Haskell.Impl;
    using Narvalo.Fx;

    // [Haskell] Control.Monad
    //
    // Translation map from Haskell to .NET:
    // - >>=            obj.Bind            (required)
    // - >>             obj.ReplaceBy                       <- Applicative::*>
    // - return         Monad.Of            (required)      <- Applicative::pure
    // - fail
    // Alternative requirement:
    // - fmap           obj.Select          (alt-required)  <- Functor::fmap, Applicative::liftA
    //
    // Basic Monad functions:
    // - mapM           Query.SelectWith
    // - mapM_
    // - forM           Kleisli.ForEach
    // - forM_
    // - sequence       Operators.Collect
    // - sequence_
    // - (=<<)          Kleisli.Invoke
    // - (>=>)          Kleisli.Compose
    // - (<=<)          Kleisli.ComposeBack
    // - forever        Operators.Forever
    // - void           obj.Skip                            <- Functor::void
    //
    // Generalisations of list functions:
    // - join           Monad.Flatten       (alt-required)
    // - filterM        Query.WhereBy
    // - mapAndUnzipM   Query.SelectUnzip
    // - zipWithM       Query.ZipWith
    // - zipWithM_
    // - foldM          Query.Fold
    // - foldM_
    // - replicateM     obj.Repeat
    // - replicateM_
    //
    // Conditional execution of monadic expressions:
    // - when           obj.When
    // - unless         obj.Unless
    //
    // Monadic lifting operators:
    // - liftM          Operators.Lift
    // - liftM2         Operators.Lift
    // - liftM3         Operators.Lift
    // - liftM4         Operators.Lift
    // - liftM5         Operators.Lift
    // - ap             obj.Gather                          <- Applicative::<*>
    //
    // Strict monadic functions:
    // - (<$!>)         Operators.InvokeWith                <- Applicative::<$>
    //
    // From Functor:
    // - <$             obj.Replace
    // - $>             Operators.Inject
    // - <$>            Operators.InvokeWith
    //
    // From Applicative:
    // - <*             obj.Ignore
    // - <**>           Operators.Apply
    // - liftA2         obj.Zip
    // - liftA3         obj.Zip

    public interface IMonad<T>
    {
        // [Haskell] (>>=) :: forall a b. m a -> (a -> m b) -> m b
        // Sequentially compose two actions, passing any value produced by the first
        // as an argument to the second.
        Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> selector);

        // [Haskell] fmap :: (a -> b) -> f a -> f b
        Monad<TResult> Select<TResult>(Func<T, TResult> selector);

        // [Haskell] join :: Monad m => m (m a) -> m a
        // The join function is the conventional monad join operator. It is used to remove
        // one level of monadic structure, projecting its bound argument into the outer level.
        Monad<TSource> Flatten_<TSource>(Monad<Monad<TSource>> square);

        // [Haskell] return :: a -> m a
        // Inject a value into the monadic type.
        Monad<TSource> Of_<TSource>(TSource value);
    }

    public interface IMonadSyntax<T>
    {
        // [Haskell] ap :: Monad m => m (a -> b) -> m a -> m b
        // In many situations, the liftM operations can be replaced by uses of ap,
        // which promotes function application.
        Monad<TResult> Gather<TResult>(Monad<Func<T, TResult>> applicative);

        // [Haskell] (>>) :: forall a b. m a -> m b -> m b
        // Sequentially compose two actions, discarding any value produced by the first,
        // like sequencing operators (such as the semicolon) in imperative languages.
        Monad<TResult> ReplaceBy<TResult>(Monad<TResult> other);

        // [Haskell] replicateM :: Applicative m => Int -> m a -> m [a]
        // replicateM n act performs the action n times, gathering the results.
        Monad<IEnumerable<T>> Repeat(int count);

        // [Haskell] void :: Functor f => f a -> f ()
        // void value discards or ignores the result of evaluation.
        Monad<Unit> Skip();
    }

    public interface IKleisliOperators
    {
        // [Haskell] (>=>) :: Monad m => (a -> m b) -> (b -> m c) -> a -> m c
        // Left-to-right Kleisli composition of monads.
        Func<TSource, Monad<TResult>> Compose<TSource, TMiddle, TResult>(
            Func<TSource, Monad<TMiddle>> first,
            Func<TMiddle, Monad<TResult>> second);

        // [Haskell] (<=<) :: Monad m => (b -> m c) -> (a -> m b) -> a -> m c
        // Right-to-left Kleisli composition of monads. (>=>), with the arguments flipped.
        Func<TSource, Monad<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            Func<TMiddle, Monad<TResult>> first,
            Func<TSource, Monad<TMiddle>> second);

        // [Haskell] forM :: (Traversable t, Monad m) => t a -> (a -> m b) -> m (t b)
        // forM is mapM with its arguments flipped.
        Monad<IEnumerable<TResult>> ForEach<TSource, TResult>(
            Func<TSource, Monad<TResult>> func,
            IEnumerable<TSource> seq);

        // [Haskell] (=<<) :: Monad m => (a -> m b) -> m a -> m b
        // Same as >>=, but with the arguments interchanged.
        Monad<TResult> Invoke<TSource, TResult>(Func<TSource, Monad<TResult>> func, Monad<TSource> value);
    }

    public interface IQueryOperators
    {
        // [Haskell] foldM :: (Foldable t, Monad m) => (b -> a -> m b) -> b -> t a -> m b
        // The foldM function is analogous to foldl, except that its result is encapsulated
        // in a monad. Note that foldM works from left-to-right over the list arguments.
        Monad<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator);

        // [Haskell] mapAndUnzipM :: Applicative m => (a -> m (b, c)) -> [a] -> m ([b], [c])
        // The mapAndUnzipM function maps its first argument over a list, returning the result
        // as a pair of lists. This function is mainly used with complicated data structures
        // or a state-transforming monad.
        Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            SelectUnzip<TSource, TFirst, TSecond>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<Tuple<TFirst, TSecond>>> selector);

        // [Haskell] mapM :: (Traversable t, Monad m) => (a -> m b) -> t a -> m (t b)
        // Map each element of a structure to a monadic action, evaluate these actions
        // from left to right, and collect the results.
        Monad<IEnumerable<TResult>> SelectWith<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<TResult>> selector);

        // [Haskell] filterM :: Applicative m => (a -> m Bool) -> [a] -> m [a]
        // This generalizes the list-based filter function.
        Monad<IEnumerable<TSource>> WhereBy<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<bool>> predicate);

        // [Haskell] zipWithM :: Applicative m => (a -> b -> m c) -> [a] -> [b] -> m [c]
        // The zipWithM function generalizes zipWith to arbitrary applicative functors.
        Monad<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelector);
    }

    public interface IMonadOperators
    {
        // [Haskell] sequence :: (Traversable t, Monad m) => t (m a) -> m (t a)
        // Evaluate each monadic action in the structure from left to right, and collect the results.
        Monad<IEnumerable<TSource>> Collect<TSource>(IEnumerable<Monad<TSource>> source);

        // [Haskell] forever :: Applicative f => f a -> f b
        // forever act repeats the action infinitely.
        Monad<TResult> Forever<TSource, TResult>(Monad<TSource> source);

        // [Haskell] (<$!>) :: Monad m => (a -> b) -> m a -> m b
        // Strict version of <$>.
        Monad<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Monad<TSource> value);

        // [Haskell] liftM :: Monad m => (a1 -> r) -> m a1 -> m r
        // Promote a function to a monad.
        Func<Monad<TSource>, Monad<TResult>> Lift<TSource, TResult>(Func<TSource, TResult> func);

        // [Haskell] liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Monad<T1>, Monad<T2>, Monad<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> func);

        // [Haskell] liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func);

        // [Haskell] liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> func);

        // [Haskell] liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> func);

        // [Haskell] when :: Applicative f => Bool -> f () -> f ()
        // The reverse of when.
        Monad<Unit> Unless(bool predicate, Monad<Unit> value);

        // [Haskell] when :: Applicative f => Bool -> f () -> f ()
        // Conditional execution of Applicative expressions.
        Monad<Unit> When(bool predicate, Monad<Unit> value);
    }
}
