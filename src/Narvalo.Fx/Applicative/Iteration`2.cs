// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using HashHelpers = Narvalo.Internal.HashHelpers;

    public struct Iteration<TResult, TSource>
        : IEquatable<Iteration<TResult, TSource>>, IStructuralEquatable
    {
        public Iteration(TResult result, TSource next)
        {
            Result = result;
            Next = next;
        }

        public TResult Result { get; }

        public TSource Next { get; }

        public override string ToString()
        {
            return "(Result=" + Result?.ToString() + ", Next=" + Next?.ToString() + ")";
        }

        public static bool operator ==(Iteration<TResult, TSource> left, Iteration<TResult, TSource> right)
            => left.Equals(right);

        public static bool operator !=(Iteration<TResult, TSource> left, Iteration<TResult, TSource> right)
            => !left.Equals(right);

        public bool Equals(Iteration<TResult, TSource> other)
            => EqualityComparer<TResult>.Default.Equals(Result, other.Result)
            && EqualityComparer<TSource>.Default.Equals(Next, other.Next);

        public override bool Equals(object obj)
            => (obj is Iteration<TResult, TSource>) && Equals((Iteration<TResult, TSource>)obj);

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (ReferenceEquals(other, null) || !(other is Iteration<TResult, TSource>)) { return false; }

            var obj = (Iteration<TResult, TSource>)other;

            return comparer.Equals(Result, obj.Result)
                && comparer.Equals(Next, obj.Next);
        }

        public override int GetHashCode()
            => HashHelpers.Combine(Result?.GetHashCode() ?? 0, Next?.GetHashCode() ?? 0);

        public int GetHashCode(IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return HashHelpers.Combine(comparer.GetHashCode(Result), comparer.GetHashCode(Next));
        }
    }
}
