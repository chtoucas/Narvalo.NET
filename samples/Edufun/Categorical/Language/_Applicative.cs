// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Applicative : IApplicative, IApplicativeGrammar
    {
        public static Applicative<T> Of<T>(T value) { throw new FakeClassException(); }

        public Applicative<T> Pure<T>(T value) => Of(value);
    }

    public partial class Applicative<T> : IApplicative<T>, IApplicativeGrammar<T>
    {
        public Applicative<TResult> Gather<TResult>(Applicative<Func<T, TResult>> applicative)
        {
            throw new FakeClassException();
        }
    }
}
