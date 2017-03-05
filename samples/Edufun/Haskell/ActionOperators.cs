// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#define STRICT_HASKELL

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;
    using Narvalo.Linq;

    // Partial work on methods that end with a "_".
    public partial class ActionOperators
    {
        // [Haskell] sequence_ :: (Foldable t, Monad m) => t (m a) -> m ()
        // [Data.Foldable] sequence_ = foldr (>>) (return ())
        public Prototype<Unit> Collect_<TSource>(IEnumerable<Prototype<TSource>> source)
        {
            throw new NotImplementedException();
        }

        // [Haskell] foldM_ :: (Foldable t, Monad m) => (b -> a -> m b) -> b -> t a -> m ()
        // [Control.Monad] foldlM f a xs >> return ()
        public Prototype<Unit> Fold_<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Prototype<TAccumulate>> accumulator)
        {
            throw new NotImplementedException();
        }

        // [Haskell] forM_ :: (Foldable t, Monad m) => t a -> (a -> m b) -> m ()
        // [Data.Foldable] forM_ = flip mapM_
        public Prototype<Unit> InvokeWith_<TSource, TResult>(
            Func<TSource, Prototype<TResult>> selector,
            IEnumerable<TSource> source)
            => SelectWith_(source, selector);

        // [Haskell] replicateM_ :: Applicative m => Int -> m a -> m ()
        public Prototype<Unit> Repeat_<TSource>(Prototype<TSource> source, int count)
        {
            throw new NotImplementedException();
        }

        // [Haskell] mapM_ :: (Foldable t, Monad m) => (a -> m b) -> t a -> m ()
        // [Data.Foldable] mapM_ f = foldr ((>>) . f) (return ())
        public Prototype<Unit> SelectWith_<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Prototype<TResult>> selector)
        {
            Func<Prototype<Unit>, TSource, Prototype<Unit>> func
                = (arg1, arg2) => selector(arg2).ContinueWith(arg1);

            return source.Aggregate(Prototype.Of(Unit.Default), func);
        }

        // [Haskell] zipWithM_ :: Applicative m => (a -> b -> m c) -> [a] -> [b] -> m ()
        // [Control.Monad] zipWithM_ f xs ys =  sequenceA_ (zipWith f xs ys)
        public Prototype<Unit> ZipWith_<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Prototype<TResult>> resultSelector)
        {
            throw new NotImplementedException();
        }
    }
}