// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    public partial class Functor<T>
    {
        // GHC.Base: (<$) = fmap . const
        public Functor<TResult> Replace<TResult>(TResult other) => Select(_ => other);
    }

    public partial class Applicative<T>
    {
        // GHC.Base: (<$) = fmap . const
        public Applicative<TResult> Replace<TResult>(TResult other) => Select(_ => other);
    }
}
