namespace Narvalo.Fx
{
    using System;
    using System.Linq;

    public static partial class Maybe
    {
        public static Maybe<TResult[]> Collect<TSource, TResult>(
            TSource[] source,
            Func<TSource, Maybe<TResult>> selectorM)
        {
            return Create(SelectAny(source, selectorM));
        }

        public static TResult[] SelectAny<TSource, TResult>(
            TSource[] source,
            Func<TSource, Maybe<TResult>> selectorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(selectorM, "selectorM");

            Maybe<TResult>[] options = Array.ConvertAll(source, _ => selectorM.Invoke(_));
            Maybe<TResult>[] values = Array.FindAll(options, _ => _.IsSome);

            int length = values.Length;
            TResult[] list = new TResult[length];
            for (int i = 0; i < length; i++) {
                list[i] = values[i].Value;
            }

            return list;
        }

        public static Maybe<T[]> Filter<T>(T[] source, Func<T, Maybe<bool>> predicateM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(predicateM, "predicateM");

            T[] list = Array.FindAll(
                source,
                _ => predicateM.Invoke(_).Match(b => b, false));

            return Create(list);
        }

        public static Maybe<TResult> FoldLeft<TSource, TResult>(
            TSource[] source,
            TResult seed,
            Func<TResult, TSource, Maybe<TResult>> accumulatorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(accumulatorM, "accumulatorM");

            Maybe<TResult> result = Create(seed);

            int length = source.Length;

            if (length == 0) { return result; }

            for (int i = 0; i < length; i++) {
                result = result.Bind(_ => accumulatorM.Invoke(_, source[i]));
            }

            return result;
        }

        public static Maybe<TResult> FoldRight<TSource, TResult>(
            TSource[] source,
            TResult seed,
            Func<TResult, TSource, Maybe<TResult>> accumulatorM)
        {
            Require.NotNull(source, "source");

            return FoldLeft(source.Reverse(), seed, accumulatorM);
        }

        public static Maybe<TSource> Reduce<TSource>(
            TSource[] source,
            Func<TSource, TSource, Maybe<TSource>> accumulatorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(accumulatorM, "accumulator");

            int length = source.Length;
            if (length == 0) { return Maybe<TSource>.None; }

            Maybe<TSource> result = Create(source[0]);

            for (int i = 1; i < length; i++) {
                result = result.Bind(_ => accumulatorM.Invoke(_, source[i]));
            }

            return result;
        }

        public static Maybe<TResult[]> Map<TSource, TResult>(
            TSource[] source,
            Func<TSource, Maybe<TResult>> kun)
        {
            Require.NotNull(source, "source");
            Require.NotNull(kun, "kun");

            Maybe<TResult>[] list = Array.ConvertAll(source, _ => kun.Invoke(_));

            return Sequence(list);
        }

        public static Maybe<T[]> Sequence<T>(Maybe<T>[] source)
        {
            Require.NotNull(source, "source");

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
    }
}
