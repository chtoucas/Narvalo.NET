// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Linq
{
    public interface IGroupingQuerySyntax<TKey, T> : IQuerySyntax<T>
    {
        TKey Key { get; }
    }
}
