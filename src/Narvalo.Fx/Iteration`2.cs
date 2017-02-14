// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public struct Iteration<TResult, TSource> : IEquatable<Iteration<TResult, TSource>> //, IComparable
    {
        public Iteration(TResult result, TSource next)
        {
            Result = result;
            Next = next;
        }

        public TResult Result { get; }

        public TSource Next { get; }

        public static bool operator ==(Iteration<TResult, TSource> left, Iteration<TResult, TSource> right)
            => left.Equals(right);

        public static bool operator !=(Iteration<TResult, TSource> left, Iteration<TResult, TSource> right)
            => !left.Equals(right);

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj) => Equals(obj, EqualityComparer<object>.Default);

        public bool Equals(object other, IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (!(other is Iteration<TResult, TSource>))
            {
                return false;
            }

            return Equals((Iteration<TResult, TSource>)other);
        }

        public bool Equals(Iteration<TResult, TSource> other)
            => Equals(other, EqualityComparer<object>.Default);

        public bool Equals(Iteration<TResult, TSource> other, IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return comparer.Equals(Result, other.Result)
                && comparer.Equals(Next, other.Next);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode() => GetHashCode(EqualityComparer<object>.Default);

        public int GetHashCode(IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            int h1 = Result != null ? comparer.GetHashCode(Result) : 0;
            int h2 = Next != null ? comparer.GetHashCode(Next) : 0;

            return ((h1 << 5) + h1) ^ h2;
        }

        //public int CompareTo(object obj)
        //{
        //    return CompareTo(obj, Comparer<Object>.Default);
        //}

        //public int CompareTo(object obj, IComparer comparer)
        //{
        //    Require.NotNull(comparer, nameof(comparer));

        //    if (!(obj is Iteration<TResult, TSource>))
        //    {
        //        throw new ArgumentException();
        //    }

        //    var other = (Iteration<TResult, TSource>)obj;

        //    int c = 0;

        //    c = comparer.Compare(Result, other.Result);

        //    if (c != 0)
        //    {
        //        return c;
        //    }

        //    return comparer.Compare(Next, other.Next);
        //}
    }
}
