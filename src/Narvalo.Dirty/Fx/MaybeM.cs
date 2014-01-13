namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Narvalo.Fx;

    public static class MaybeM
    {
        #region Tableaux.

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<T[]> Filter<T>(
           T[] source,
           Func<T, Maybe<bool>> predicate)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(predicate, "predicate");

            T[] list = Array.FindAll(
                source,
                _ => { Maybe<bool> m = predicate(_); return m.IsSome && m.Value; });

            return Maybe.Create(list);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<TResult> FoldLeft<TSource, TResult>(
            TSource[] source,
            TResult seed,
            Func<TResult, TSource, Maybe<TResult>> fun)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(fun, "fun");

            Maybe<TResult> option = Maybe.Create(seed);

            int length = source.Length;
            //if (length == 0) { return option; }

            for (int i = 0; i < length; i++) {
                option = option.Bind(_ => fun(_, source[i]));
            }

            return option;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<TResult> FoldRight<TSource, TResult>(
            TSource[] source,
            TResult seed,
            Func<TResult, TSource, Maybe<TResult>> fun)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(fun, "fun");

            Maybe<TResult> option = Maybe.Create(seed);

            int length = source.Length;
            //if (length == 0) { return option; }

            for (int i = length - 1; i >= 0; i--) {
                option = option.Bind(_ => fun(_, source[i]));
            }

            return option;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<TResult[]> Map<TSource, TResult>(
            TSource[] source,
            Func<TSource, Maybe<TResult>> fun)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(fun, "fun");

            Maybe<TResult>[] options = Array.ConvertAll(source, _ => fun(_));

            return Sequence(options);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<TSource> Reduce<TSource>(
            TSource[] source,
            Func<TSource, TSource, Maybe<TSource>> fun)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(fun, "fun");

            int length = source.Length;
            if (length == 0) { return Maybe<TSource>.None; }

            Maybe<TSource> option = Maybe.Create(source[0]);
            
            for (int i = 1; i < length; i++) {
                option = option.Bind(_ => fun(_, source[i]));
            }

            return option;
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

            return Maybe.Create(list);
        }

        //public static Maybe<T3[]> Zip<T1, T2, T3>(
        //    T1[] list1,
        //    T2[] list2,
        //    Func<T1, T2, Maybe<T3>> fun)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Collections.

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<IEnumerable<T>> Filter<T>(
            IEnumerable<T> source,
            Func<T, Maybe<bool>> predicate)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(predicate, "predicate");

            var list = from item in source
                       let m = predicate(item)
                       where m.IsSome && m.Value
                       select item;

            return Maybe.Create(list);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> fun)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(fun, "fun");

            Maybe<TAccumulate> option = Maybe.Create(seed);

            foreach (TSource item in source) {
                option = option.Bind(_ => fun(_, item));
            }

            return option;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<IList<TResult>> Map<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<TResult>> fun)
        {
            Requires.NotNull(source, "source");
            Requires.NotNull(fun, "fun");

            var list = from item in source select fun(item);

            return Sequence(list);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static Maybe<IList<T>> Sequence<T>(IEnumerable<Maybe<T>> source)
        {
            Requires.NotNull(source, "source");

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
            Requires.NotNull(fun, "fun");

            return option.IsSome ? Maybe.Create(fun(option.Value)) : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, TResult>(
            Func<T1, T2, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2)
        {
            Requires.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome
                ? Maybe.Create(fun(option1.Value, option2.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2,
            Maybe<T3> option3)
        {
            Requires.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome
                ? Maybe.Create(fun(option1.Value, option2.Value, option3.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun,
            Maybe<T1> option1,
            Maybe<T2> option2,
            Maybe<T3> option3,
            Maybe<T4> option4)
        {
            Requires.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome && option4.IsSome
                ? Maybe.Create(fun(option1.Value, option2.Value, option3.Value, option4.Value))
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
            Requires.NotNull(fun, "fun");

            return option1.IsSome && option2.IsSome && option3.IsSome
                    && option4.IsSome && option5.IsSome
                ? Maybe.Create(fun(option1.Value, option2.Value, option3.Value,
                    option4.Value, option5.Value))
                : Maybe<TResult>.None;
        }

        #endregion
    }
}
