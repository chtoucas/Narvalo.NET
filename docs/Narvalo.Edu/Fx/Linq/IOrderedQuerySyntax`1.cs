// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System;

    public interface IOrderedQuerySyntax<T> : IQuerySyntax<T>
    {
        IOrderedQuerySyntax<T> ThenBy<TKey>(Func<T, TKey> keySelector);

        IOrderedQuerySyntax<T> ThenByDescending<TKey>(Func<T, TKey> keySelector);
    }
}
