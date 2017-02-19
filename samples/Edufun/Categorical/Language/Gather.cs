// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Applicative<T>
    {
        Applicative<TResult> IApplicative<T>.Gather<TResult>(Applicative<Func<T, TResult>> applicative)
            => Gather(applicative);
    }
}