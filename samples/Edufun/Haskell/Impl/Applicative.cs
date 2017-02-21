// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Impl
{
    using System;

    public static class Applicative
    {
        public static Applicative<T> Of<T>(T value) { throw new FakeClassException(); }
    }

    public partial class Applicative<T> : IApplicative<T>, IApplicativeSyntax<T>
    {
        public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative)
        {
            throw new FakeClassException();
        }

        public Applicative<TSource> Of_<TSource>(TSource value) => Applicative.Of(value);
    }
}
