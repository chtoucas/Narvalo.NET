// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Monads
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Minimal skeleton definition of a monoid.
    public class Monoid<T>
    {
        // Empty property.
        public static Monoid<T> Empty
        {
            get { throw new NotImplementedException(); }
        }

        // Append method.
        public Monoid<T> Append(Monoid<T> other)
        {
            throw new NotImplementedException();
        }
    }

    // Extension methods.
    public static class Monoid
    {
        // Concat method.
        public static Monoid<T> Flatten<T>(this IEnumerable<Monoid<T>> @this)
        {
            Func<Monoid<T>, Monoid<T>, Monoid<T>> accumulator = (m1, m2) => m1.Append(m2);

            return @this.Aggregate(Monoid<T>.Empty, accumulator);
        }
    }

    public static class MonoidFacts
    {
        // First law: Empty is a left identity for Append.
        public static void FirstLaw<T>(Monoid<T> m)
        {
            Assert.True(
                Monoid<T>.Empty.Append(m) == m
            );
        }

        // Second law: Empty is a right identity for Append.
        public static void SecondLaw<T>(Monoid<T> m)
        {
            Assert.True(
                m.Append(Monoid<T>.Empty) == m
            );
        }

        // Third law: Append is associative.
        public static void ThirdLaw<T>(Monoid<T> a, Monoid<T> b, Monoid<T> c)
        {
            Assert.True(
                a.Append(b.Append(c)) == (a.Append(b)).Append(c)
            );
        }
    }
}
