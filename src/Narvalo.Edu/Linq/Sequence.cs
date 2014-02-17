// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;

    public static partial class Sequence
    {
        public static Sequence<TResult> Ana<T, TResult>(
            Func<T, T> succ,
            T seed,
            Func<T, bool> predicate,
            Func<T, TResult> selector)
        {
            throw new NotImplementedException();
        }

        public static Sequence<TResult> Ana<T, TResult>(
            Func<T, T> succ,
            T seed,
            Func<T, TResult> selector)
        {
            return Ana(succ, seed, predicate: _ => true, selector: selector);
        }

        public static Sequence<T> Return<T>(T value)
        {
            return Repeat(value, 1);
        }

        #region Generation Operators

        public static Sequence<int> Range(int start, int count)
        {
            return Ana(i => i + 1, 0, i => i < count, i => start + i);
        }

        public static Sequence<T> Repeat<T>(T value)
        {
            return Ana(i => i + 1, 0, i => true, i => value);
        }

        public static Sequence<T> Repeat<T>(T value, int count)
        {
            return Ana(i => i + 1, 0, i => i < count, i => value);
        }

        public static Sequence<T> Empty<T>()
        {
            return Ana(i => i, 0, i => false, i => default(T));
        }

        #endregion
    }
}
