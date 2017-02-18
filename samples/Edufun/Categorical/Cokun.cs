// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    public delegate T Cokunc<T>(Comonad<T> arg);

    public delegate TResult Cokunc<T, out TResult>(Comonad<T> arg);
}
