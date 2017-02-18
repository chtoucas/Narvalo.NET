// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Sequences
{
    using System;

    internal delegate Func<TSource, TResult> Recursive<TSource, TResult>(Recursive<TSource, TResult> rec);
}
