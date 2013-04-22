namespace Narvalo.Fx.Theory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Resources;

    // Cf. http://www.haskell.org/onlinereport/monad.html
    // Postfix M = function in the Kleisli category.

    public interface Monad<T>
    {
    }

    public interface IMonadDef
    {
        Monad<TSource> Return<TSource>(TSource value);

        Monad<TResult> Bind<TSource, TResult>(
            Monad<TSource> option,
            Func<TSource, Monad<TResult>> fun);

        Monad<TResult> Compose<TSource, TInner, TResult>(
           Monad<TSource> option,
           Func<TSource, Monad<TInner>> funA,
           Func<TInner, Monad<TResult>> funB);
    }

    public interface IMonadExpressions
    {
        Monad<TSource> Join<TSource>(Monad<Monad<TSource>> option);

        Monad<TSource> Filter<TSource>(
            Monad<TSource> option,
            Predicate<TSource> predicate);

        Monad<TResult> Map<TSource, TResult>(
            Monad<TSource> option,
            Func<TSource, TResult> fun);


        Monad<IEnumerable<T>> Filter<T>(
            IEnumerable<T> source,
            Func<T, Monad<bool>> predicate);

        Monad<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> fun);

        Monad<IList<TResult>> Map<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<TResult>> fun);

        Monad<IList<T>> Sequence<T>(IEnumerable<Monad<T>> source);

        Monad<TResult> Lift<TSource, TResult>(
            Func<TSource, TResult> fun,
            Monad<TSource> option);

        Monad<TResult> Lift<T1, T2, TResult>(
            Func<T1, T2, TResult> fun,
            Monad<T1> option1,
            Monad<T2> option2);

        Monad<TResult> Lift<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> fun,
            Monad<T1> option1,
            Monad<T2> option2,
            Monad<T3> option3);

        Monad<TResult> Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun,
            Monad<T1> option1,
            Monad<T2> option2,
            Monad<T3> option3,
            Monad<T4> option4);

        Monad<TResult> Lift<T1, T2, T3, T4, T5, TResult>(
           Func<T1, T2, T3, T4, T5, TResult> fun,
           Monad<T1> option1,
           Monad<T2> option2,
           Monad<T3> option3,
           Monad<T4> option4,
           Monad<T5> option5);
    }

    //public static class MaybeM
    //{
    //    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    //    public static Maybe<T> Join<T>(Maybe<Maybe<T>> option)
    //    {
    //        return option.Bind(_ => _);
    //    }

    //    #region Tableaux.

    //    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    //    public static Maybe<T[]> Filter<T>(
    //       T[] source,
    //       Func<T, Maybe<bool>> predicate)
    //    {
    //        Requires.NotNull(source, "source");
    //        Requires.NotNull(predicate, "predicate");

    //        T[] list = Array.FindAll(
    //            source,
    //            _ => { Maybe<bool> m = predicate(_); return m.IsSome && m.Value; });

    //        return Maybe.Create(list);
    //    }

    //    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    //    public static Maybe<TResult> Fold<TSource, TResult>(
    //        TSource[] source,
    //        TResult seed,
    //        Func<TResult, TSource, Maybe<TResult>> fun)
    //    {
    //        Requires.NotNull(source, "source");
    //        Requires.NotNull(fun, "fun");

    //        Maybe<TResult> option = Maybe.Create(seed);

    //        int length = source.Length;
    //        for (int i = 0; i < length; i++) {
    //            option = option.Bind(_ => fun(_, source[i]));
    //        }

    //        return option;
    //    }

    //    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    //    public static Maybe<TResult[]> Map<TSource, TResult>(
    //        TSource[] source,
    //        Func<TSource, Maybe<TResult>> fun)
    //    {
    //        Requires.NotNull(source, "source");
    //        Requires.NotNull(fun, "fun");

    //        Maybe<TResult>[] options = Array.ConvertAll(source, _ => fun(_));

    //        return Sequence(options);
    //    }

    //    public static Maybe<T[]> Sequence<T>(Maybe<T>[] source)
    //    {
    //        Requires.NotNull(source, "source");

    //        int length = source.Length;
    //        T[] list = new T[length];

    //        for (int i = 0; i < length; i++) {
    //            Maybe<T> m = source[i];
    //            if (m.IsNone) {
    //                return Maybe.None;
    //            }
    //            list[i] = m.Value;
    //        }

    //        return Maybe.Create(list);
    //    }

    //    //public static Maybe<T3[]> Zip<T1, T2, T3>(
    //    //    T1[] list1,
    //    //    T2[] list2,
    //    //    Func<T1, T2, Maybe<T3>> fun)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    #endregion

    //    #region Collections.

    //    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    //    public static Maybe<IEnumerable<T>> Filter<T>(
    //        IEnumerable<T> source,
    //        Func<T, Maybe<bool>> predicate)
    //    {
    //        Requires.NotNull(source, "source");
    //        Requires.NotNull(predicate, "predicate");

    //        var list = from item in source
    //                   let m = predicate(item)
    //                   where m.IsSome && m.Value
    //                   select item;

    //        return Maybe.Create(list);
    //    }

    //    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    //    public static Maybe<TAccumulate> Fold<TSource, TAccumulate>(
    //        IEnumerable<TSource> source,
    //        TAccumulate seed,
    //        Func<TAccumulate, TSource, Maybe<TAccumulate>> fun)
    //    {
    //        Requires.NotNull(source, "source");
    //        Requires.NotNull(fun, "fun");

    //        Maybe<TAccumulate> option = Maybe.Create(seed);

    //        foreach (TSource item in source) {
    //            option = option.Bind(_ => fun(_, item));
    //        }

    //        return option;
    //    }

    //    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    //    public static Maybe<IList<TResult>> Map<TSource, TResult>(
    //        IEnumerable<TSource> source,
    //        Func<TSource, Maybe<TResult>> fun)
    //    {
    //        Requires.NotNull(source, "source");
    //        Requires.NotNull(fun, "fun");

    //        var list = from item in source select fun(item);

    //        return Sequence(list);
    //    }

    //    [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
    //    public static Maybe<IList<T>> Sequence<T>(IEnumerable<Maybe<T>> source)
    //    {
    //        Requires.NotNull(source, "source");

    //        IList<T> list = new List<T>();

    //        foreach (var m in source) {
    //            if (m.IsNone) {
    //                return Maybe.None;
    //            }
    //            list.Add(m.Value);
    //        }

    //        return Maybe.Create(list);
    //    }

    //    //public static Maybe<IEnumerable<T3>> Zip<T1, T2, T3>(
    //    //    IEnumerable<T1> list1,
    //    //    IEnumerable<T2> list2,
    //    //    Func<T1, T2, Maybe<T3>> fun)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    #endregion
}
