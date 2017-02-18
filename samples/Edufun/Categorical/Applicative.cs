// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Narvalo.Fx;

    // [Haskell] Control.Applicative
    // In-between a Functor and a Monad.
    //
    // Translation from Haskell to .NET.
    // Requirements:
    // - pure       Pure
    // - <*>        Gather
    // API:
    // - *>         Replace
    // - <*         Ignore_
    // - fmap       Select
    // Utility functions:
    // - <$>        Invoke_
    // - <$         Replace
    // - <**>       Apply
    // - liftA
    // - liftA2     Zip
    // - liftA3     Zip
    // - optional

    public interface IApplicative<T>
    {
        // [Haskell] pure :: a -> f a
        // Embed pure expressions ie lift a value.
        Applicative<T> Pure(T value);

        // [Haskell] (<*>) :: f (a -> b) -> f a -> f b
        // Sequence computations and combine their results.
        Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative);
    }

    public sealed class Applicative<T>
    {
        public static Applicative<T> Pure(T value)
        {
            throw new NotImplementedException();
        }

        public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative)
        {
            throw new NotImplementedException();
        }

        // If an applicative functor is also a monad, it should satisfy:
        // pure = return
        // (<*>) = ap
        public static class Rules
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
                return true;
            }
        }
    }

    public static partial class Applicative
    {
        public static Applicative<T> Of<T>(T value) => Applicative<T>.Pure(value);
    }

    // Extension methods
    public static partial class ApplicativeApi
    {
        // [Haskell] fmap
        // fmap f x = pure f <*> x
        public static Applicative<TResult> Select<T, TResult>(this Applicative<T> @this, Func<T, TResult> selector)
            => @this.Gather(Applicative.Of(selector));

        // [Haskell] *>
        // u *> v = pure (const id) <*> u <*> v
        //public static Applicative<TResult> ReplaceBy<T, TResult>(
        //    this Applicative<T> @this,
        //    Applicative<TResult> other)
        //{

        //}

        // <$ :: a -> f b -> f a
        // fmap . const
        public static Applicative<T> Replace<T>(this Applicative<T> @this, T value)
            => @this.Select(_ => value);


        public static Applicative<Unit> Zip<TFirst, TSecond, TResult>(
            this Applicative<TFirst> @this,
            Applicative<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            throw new NotImplementedException();
        }

        public static Applicative<TResult> Zip<T1, T2, T3, TResult>(
            this Applicative<T1> @this,
            Applicative<T2> second,
            Applicative<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            throw new NotImplementedException();
        }
    }
}
