// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Monads
{
    using System;

    // Minimal skeleton definition of a funtor.
    public class Functor<T>
    {
        // Map method.
        public Functor<TResult> Map<TResult>(Func<T, TResult> selector)
        {
            throw new NotImplementedException();
        }
    }

    // Extension methods.
    public static class Functor
    {
        // Replace method.
        public static Functor<T> Replace<T>(this Functor<T> @this, T value)
        {
            return @this.Map(_ => value);
        }

        // Void method.
        public static Functor<Unit> Forget<T>(this Functor<T> @this)
        {
            return @this.Map(_ => Unit.Single);
        }
    }

    public static class FunctorFacts
    {
        // First law: The identity map is a fixed point for Map.
        public static void FirstLaw<X>(Functor<X> m)
        {
            Assert.True(
                m.Map(_ => _) == m
            );
        }

        // Second law: Map preserves the composition operator.
        public static void SecondLaw<X, Y, Z>(Functor<X> m, Func<Y, Z> f, Func<X, Y> g)
        {
            Assert.True(
                m.Map(_ => f(g(_))) == m.Map(g).Map(f)
            );
        }
    }
}
