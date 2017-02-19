// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    public partial class Functor
    {
        // ($>) = flip (<$)
        public Functor<TResult> Inject<T, TResult>(TResult other, Functor<T> value)
            => value.Select(_ => other);
    }
}