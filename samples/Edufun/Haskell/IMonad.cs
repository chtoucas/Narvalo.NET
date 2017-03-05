// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    // [Haskell] Control.Monad
    //
    // Rules
    // -----
    // > Monad::return = Applicative::pure
    // > Monad::ap     = Applicative::<*>
    // which implies:
    // > fmap f xs     =  xs >>= return . f
    // > Monad::>>     = Applicative::*>
    // in addition we have:
    // > Monad::fmap   = Functor::fmap = Applicative::liftA
    //
    // API
    // ---
    // - >>=            obj.Bind            (required)
    // - >>             obj.ContinueWith        <- Applicative::*>
    // - return         Monad.Of            (required) <- Applicative::pure
    // - fail
    // - fmap           obj.Select          (alt-required) <- Functor::fmap (required), Applicative::liftA
    //
    // Basic Monad functions:
    // - mapM           Query.SelectWith
    // - mapM_
    // - forM           Kleisli.InvokeWith
    // - forM_
    // - sequence       Operators.Collect
    // - sequence_
    // - (=<<)          Kleisli.InvokeWith
    // - (>=>)          Kleisli.Compose
    // - (<=<)          Kleisli.ComposeBack
    // - forever        Operators.Forever
    // - void           obj.Skip                <- Functor::void
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
    // - ap             obj.Gather              <- Applicative::<*> (required)
    //
    // Strict monadic functions:
    // - (<$!>)         Operators.InvokeWith    <- Applicative::<$>
    //
    // Inherited:
    // - <$             obj.ReplaceBy           <- Functor::<$
    // - <*             obj.PassThrough         <- Applicative::<*
    // - <**>           Operators.Apply         <- Applicative::<**>
    // - liftA2         obj.Zip                 <- Applicative::liftA2
    // - liftA3         obj.Zip                 <- Applicative::liftA3

    public interface IMonad<T>
    {
        // [Haskell] (>>=) :: forall a b. m a -> (a -> m b) -> m b
        // Sequentially compose two actions, passing any value produced by the first
        // as an argument to the second.
        Prototype<TResult> Bind<TResult>(Func<T, Prototype<TResult>> selector);

        // [Haskell] fmap :: (a -> b) -> f a -> f b
        Prototype<TResult> Select<TResult>(Func<T, TResult> selector);

        // [Haskell] join :: Monad m => m (m a) -> m a
        // The join function is the conventional monad join operator. It is used to remove
        // one level of monadic structure, projecting its bound argument into the outer level.
        Prototype<TSource> Flatten_<TSource>(Prototype<Prototype<TSource>> square);

        // [Haskell] return :: a -> m a
        // Inject a value into the monadic type.
        Prototype<TSource> Of_<TSource>(TSource value);
    }

    public interface IMonadSyntax<T>
    {
        // [Haskell] ap :: Monad m => m (a -> b) -> m a -> m b
        // In many situations, the liftM operations can be replaced by uses of ap,
        // which promotes function application.
        Prototype<TResult> Gather<TResult>(Prototype<Func<T, TResult>> applicative);

        // [Haskell] replicateM :: Applicative m => Int -> m a -> m [a]
        // replicateM n act performs the action n times, gathering the results.
        Prototype<IEnumerable<T>> Repeat(int count);

        // [Haskell] void :: Functor f => f a -> f ()
        // void value discards or ignores the result of evaluation.
        Prototype<Unit> Skip();

        // [Haskell] (>>) :: forall a b. m a -> m b -> m b
        // Sequentially compose two actions, discarding any value produced by the first,
        // like sequencing operators (such as the semicolon) in imperative languages.
        Prototype<TResult> ContinueWith<TResult>(Prototype<TResult> other);
    }

    public interface IKleisliOperators
    {
        // [Haskell] (>=>) :: Monad m => (a -> m b) -> (b -> m c) -> a -> m c
        // Left-to-right Kleisli composition of monads.
        Func<TSource, Prototype<TResult>> Compose<TSource, TMiddle, TResult>(
            Func<TSource, Prototype<TMiddle>> first,
            Func<TMiddle, Prototype<TResult>> second);

        // [Haskell] (<=<) :: Monad m => (b -> m c) -> (a -> m b) -> a -> m c
        // Right-to-left Kleisli composition of monads. (>=>), with the arguments flipped.
        Func<TSource, Prototype<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            Func<TMiddle, Prototype<TResult>> first,
            Func<TSource, Prototype<TMiddle>> second);

        // [Haskell] forM :: (Traversable t, Monad m) => t a -> (a -> m b) -> m (t b)
        // forM is mapM with its arguments flipped.
        Prototype<IEnumerable<TResult>> InvokeWith<TSource, TResult>(
            Func<TSource, Prototype<TResult>> func,
            IEnumerable<TSource> seq);

        // [Haskell] (=<<) :: Monad m => (a -> m b) -> m a -> m b
        // Same as >>=, but with the arguments interchanged.
        Prototype<TResult> InvokeWith<TSource, TResult>(
            Func<TSource, Prototype<TResult>> func,
            Prototype<TSource> value);
    }

    public interface IQueryOperators
    {
        // [Haskell] foldM :: (Foldable t, Monad m) => (b -> a -> m b) -> b -> t a -> m b
        // The foldM function is analogous to foldl, except that its result is encapsulated
        // in a monad. Note that foldM works from left-to-right over the list arguments.
        Prototype<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Prototype<TAccumulate>> accumulator);

        // [Haskell] mapAndUnzipM :: Applicative m => (a -> m (b, c)) -> [a] -> m ([b], [c])
        // The mapAndUnzipM function maps its first argument over a list, returning the result
        // as a pair of lists. This function is mainly used with complicated data structures
        // or a state-transforming monad.
        Prototype<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            SelectUnzip<TSource, TFirst, TSecond>(
            IEnumerable<TSource> source,
            Func<TSource, Prototype<Tuple<TFirst, TSecond>>> selector);

        // [Haskell] mapM :: (Traversable t, Monad m) => (a -> m b) -> t a -> m (t b)
        // Map each element of a structure to a monadic action, evaluate these actions
        // from left to right, and collect the results.
        Prototype<IEnumerable<TResult>> SelectWith<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Prototype<TResult>> selector);

        // [Haskell] filterM :: Applicative m => (a -> m Bool) -> [a] -> m [a]
        // This generalizes the list-based filter function.
        Prototype<IEnumerable<TSource>> WhereBy<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Prototype<bool>> predicate);

        // [Haskell] zipWithM :: Applicative m => (a -> b -> m c) -> [a] -> [b] -> m [c]
        // The zipWithM function generalizes zipWith to arbitrary applicative functors.
        Prototype<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Prototype<TResult>> resultSelector);
    }

    public interface IMonadOperators
    {
        // [Haskell] sequence :: (Traversable t, Monad m) => t (m a) -> m (t a)
        // Evaluate each monadic action in the structure from left to right, and collect the results.
        Prototype<IEnumerable<TSource>> Collect<TSource>(IEnumerable<Prototype<TSource>> source);

        // [Haskell] forever :: Applicative f => f a -> f b
        // forever act repeats the action infinitely.
        Prototype<TResult> Forever<TSource, TResult>(Prototype<TSource> source);

        // [Haskell] (<$!>) :: Monad m => (a -> b) -> m a -> m b
        // Strict version of <$>.
        Prototype<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Prototype<TSource> value);

        // [Haskell] liftM :: Monad m => (a1 -> r) -> m a1 -> m r
        // Promote a function to a monad.
        Func<Prototype<TSource>, Prototype<TResult>> Lift<TSource, TResult>(Func<TSource, TResult> func);

        // [Haskell] liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Prototype<T1>, Prototype<T2>, Prototype<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> func);

        // [Haskell] liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func);

        // [Haskell] liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<T4>, Prototype<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> func);

        // [Haskell] liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<T4>, Prototype<T5>, Prototype<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> func);

        // [Haskell] when :: Applicative f => Bool -> f () -> f ()
        // The reverse of when.
        Prototype<Unit> Unless(bool predicate, Prototype<Unit> value);

        // [Haskell] when :: Applicative f => Bool -> f () -> f ()
        // Conditional execution of Applicative expressions.
        Prototype<Unit> When(bool predicate, Prototype<Unit> value);
    }
}
