// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;

    interface IQuerySyntax<T> : IQuerySyntax
    {
        IQuerySyntax<T> Where(Func<T, bool> predicate);

        IQuerySyntax<U> Select<U>(Func<T, U> selector);

        IQuerySyntax<TResult> SelectMany<TMiddle, TResult>(
            Func<T, IQuerySyntax<TMiddle>> selector,
            Func<T, TMiddle, TResult> resultSelector);

        IQuerySyntax<TResult> Join<TInner, TKey, TResult>(
            IQuerySyntax<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector);

        IQuerySyntax<TResult> GroupJoin<TInner, TKey, TResult>(
            IQuerySyntax<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, IQuerySyntax<TInner>, TResult> resultSelector);

        IOrderedQuerySyntax<T> OrderBy<TKey>(Func<T, TKey> keySelector);

        IOrderedQuerySyntax<T> OrderByDescending<TKey>(Func<T, TKey> keySelector);

        IQuerySyntax<IGroupingQuerySyntax<TKey, T>> GroupBy<TKey>(Func<T, TKey> keySelector);

        IQuerySyntax<IGroupingQuerySyntax<TKey, TElement>> GroupBy<TKey, TElement>(
            Func<T, TKey> keySelector,
            Func<T, TElement> elementSelector);
    }

}
