namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Maybe
    {
        public static Maybe<IEnumerable<TResult>> Collect<TSource, TResult>(
            IEnumerable<TSource> source,
            MayFunc<TSource, TResult> selectorM)
        {
            return Create(SelectAny(source, selectorM));
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            IEnumerable<TSource> source,
            MayFunc<TSource, TResult> selectorM)
        {
            Requires.NotNull(selectorM, "selectorM");

            return from item in source
                   let m = selectorM(item)
                   where m.IsSome
                   select m.Value;
        }

        #region > Single <

        public static Maybe<T> FirstOrNone<T>(IEnumerable<T> source, Predicate<T> predicate)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(predicate, "predicate");

            var seq = from t in source where predicate(t) select Create(t);
            using (var iter = seq.GetEnumerator()) {
                return iter.MoveNext() ? iter.Current : Maybe<T>.None;
            }
        }

        public static Maybe<T> LastOrNone<T>(IEnumerable<T> source, Predicate<T> predicate)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(predicate, "predicate");

            var seq = from t in source where predicate(t) select Create(t);
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
            Requires.NotNull(source, "source");
            Requires.NotNull(predicate, "predicate");

            var seq = from t in source where predicate(t) select Create(t);
            using (var iter = seq.GetEnumerator()) {
                var result = iter.MoveNext() ? iter.Current : Maybe<T>.None;
                // On retourne Maybe.None si il y a encore un élément.
                return iter.MoveNext() ? Maybe<T>.None : result;
            }
        }

        #endregion

        public static Maybe<IEnumerable<TSource>> Filter<TSource>(
            IEnumerable<TSource> source,
            MayFunc<TSource, bool> predicateM)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(predicateM, "predicateM");

            var list = from item in source
                       where predicateM(item).Match(b => b, false)
                       select item;

            return Create(list);
        }

        #region > Aggregate <

        public static Maybe<TAccumulate> FoldLeft<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            MayFunc<TAccumulate, TSource, TAccumulate> accumulatorM)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(accumulatorM, "accumulatorM");

            Maybe<TAccumulate> option = Create(seed);

            foreach (TSource item in source) {
                option = option.Bind(_ => accumulatorM(_, item));
            }

            return option;
        }

        public static Maybe<TAccumulate> FoldRight<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            MayFunc<TAccumulate, TSource, TAccumulate> accumulatorM)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(accumulatorM, "accumulatorM");

            return FoldLeft(source.Reverse(), seed, accumulatorM);
        }

        public static Maybe<TSource> Reduce<TSource>(
            IEnumerable<TSource> source,
            MayFunc<TSource, TSource, TSource> accumulatorM)
        {
            Requires.NotNull(source, "source");

            // FIXME
            return Reduce(source.ToArray(), accumulatorM);
        }

        #endregion

        public static Maybe<IList<TResult>> Map<TSource, TResult>(
            IEnumerable<TSource> source,
            MayFunc<TSource, TResult> kun)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(kun, "kun");

            var list = from item in source select kun(item);

            return Sequence(list);
        }

        public static Maybe<IList<TSource>> Sequence<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Requires.NotNull(source, "source");

            IList<TSource> list = new List<TSource>();

            foreach (var m in source) {
                if (m.IsNone) {
                    return Maybe<IList<TSource>>.None;
                }
                list.Add(m.Value);
            }

            return Create(list);
        }
    }
}
