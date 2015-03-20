// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Internal;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that depend on the <see cref="Maybe{T}"/> class.
    /// </summary>
    public static partial class EnumerableMaybeExtensions { }

    /// <content>
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that are specific to the <see cref="Maybe{T}"/> class.
    /// </content>
    public static partial class EnumerableMaybeExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            // We could use a LINQ query expression but it then won't be possible 
            // to add the Code Contracts workarounds.
            //  return from _ in @this
            //      let m = funM.Invoke(_)
            //      where m.IsSome
            //      select m.Value;
            return @this
                .Select(_ => funM.Invoke(_)).AssumeNotNull()
                .Where(_ => _.IsSome)
                .Select(_ => _.Value).AssumeNotNull();
        }

        #region Element Operators

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            return FirstOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            IEnumerable<Maybe<TSource>> seq
                = from t in @this where predicate.Invoke(t) select Maybe.Create(t);

            using (var iter = seq.AssumeNotNull().GetEnumerator())
            {
                return iter.MoveNext() ? iter.Current.AssumeNotNull() : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            return LastOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            IEnumerable<Maybe<TSource>> seq
                = from t in @this where predicate.Invoke(t) select Maybe.Create(t);

            using (var iter = seq.AssumeNotNull().GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    return Maybe<TSource>.None;
                }

                var value = iter.Current.AssumeNotNull();
                while (iter.MoveNext())
                {
                    value = iter.Current.AssumeNotNull();
                }

                return value;
            }
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            return SingleOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            IEnumerable<Maybe<TSource>> seq
                = from t in @this where predicate.Invoke(t) select Maybe.Create(t);

            using (var iter = seq.AssumeNotNull().GetEnumerator())
            {
                var result = iter.MoveNext() ? iter.Current.AssumeNotNull() : Maybe<TSource>.None;

                // Return Maybe.None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        #endregion
    }

    /// <content>
    /// Provides overrides for a bunch of auto-generated (extension) methods (see Maybe.g.cs).
    /// </content>
    public static partial class EnumerableMaybeExtensions
    {
        // Custom version of CollectCore.
        internal static Maybe<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Maybe<IEnumerable<TSource>>>() != null);

            var list = new List<TSource>();

            foreach (var m in @this)
            {
                // REVIEW: Is this the correct behaviour when m is null?
                if (m == null || m.IsNone)
                {
                    return Maybe<IEnumerable<TSource>>.None;
                }

                list.Add(m.Value);
            }

            return Maybe.Create(list.AsEnumerable());
        }

        // Custom version of FilterCore with Maybe<T>.
        internal static IEnumerable<TSource> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            return @this
                .Where(_ => predicateM.Invoke(_).ValueOrElse(false))
                .AssumeNotNull();
        }
    }
}
