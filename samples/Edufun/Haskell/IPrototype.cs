// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Fx;

    public interface IPrototype
    {
        Prototype<IEnumerable<TSource>> Collect<TSource>(IEnumerable<Prototype<TSource>> source);
        Prototype<TResult> Forever<TSource, TResult>(Prototype<TSource> source);
        Prototype<Unit> Guard(bool predicate);
        Prototype<TResult> Inject<TSource, TResult>(TResult value, Prototype<TSource> functor);
        Prototype<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Prototype<TSource> value);
        Func<Prototype<TSource>, Prototype<TResult>> Lift<TSource, TResult>(Func<TSource, TResult> func);
        Func<Prototype<T1>, Prototype<T2>, Prototype<TResult>> Lift<T1, T2, TResult>(Func<T1, T2, TResult> func);
        Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<TResult>> Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func);
        Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<T4>, Prototype<TResult>> Lift<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func);
        Func<Prototype<T1>, Prototype<T2>, Prototype<T3>, Prototype<T4>, Prototype<T5>, Prototype<TResult>> Lift<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> func);
        Prototype<IEnumerable<T>> Many<T>(Prototype<T> value);
        Prototype<Maybe<T>> Optional<T>(Prototype<T> value);
        Prototype<IEnumerable<T>> Some<T>(Prototype<T> value);
        Prototype<TSource> Sum<TSource>(IEnumerable<Prototype<TSource>> source);
        Prototype<Unit> Unless(bool predicate, Prototype<Unit> value);
        Prototype<Unit> When(bool predicate, Prototype<Unit> value);
    }

    public interface IPrototype<T>
    {
        Prototype<TResult> Bind<TResult>(Func<T, Prototype<TResult>> selector);

        Prototype<TResult> Gather<TResult>(Prototype<Func<T, TResult>> applicative);
        Prototype<T> Ignore<TOther>(Prototype<TOther> other);
        Prototype<IEnumerable<T>> Repeat(int count);
        Prototype<TResult> Replace<TResult>(TResult other);
        Prototype<TResult> ReplaceBy<TResult>(Prototype<TResult> other);
        Prototype<TResult> Select<TResult>(Func<T, TResult> selector);
        Prototype<Unit> Skip();
        Prototype<TResult> Zip<TSecond, TResult>(Prototype<TSecond> second, Func<T, TSecond, TResult> resultSelector);
        Prototype<TResult> Zip<T2, T3, TResult>(Prototype<T2> second, Prototype<T3> third, Func<T, T2, T3, TResult> resultSelector);

        Prototype<T> Append(Prototype<T> value);
        Prototype<T> Plus(Prototype<T> value);

        Prototype<T> Where(Func<T, bool> predicate);
    }
}