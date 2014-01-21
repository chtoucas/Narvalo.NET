namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MaybeM
    {
        public static Maybe<T[]> Filter<T>(
           T[] source,
           Func<T, Maybe<bool>> predicateM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(predicateM, "predicateM");

            T[] list = Array.FindAll(
                source,
                _ => { Maybe<bool> m = predicateM.Invoke(_); return m.IsSome && m.Value; });

            return Maybe.Create(list);
        }

        public static Maybe<TResult> FoldLeft<TSource, TResult>(
            TSource[] source,
            TResult seed,
            Func<TResult, TSource, Maybe<TResult>> accumulatorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(accumulatorM, "accumulatorM");

            Maybe<TResult> option = Maybe.Create(seed);

            int length = source.Length;
            //if (length == 0) { return option; }

            for (int i = 0; i < length; i++) {
                option = option.Bind(_ => accumulatorM.Invoke(_, source[i]));
            }

            return option;
        }

        public static Maybe<TResult> FoldRight<TSource, TResult>(
            TSource[] source,
            TResult seed,
            Func<TResult, TSource, Maybe<TResult>> accumulatorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(accumulatorM, "accumulatorM");

            Maybe<TResult> option = Maybe.Create(seed);

            int length = source.Length;
            //if (length == 0) { return option; }

            for (int i = length - 1; i >= 0; i--) {
                option = option.Bind(_ => accumulatorM.Invoke(_, source[i]));
            }

            return option;
        }

        public static Maybe<TResult[]> Map<TSource, TResult>(
            TSource[] source,
            Func<TSource, Maybe<TResult>> kun)
        {
            Require.NotNull(source, "source");
            Require.NotNull(kun, "kun");

            Maybe<TResult>[] options = Array.ConvertAll(source, _ => kun.Invoke(_));

            return Sequence(options);
        }

        public static Maybe<T> Reduce<T>(
            T[] source,
            Func<T, T, Maybe<T>> kun)
        {
            Require.NotNull(source, "source");
            Require.NotNull(kun, "kun");

            int length = source.Length;
            if (length == 0) { return Maybe<T>.None; }

            Maybe<T> option = Maybe.Create(source[0]);
            
            for (int i = 1; i < length; i++) {
                option = option.Bind(_ => kun.Invoke(_, source[i]));
            }

            return option;
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

            return Maybe.Create(list);
        }

        public static Maybe<T3[]> Zip<T1, T2, T3>(
            T1[] list1,
            T2[] list2,
            Func<T1, T2, Maybe<T3>> fun)
        {
            throw new NotImplementedException();
        }

        public static Maybe<IEnumerable<T3>> Zip<T1, T2, T3>(
            IEnumerable<T1> list1,
            IEnumerable<T2> list2,
            Func<T1, T2, Maybe<T3>> fun)
        {
            throw new NotImplementedException();
        }

        #region Collections.

        public static Maybe<IEnumerable<T>> Filter<T>(
            IEnumerable<T> source,
            Func<T, Maybe<bool>> predicateM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(predicateM, "predicateM");

            var list = from item in source
                       let m = predicateM.Invoke(item)
                       where m.IsSome && m.Value
                       select item;

            return Maybe.Create(list);
        }

        public static Maybe<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulatorM)
        {
            Require.NotNull(source, "source");
            Require.NotNull(accumulatorM, "accumulatorM");

            Maybe<TAccumulate> option = Maybe.Create(seed);

            foreach (TSource item in source) {
                option = option.Bind(_ => accumulatorM.Invoke(_, item));
            }

            return option;
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

            return Maybe.Create(list);
        }

        //public static Maybe<IEnumerable<T3>> Zip<T1, T2, T3>(
        //    IEnumerable<T1> list1,
        //    IEnumerable<T2> list2,
        //    Func<T1, T2, Maybe<T3>> fun)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Promotion de fonctions.

        public static Maybe<TResult> Lift<TSource, TResult>(
            Func<TSource, TResult> fun,
            Maybe<TSource> option)
        {
            Require.NotNull(fun, "fun");

            return option.IsSome ? Maybe.Create(fun.Invoke(option.Value)) : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, TResult>(
            Func<T1, T2, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2)
        {
            Require.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome
                ? Maybe.Create(fun.Invoke(option1.Value, option2.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2,
            Maybe<T3> option3)
        {
            Require.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome
                ? Maybe.Create(fun.Invoke(option1.Value, option2.Value, option3.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2,
            Maybe<T3> option3,
            Maybe<T4> option4)
        {
            Require.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome && option4.IsSome
                ? Maybe.Create(fun.Invoke(option1.Value, option2.Value, option3.Value, option4.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, T3, T4, T5, TResult>(
           Func<T1, T2, T3, T4, T5, TResult> fun,
           Maybe<T1> option1,
           Maybe<T2> option2,
           Maybe<T3> option3,
           Maybe<T4> option4,
           Maybe<T5> option5)
        {
            Require.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome
                    && option4.IsSome && option5.IsSome
                ? Maybe.Create(fun.Invoke(option1.Value, option2.Value, option3.Value,
                    option4.Value, option5.Value))
                : Maybe<TResult>.None;
        }

        #endregion
    }
}
