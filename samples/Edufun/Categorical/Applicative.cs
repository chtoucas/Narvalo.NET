// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;

    using Narvalo.Fx;

    // [Haskell] Control.Applicative
    public sealed class Applicative<T>
    {
        // [Haskell] pure
        internal static Applicative<T> Pure(T value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] pure
        internal Applicative<Func<T, TResult>> Pure<TResult>(Func<T, TResult> value)
        {
            throw new NotImplementedException();
        }

        // [Haskell] <*>
        public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative)
        {
            throw new NotImplementedException();
        }
    }

    public static partial class Applicative
    {
        public static Applicative<T> Of<T>(T value) => Applicative<T>.Pure(value);
    }

    // Extension methods
    public static partial class Applicative
    {
        // [Haskell] fmap
        // fmap f x = pure f <*> x
        public static Applicative<TResult> Select<T, TResult>(this Applicative<T> @this, Func<T, TResult> selector)
           => @this.Gather(@this.Pure(selector));

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
