namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Maybe
    {
        public static Maybe<IEnumerable<TResult>> Collect<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> selectorM)
        {
            return Create(SelectAny(source, selectorM));
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> selectorM)
        {
            Require.NotNull(selectorM, "selectorM");

            return from item in source
                   let m = selectorM.Invoke(item)
                   where m.IsSome
                   select m.Value;
        }

        public static Maybe<T> FirstOrNone<T>(IEnumerable<T> source, Predicate<T> predicate)
        {
            Require.NotNull(source, "source");
            Require.NotNull(predicate, "predicate");

            var seq = from t in source where predicate.Invoke(t) select Create(t);
            using (var iter = seq.GetEnumerator()) {
                return iter.MoveNext() ? iter.Current : Maybe<T>.None;
            }
        }

        public static Maybe<T> LastOrNone<T>(IEnumerable<T> source, Predicate<T> predicate)
        {
            Require.NotNull(source, "source");
            Require.NotNull(predicate, "predicate");

            var seq = from t in source where predicate.Invoke(t) select Create(t);
            using (var iter = seq.GetEnumerator()) {
                if (!iter.MoveNext()) {
                    return Maybe<T>.None;
                }

                var value = iter.Current;
                while (iter.MoveNext()) {
                    value = iter.Current;
                }
                
                return value;
            }
        }

        public static Maybe<T> SingleOrNone<T>(IEnumerable<T> source, Predicate<T> predicate)
        {
            Require.NotNull(source, "source");
            Require.NotNull(predicate, "predicate");

            var seq = from t in source where predicate.Invoke(t) select Create(t);
            using (var iter = seq.GetEnumerator()) {
                var result = iter.MoveNext() ? iter.Current : Maybe<T>.None;
                
                // On retourne Maybe.None si il y a encore un élément.
                return iter.MoveNext() ? Maybe<T>.None : result;
            }
        }

        public static Maybe<IEnumerable<T>> Filter<T>(
            IEnumerable<T> source,
            Func<T, Maybe<bool>> predicateM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(predicateM, "predicateM");

            var list = from item in source
                       where predicateM.Invoke(item).Match(b => b, false)
                       select item;

            return Create(list);
        }

        public static Maybe<TAccumulate> FoldLeft<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulatorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(accumulatorM, "accumulatorM");

            Maybe<TAccumulate> option = Create(seed);

            foreach (TSource item in source) {
                option = option.Bind(_ => accumulatorM.Invoke(_, item));
            }

            return option;
        }

        public static Maybe<TAccumulate> FoldRight<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulatorM)
        {
            Require.NotNull(source, "source");

            return FoldLeft(source.Reverse(), seed, accumulatorM);
        }

        public static Maybe<TSource> Reduce<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, TSource, Maybe<TSource>> accumulatorM)
        {
            Require.NotNull(source, "source");

            return Reduce(source.ToArray(), accumulatorM);
        }

        public static Maybe<IList<TResult>> Map<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> kun)
        {
            Require.NotNull(source, "source");
            Require.NotNull(kun, "kun");

            var list = from item in source select kun.Invoke(item);

            return Sequence(list);
        }

        public static Maybe<IList<T>> Sequence<T>(IEnumerable<Maybe<T>> source)
        {
            Require.NotNull(source, "source");

            IList<T> list = new List<T>();

            foreach (var m in source) {
                if (m.IsNone) {
                    return Maybe<IList<T>>.None;
                }

                list.Add(m.Value);
            }

            return Create(list);
        }
    }
}
