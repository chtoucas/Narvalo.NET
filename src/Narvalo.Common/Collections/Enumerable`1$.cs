// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="System.Collections.Generic.IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        public static ICollection<T> ToCollection<T>(this IEnumerable<T> @this)
        {
            Require.Object(@this);

            var result = new Collection<T>();

            foreach (T item in @this) {
                result.Add(item);
            }

            return result;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> @this)
        {
            Require.Object(@this);

            return !@this.Any();
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> @this, T element)
        {
            Require.Object(@this);

            // return AppendCore_(@this, element);
            return @this.Concat(Enumerable.Repeat(element, 1));
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> @this, T element)
        {
            Require.Object(@this);

            // return PrependCore_(@this, element);
            return Enumerable.Repeat(element, 1).Concat(@this);
        }

        ////static IEnumerable<T> AppendCore_<T>(IEnumerable<T> source, T element)
        ////{
        ////    foreach (var item in source) {
        ////        yield return item;
        ////    }

        ////    yield return element;
        ////}

        ////static IEnumerable<T> PrependCore_<T>(IEnumerable<T> source, T element)
        ////{
        ////    yield return element;

        ////    foreach (var item in source) {
        ////        yield return item;
        ////    }
        ////}
    }

    // Extensions using monadic classes.
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> funM)
        {
            Require.NotNull(funM, "funM");

            return from _ in @this
                   let m = funM.Invoke(_)
                   where m.IsSome
                   select m.Value;
        }

        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<TResult>> funM)
        {
            Require.NotNull(funM, "funM");

            return from _ in @this
                   let m = funM.Invoke(_)
                   where m.IsSuccess
                   select m.Value;
        }

        #region Element Operators

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return @this.FirstOrNone(_ => true);
        }

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Maybe.Create(t);
            using (var iter = seq.GetEnumerator()) {
                return iter.MoveNext() ? iter.Current : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return @this.LastOrNone(_ => true);
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Maybe.Create(t);
            using (var iter = seq.GetEnumerator()) {
                if (!iter.MoveNext()) {
                    return Maybe<TSource>.None;
                }

                var value = iter.Current;
                while (iter.MoveNext()) {
                    value = iter.Current;
                }

                return value;
            }
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return @this.SingleOrNone(_ => true);
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Maybe.Create(t);
            using (var iter = seq.GetEnumerator()) {
                var result = iter.MoveNext() ? iter.Current : Maybe<TSource>.None;

                // On retourne Maybe.None si il y a encore un élément.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        #endregion
    }

    // Optimized extensions for Maybe<T>.
    public static partial class EnumerableExtensions
    {
        internal static IEnumerable<TSource> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.NotNull(predicateM, "predicateM");

            return from _ in @this
                   where predicateM.Invoke(_).ValueOrElse(false)
                   select _;
        }
    }
}
