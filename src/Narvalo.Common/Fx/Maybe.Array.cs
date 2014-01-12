namespace Narvalo.Fx
{
    using System;
    using System.Linq;

    public static partial class Maybe
    {
        public static Maybe<TResult[]> Collect<TSource, TResult>(
            TSource[] source,
            MayFunc<TSource, TResult> selectorM)
        {
            return Create(SelectAny(source, selectorM));
        }

        public static TResult[] SelectAny<TSource, TResult>(
            TSource[] source,
            MayFunc<TSource, TResult> selectorM)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(selectorM, "selectorM");

            Maybe<TResult>[] options = Array.ConvertAll(source, _ => selectorM(_));
            Maybe<TResult>[] values = Array.FindAll(options, _ => _.IsSome);

            int length = values.Length;
            TResult[] list = new TResult[length];
            for (int i = 0; i < length; i++) {
                list[i] = values[i].Value;
            }

            return list;
        }

        public static Maybe<TSource[]> Filter<TSource>(
            TSource[] source, 
            MayFunc<TSource, bool> predicateM)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(predicateM, "predicateM");

            TSource[] list = Array.FindAll(
                source,
                _ => predicateM(_).Match(b => b, false));

            return Create(list);
        }

        #region > Aggregate <

        public static Maybe<TResult> FoldLeft<TSource, TResult>(
            TSource[] source,
            TResult seed,
            MayFunc<TResult, TSource, TResult> accumulatorM)
        {
            Requires.NotNull(source, "@this");
            Requires.NotNull(accumulatorM, "accumulatorM");

            Maybe<TResult> option = Create(seed);

            int length = source.Length;
            // FIXME if (length == 0) { return @this; }

            for (int i = 0; i < length; i++) {
                option = option.Bind(_ => accumulatorM(_, source[i]));
            }

            return option;
        }

        public static Maybe<TResult> FoldRight<TSource, TResult>(
            TSource[] source,
            TResult seed,
            MayFunc<TResult, TSource, TResult> accumulatorM)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(accumulatorM, "accumulator");

            return FoldLeft(source.Reverse(), seed, accumulatorM);
        }

        public static Maybe<TSource> Reduce<TSource>(
            TSource[] source,
            MayFunc<TSource, TSource, TSource> accumulatorM)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(accumulatorM, "accumulator");

            int length = source.Length;
            if (length == 0) { return Maybe<TSource>.None; }

            Maybe<TSource> option = Create(source[0]);

            for (int i = 1; i < length; i++) {
                option = option.Bind(_ => accumulatorM(_, source[i]));
            }

            return option;
        }

        #endregion

        public static Maybe<TResult[]> Map<TSource, TResult>(
            TSource[] source,
            MayFunc<TSource, TResult> funM)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(funM, "funM");

            Maybe<TResult>[] list = Array.ConvertAll(source, _ => funM(_));

            return Sequence(list);
        }

        public static Maybe<T[]> Sequence<T>(Maybe<T>[] source)
        {
            Requires.NotNull(source, "source");

            int length = source.Length;
            T[] list = new T[length];

            for (int i = 0; i < length; i++) {
                Maybe<T> m = source[i];
                if (m.IsNone) {
                    return Maybe<T[]>.None;
                }
                list[i] = m.Value;
            }

            return Create(list);
        }

        //public static Maybe<T3[]> Zip<T1, T2, T3>(
        //    T1[] list1,
        //    T2[] list2,
        //    Func<T1, T2, Maybe<T3>> fun)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
