namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="System.Collections.Generic.IEnumerable{T}"/>.
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

        public static TAccumulate FoldLeft<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Require.Object(@this);

            return @this.Aggregate(seed, accumulator);
        }

        public static TAccumulate FoldRight<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Require.Object(@this);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static T Reduce<T>(
            this IEnumerable<T> @this,
            Func<T, T, T> accumulator)
        {
            Require.Object(@this);

            return @this.Aggregate(accumulator);
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> @this, T element)
        {
            Require.Object(@this);

            return AppendImpl_(@this, element);
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> @this, T element)
        {
            Require.Object(@this);

            return PrependImpl_(@this, element);
        }

        static IEnumerable<T> AppendImpl_<T>(IEnumerable<T> source, T element)
        {
            foreach (var item in source) {
                yield return item;
            }

            yield return element;
        }

        static IEnumerable<T> PrependImpl_<T>(IEnumerable<T> source, T element)
        {
            yield return element;

            foreach (var item in source) {
                yield return item;
            }
        }
    }
}
