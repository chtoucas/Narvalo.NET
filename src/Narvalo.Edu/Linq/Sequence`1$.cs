// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class SequenceExtensions
    {
        #region Quantifiers

        public static bool Any<T>(this Sequence<T> seq, Func<T, bool> predicate)
        {
            return seq.Cata(true, acc => !acc, (acc, item) => acc || predicate.Invoke(item));
        }

        public static bool All<T>(this Sequence<T> seq, Func<T, bool> predicate)
        {
            return seq.Cata(true, acc => acc, (acc, item) => acc && predicate.Invoke(item));
        }

        public static bool Contains<T>(this Sequence<T> seq, T value)
        {
            return Contains(seq, value, EqualityComparer<T>.Default);
        }

        public static bool Contains<T>(this Sequence<T> seq, T value, IEqualityComparer<T> comparer)
        {
            return Any(seq, _ => comparer.Equals(_, value));
        }

        #endregion

        #region Aggregate Operators

        public static int Count<T>(this Sequence<T> seq)
        {
            return Aggregate(seq, 0, (acc, item) => checked(acc + 1));
        }

        public static long LongCount<T>(this Sequence<T> seq)
        {
            return Aggregate(seq, 0L, (acc, item) => checked(acc + 1));
        }

        public static int Sum(this Sequence<int> seq)
        {
            return Sum(seq, i => i);
        }

        public static int Sum<T>(this Sequence<T> seq, Func<T, int> selector)
        {
            return Aggregate(seq, 0, (acc, item) => checked(acc + selector.Invoke(item)));
        }

        public static TResult Aggregate<T, TResult>(
            this Sequence<T> seq,
            TResult seed,
            Func<TResult, T, TResult> accumulator)
        {
            return seq.Cata(seed, _ => true, accumulator);
        }

        #endregion

        #region Conversion Operators

        public static IEnumerable<T> AsEnumerable<T>(this Sequence<T> seq)
        {
            while (true) {
                yield return seq.Iterator.Invoke();
            }
        }

        #endregion
    }
}
