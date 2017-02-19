// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define MONAD_VIA_MAP_MULTIPLY

namespace Edufun.Categorical
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Fx;

    // [Haskell] Control.Monad
    //
    // Translation map from Haskell to .NET:
    // - >>=            obj.Bind            (required)
    // - >>             obj.ReplaceBy       <- Applicative::*>
    // - return         Monad.Return        <- Applicative::pure
    //                  Monad.Of
    // - fail
    // Alternative requirement:
    // - fmap           obj.Select          (required) <- Functor::fmap, Applicative::liftA
    //
    // Basic Monad functions:
    // - mapM           Monad.SelectWith
    // - mapM_
    // - forM           Monad.ForEach
    // - forM_
    // - sequence       Monad.Collect
    // - sequence_
    // - (=<<)          Monad.Invoke
    // - (>=>)          Monad.Compose
    // - (<=<)          Monad.ComposeBack
    // - forever        obj.Forever
    // - void           obj.Skip            <- Functor::void
    //
    // Generalisations of list functions:
    // - join           Monad.Flatten
    // - filterM        Monad.WhereBy
    // - mapAndUnzipM   Monad.SelectUnzip
    // - zipWithM       Monad.ZipWith
    // - zipWithM_
    // - foldM          Monad.Fold
    // - foldM_
    // - replicateM     obj.Repeat
    // - replicateM_
    //
    // Conditional execution of monadic expressions:
    // - guard          Monad.Guard
    // - when           obj.When
    // - unless         obj.Unless
    //
    // Monadic lifting operators:
    // - liftM          Monad.Lift
    // - liftM2         Monad.Lift
    // - liftM3         Monad.Lift
    // - liftM4         Monad.Lift
    // - liftM5         Monad.Lift
    // - ap             obj.Gather          <- Applicative::<*>
    //
    // Strict monadic functions:
    // - (<$!>)         Monad.InvokeWith    <- Applicative::<$>

    public interface IMonad<T>
    {
        // [Haskell] (>>=) :: forall a b. m a -> (a -> m b) -> m b
        // Sequentially compose two actions, passing any value produced by the first
        // as an argument to the second.
        Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> selector);

        // [Haskell] (>>) :: forall a b. m a -> m b -> m b
        // Sequentially compose two actions, discarding any value produced by the first,
        // like sequencing operators (such as the semicolon) in imperative languages.
        Monad<TResult> ReplaceBy<TResult>(Monad<TResult> other);

        // [Haskell] fmap :: (a -> b) -> f a -> f b
        Monad<TResult> Select<TResult>(Func<T, TResult> selector);

        // [Haskell] forever :: Applicative f => f a -> f b
        // forever act repeats the action infinitely.
        Monad<TResult> Forever<TSource, TResult>(Func<Monad<TResult>> thunk);

        // [Haskell] void :: Functor f => f a -> f ()
        // void value discards or ignores the result of evaluation.
        Monad<Unit> Skip();

        // [Haskell] replicateM :: Applicative m => Int -> m a -> m [a]
        // replicateM n act performs the action n times, gathering the results.
        Monad<IEnumerable<T>> Repeat(int count);

        // [Haskell] when :: Applicative f => Bool -> f () -> f ()
        // Conditional execution of Applicative expressions.
        void When(Func<T, bool> predicate, Action<T> action);

        // [Haskell] when :: Applicative f => Bool -> f () -> f ()
        // The reverse of when.
        void Unless(Func<T, bool> predicate, Action<T> action);

        // [Haskell] ap :: Monad m => m (a -> b) -> m a -> m b
        // In many situations, the liftM operations can be replaced by uses of ap,
        // which promotes function application.
        Monad<TResult> Gather<TResult>(Monad<Func<T, TResult>> applicative);
    }

    public interface IMonad
    {
        // [Haskell] return :: a -> m a
        // Inject a value into the monadic type.
        Monad<T> Return<T>(T value);

        // [Haskell] mapM :: (Traversable t, Monad m) => (a -> m b) -> t a -> m (t b)
        // Map each element of a structure to a monadic action, evaluate these actions
        // from left to right, and collect the results.
        Monad<IEnumerable<TResult>> SelectWith<T, TResult>(
            IEnumerable<T> @this,
            Func<T, Monad<TResult>> selector);

        // [Haskell] forM :: (Traversable t, Monad m) => t a -> (a -> m b) -> m (t b)
        // forM is mapM with its arguments flipped.
        Monad<IEnumerable<TResult>> ForEach<T, TResult>(
            Func<T, Monad<TResult>> @this,
            IEnumerable<T> seq);

        // [Haskell] sequence :: (Traversable t, Monad m) => t (m a) -> m (t a)
        // Evaluate each monadic action in the structure from left to right, and collect the results.
        Monad<IEnumerable<T>> Collect<T>(IEnumerable<Monad<T>> @this);

        // [Haskell] (=<<) :: Monad m => (a -> m b) -> m a -> m b
        // Same as >>=, but with the arguments interchanged.
        Monad<TResult> Invoke<T, TResult>(Func<T, Monad<TResult>> @this, Monad<T> value);

        // [Haskell] (>=>) :: Monad m => (a -> m b) -> (b -> m c) -> a -> m c
        // Left-to-right Kleisli composition of monads.
        Func<T, Monad<TResult>> Compose<T, TMiddle, TResult>(
            Func<T, Monad<TMiddle>> @this,
            Func<TMiddle, Monad<TResult>> thunk);

        // [Haskell] (<=<) :: Monad m => (b -> m c) -> (a -> m b) -> a -> m c
        // Right-to-left Kleisli composition of monads. (>=>), with the arguments flipped.
        Func<T, Monad<TResult>> ComposeBack<T, TMiddle, TResult>(
            Func<TMiddle, Monad<TResult>> @this,
            Func<T, Monad<TMiddle>> thunk);

        // [Haskell] join :: Monad m => m (m a) -> m a
        // The join function is the conventional monad join operator. It is used to remove
        // one level of monadic structure, projecting its bound argument into the outer level.
        Monad<T> Flatten<T>(Monad<Monad<T>> square);

        // [Haskell] filterM :: Applicative m => (a -> m Bool) -> [a] -> m [a]
        // This generalizes the list-based filter function.
        Monad<IEnumerable<TSource>> WhereBy<TSource>(
            IEnumerable<TSource> @this,
            Func<TSource, Monad<bool>> predicate);

        // [Haskell] mapAndUnzipM :: Applicative m => (a -> m (b, c)) -> [a] -> m ([b], [c])
        // The mapAndUnzipM function maps its first argument over a list, returning the result
        // as a pair of lists. This function is mainly used with complicated data structures
        // or a state-transforming monad.
        Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            SelectUnzip<TSource, TFirst, TSecond>(
            IEnumerable<TSource> @this,
            Func<TSource, Monad<Tuple<TFirst, TSecond>>> thunk);

        // [Haskell] zipWithM :: Applicative m => (a -> b -> m c) -> [a] -> [b] -> m [c]
        // The zipWithM function generalizes zipWith to arbitrary applicative functors.
        Monad<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelector);

        // [Haskell] foldM :: (Foldable t, Monad m) => (b -> a -> m b) -> b -> t a -> m b
        // The foldM function is analogous to foldl, except that its result is encapsulated
        // in a monad. Note that foldM works from left-to-right over the list arguments.
        Monad<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator);

        // [Haskell] guard :: Alternative f => Bool -> f ()
        // guard b is pure () if b is True, and empty if b is False.
        Monad<Unit> Guard(bool predicate);

        // [Haskell] liftM :: Monad m => (a1 -> r) -> m a1 -> m r
        // Promote a function to a monad.
        Func<Monad<T>, Monad<TResult>> Lift<T, TResult>(Func<T, TResult> thunk);

        // [Haskell] liftM2 :: Monad m => (a1 -> a2 -> r) -> m a1 -> m a2 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Monad<T1>, Monad<T2>, Monad<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> thunk);

        // [Haskell] liftM3 :: Monad m => (a1 -> a2 -> a3 -> r) -> m a1 -> m a2 -> m a3 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> thunk);

        // [Haskell] liftM4 :: Monad m => (a1 -> a2 -> a3 -> a4 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> thunk);

        // [Haskell] liftM5 :: Monad m => (a1 -> a2 -> a3 -> a4 -> a5 -> r) -> m a1 -> m a2 -> m a3 -> m a4 -> m a5 -> m r
        // Promote a function to a monad, scanning the monadic arguments from left to right.
        Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> thunk);

        // [Haskell] (<$!>) :: Monad m => (a -> b) -> m a -> m b
        // Strict version of <$>.
        Monad<TResult> InvokeWith<T, TResult>(Func<T, TResult> selector, Monad<T> value);
    }

    public class Monad<T>
    {
        // [Haskell] mzero
        [SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public static Monad<T> Zero { get { throw new FakeClassException(); } }

        // [Haskell] mplus
        public Monad<T> Plus(Monad<T> other)
        {
            throw new FakeClassException();
        }

        // [Haskell] >>=
        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_MAP_MULTIPLY
            return Monad<TResult>.Join(Select(_ => kun.Invoke(_)));
#else
            throw new FakeClassException();
#endif
        }

        // [Haskell] fmap
        public Monad<TResult> Select<TResult>(Func<T, TResult> selector)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new FakeClassException();
#else
            return Bind(_ => Monad<TResult>.Return(selector.Invoke(_)));
#endif
        }

        // [Haskell] >>
        public Monad<TResult> ReplaceBy<TResult>(Monad<TResult> other)
            => Bind(_ => other);

        // [Haskell] return
        internal static Monad<T> Return(T value)
        {
            throw new FakeClassException();
        }

        // [Haskell] join
        internal static Monad<T> Join(Monad<Monad<T>> square)
        {
#if MONAD_VIA_MAP_MULTIPLY
            throw new FakeClassException();
#else
            Kunc<Monad<T>, T> id = _ => _;

            return square.Bind(id);
#endif
        }
    }

    public static partial class Monad
    {
        public static readonly Monad<Unit> Unit = Of(Narvalo.Fx.Unit.Single);

        public static Monad<T> Of<T>(T value) => Monad<T>.Return(value);

        public static Monad<T> Flatten<T>(Monad<Monad<T>> square) => Monad<T>.Join(square);

        // [Haskell] void
        public static Monad<Unit> Skip<T>(this Monad<T> @this)
            => @this.Select(_ => Narvalo.Fx.Unit.Single);
    }
}
