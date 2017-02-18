// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Monads
{
    using System;

    using Narvalo.Fx;

    // Minimal skeleton definition of an applicative funtor.
    public class Applicative<T>
    {
        // Map method.
        public Applicative<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            throw new NotImplementedException();
        }

        // Pure method.
        public static Applicative<T> Pure(T value)
        {
            throw new NotImplementedException();
        }

        // Gather method.
        public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> funs)
        {
            throw new NotImplementedException();
        }
    }

    // Extension methods.
    public static class Applicative
    {
        // Replace method.
        public static Applicative<T> Replace<T>(this Applicative<T> @this, T value)
            => @this.Map(_ => value);

        // Void method.
        public static Applicative<Unit> Forget<T>(this Applicative<T> @this)
            => @this.Map(_ => Unit.Single);

        // Zip method.
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
