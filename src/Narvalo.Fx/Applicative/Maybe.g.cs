﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.42000
// Microsoft.VisualStudio.TextTemplating: 14.0
// </auto-generated>
//------------------------------------------------------------------------------

using _Unit_ = global::Narvalo.Applicative.Unit;

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Internal;
    using Narvalo.Linq;

    // Provides a set of static methods for Maybe<T>.
    // T4: EmitHelpers().
    public static partial class Maybe
    {
        /// <summary>
        /// The unique object of type <c>Maybe&lt;Unit&gt;</c>.
        /// </summary>
        private static readonly Maybe<_Unit_> s_Unit = Of(_Unit_.Default);

        /// <summary>
        /// Gets the unique object of type <c>Maybe&lt;Unit&gt;</c>.
        /// </summary>
        public static Maybe<_Unit_> Unit => s_Unit;

        /// <summary>
        /// Gets the zero for <see cref="Maybe{T}.Bind"/>.
        /// </summary>
        public static Maybe<_Unit_> None => Maybe<_Unit_>.None;

        /// <summary>
        /// Obtains an instance of the <see cref="Maybe{T}"/> class for the specified value.
        /// </summary>
        /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into an object of type <see cref="Maybe{T}"/>.</param>
        /// <returns>An instance of the <see cref="Maybe{T}"/> class for the specified value.</returns>
        public static Maybe<T> Of<T>(T value) => Maybe<T>.η(value);

        public static Maybe<_Unit_> Guard(bool predicate) => predicate ? Unit : None;

        public static Maybe<IEnumerable<TSource>> Repeat<TSource>(
            Maybe<TSource> source,
            int count)
        {
            /* T4: NotNull(source) */
            Require.Range(count >= 0, nameof(count));
            return source.Select(val => Enumerable.Repeat(val, count));
        }

        #region Lift()

        /// <summary>
        /// Promotes a function to use and return <see cref="Maybe{T}" /> values.
        /// </summary>
        /// <seealso cref="Maybe.Select{T, TResult}" />
        public static Func<Maybe<T>, Maybe<TResult>> Lift<T, TResult>(
            Func<T, TResult> func)
            => arg =>
            {
                /* T4: NotNull(arg) */
                return arg.Select(func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Maybe{T}" /> values.
        /// </summary>
        /// <seealso cref="Maybe.Zip{T1, T2, TResult}"/>
        public static Func<Maybe<T1>, Maybe<T2>, Maybe<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> func)
            => (arg1, arg2) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Maybe{T}" /> values.
        /// </summary>
        /// <seealso cref="Maybe.Zip{T1, T2, T3, TResult}"/>
        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
            => (arg1, arg2, arg3) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Maybe{T}" /> values.
        /// </summary>
        /// <seealso cref="Maybe.Zip{T1, T2, T3, T4, TResult}"/>
        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> func)
            => (arg1, arg2, arg3, arg4) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, arg4, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Maybe{T}" /> values.
        /// </summary>
        /// <seealso cref="Maybe.Zip{T1, T2, T3, T4, T5, TResult}"/>
        public static Func<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>, Maybe<T5>, Maybe<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> func)
            => (arg1, arg2, arg3, arg4, arg5) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, arg4, arg5, func);
            };

        #endregion
    }

    // Provides extension methods for Maybe<T>.
    // T4: EmitExtensions().
    public static partial class Maybe
    {
        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> @this)
            => Maybe<T>.μ(@this);

        /// <seealso cref="Ap.Apply{TSource, TResult}(Maybe{Func{TSource, TResult}}, Maybe{TSource})" />
        public static Maybe<TResult> Gather<TSource, TResult>(
            this Maybe<TSource> @this,
            Maybe<Func<TSource, TResult>> applicative)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(applicative) */
            return applicative.Bind(func => @this.Select(func));
        }

        public static Maybe<TResult> ReplaceBy<TSource, TResult>(
            this Maybe<TSource> @this,
            TResult value)
        {
            /* T4: NotNull(@this) */
            return @this.Select(_ => value);
        }

        public static Maybe<TResult> ContinueWith<TSource, TResult>(
            this Maybe<TSource> @this,
            Maybe<TResult> other)
        {
            /* T4: NotNull(@this) */
            return @this.Bind(_ => other);
        }

        public static Maybe<TSource> PassBy<TSource, TOther>(
            this Maybe<TSource> @this,
            Maybe<TOther> other)
        {
            /* T4: NotNull(@this) */
            return @this.Zip(other, (arg, _) => arg);
        }

        public static Maybe<_Unit_> Skip<TSource>(this Maybe<TSource> @this)
        {
            /* T4: NotNull(@this) */
            return @this.ContinueWith(Maybe.Unit);
        }

        #region Zip()

        /// <seealso cref="Maybe.Lift{T1, T2, TResult}"/>
        public static Maybe<TResult> Zip<T1, T2, TResult>(
            this Maybe<T1> @this,
            Maybe<T2> second,
            Func<T1, T2, TResult> zipper)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(second) */
            Require.NotNull(zipper, nameof(zipper));

            return @this.Bind(
                arg1 => second.Select(
                    arg2 => zipper(arg1, arg2)));
        }

        /// <seealso cref="Maybe.Lift{T1, T2, T3, TResult}"/>
        public static Maybe<TResult> Zip<T1, T2, T3, TResult>(
            this Maybe<T1> @this,
            Maybe<T2> second,
            Maybe<T3> third,
            Func<T1, T2, T3, TResult> zipper)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(second) */
            /* T4: NotNull(third) */
            Require.NotNull(zipper, nameof(zipper));

            // This is the same as:
            // > return @this.Bind(
            // >     arg1 => second.Bind(
            // >        arg2 => third.Select(
            // >            arg3 => zipper(arg1, arg2, arg3))));
            // but faster if Zip is locally shadowed.
            return @this.Bind(
                arg1 => second.Zip(
                    third, (arg2, arg3) => zipper(arg1, arg2, arg3)));
        }

        /// <seealso cref="Maybe.Lift{T1, T2, T3, T4, TResult}"/>
        public static Maybe<TResult> Zip<T1, T2, T3, T4, TResult>(
             this Maybe<T1> @this,
             Maybe<T2> second,
             Maybe<T3> third,
             Maybe<T4> fourth,
             Func<T1, T2, T3, T4, TResult> zipper)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(second) */
            /* T4: NotNull(third) */
            /* T4: NotNull(fourth) */
            Require.NotNull(zipper, nameof(zipper));

            // > return @this.Bind(
            // >     arg1 => second.Bind(
            // >         arg2 => third.Bind(
            // >             arg3 => fourth.Select(
            // >                 arg4 => zipper(arg1, arg2, arg3, arg4)))));
            return @this.Bind(
                arg1 => second.Zip(
                    third,
                    fourth,
                    (arg2, arg3, arg4) => zipper(arg1, arg2, arg3, arg4)));
        }

        /// <seealso cref="Maybe.Lift{T1, T2, T3, T4, T5, TResult}"/>
        public static Maybe<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this Maybe<T1> @this,
            Maybe<T2> second,
            Maybe<T3> third,
            Maybe<T4> fourth,
            Maybe<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> zipper)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(second) */
            /* T4: NotNull(third) */
            /* T4: NotNull(fourth) */
            /* T4: NotNull(fifth) */
            Require.NotNull(zipper, nameof(zipper));

            // > return @this.Bind(
            // >     arg1 => second.Bind(
            // >         arg2 => third.Bind(
            // >             arg3 => fourth.Bind(
            // >                 arg4 => fifth.Select(
            // >                     arg5 => zipper(arg1, arg2, arg3, arg4, arg5))))));
            return @this.Bind(
                arg1 => second.Zip(
                    third,
                    fourth,
                    fifth,
                    (arg2, arg3, arg4, arg5) => zipper(arg1, arg2, arg3, arg4, arg5)));
        }

        #endregion

        #region Resource management

        // Bind() with automatic resource management.
        public static Maybe<TResult> Using<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TResult>> selector)
            where TSource : IDisposable
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            return @this.Bind(val => { using (val) { return selector(val); } });
        }

        // Select() with automatic resource management.
        public static Maybe<TResult> Using<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, TResult> selector)
            where TSource : IDisposable
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            return @this.Select(val => { using (val) { return selector(val); } });
        }

        #endregion

        #region Query Expression Pattern

        public static Maybe<TResult> Select<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, TResult> selector)
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            return @this.Bind(val => Maybe<TResult>.η(selector(val)));
        }

        public static Maybe<TSource> Where<TSource>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate)
        {
            /* T4: NotNull(@this) */
            Require.NotNull(predicate, nameof(predicate));
            return @this.Bind(val => predicate(val) ? Maybe<TSource>.η(val) : Maybe<TSource>.None);
        }

        // Generalizes both Bind() and Zip<T1, T2, TResult>().
        public static Maybe<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, Maybe<TMiddle>> selector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(
                val => selector(val).Select(
                    middle => resultSelector(val, middle)));
        }

        public static Maybe<TResult> Join<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
            => Join(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);

        public static Maybe<TResult> Join<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(inner) */
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(comparer, nameof(comparer));

            var keyLookup = GetKeyLookup(
                inner, outerKeySelector, innerKeySelector, comparer ?? EqualityComparer<TKey>.Default);

            return @this.SelectMany(val => keyLookup(val).ContinueWith(inner), resultSelector);
        }

        public static Maybe<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, Maybe<TInner>, TResult> resultSelector)
            => GroupJoin(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);

        public static Maybe<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this Maybe<TSource> @this,
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, Maybe<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(inner) */
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(comparer, nameof(comparer));

            var keyLookup = GetKeyLookup(
                inner, outerKeySelector, innerKeySelector, comparer ?? EqualityComparer<TKey>.Default);

            return @this.Select(val => resultSelector(val, keyLookup(val).ContinueWith(inner)));
        }

        private static Func<TSource, Maybe<TKey>> GetKeyLookup<TSource, TInner, TKey>(
            Maybe<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> comparer)
        {
            Demand.NotNull(outerKeySelector);
            Demand.NotNull(innerKeySelector);
            Demand.NotNull(comparer);

            return arg => inner.Select(innerKeySelector)
                .Where(key => comparer.Equals(key, outerKeySelector(arg)));
        }

        #endregion
    }

    // Provides extension methods for Maybe<Func<TSource, TResult>>.
    // T4: EmitApplicative().
    public static partial class Ap
    {
        /// <seealso cref="Maybe.Gather{TSource, TResult}" />
        public static Maybe<TResult> Apply<TSource, TResult>(
            this Maybe<Func<TSource, TResult>> @this,
            Maybe<TSource> value)
        {
            /* T4: NotNull(value) */
            return value.Gather(@this);
        }
    }

    // Provides extension methods for functions in the Kleisli category.
    // T4: EmitKleisli().
    public static partial class Kleisli
    {
        public static Maybe<IEnumerable<TResult>> InvokeWith<TSource, TResult>(
            this Func<TSource, Maybe<TResult>> @this,
            IEnumerable<TSource> seq)
            => seq.SelectWith(@this);

        public static Maybe<TResult> InvokeWith<TSource, TResult>(
            this Func<TSource, Maybe<TResult>> @this,
            Maybe<TSource> value)
        {
            /* T4: NotNull(value) */
            return value.Bind(@this);
        }

        public static Func<TSource, Maybe<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Maybe<TMiddle>> @this,
            Func<TMiddle, Maybe<TResult>> second)
        {
            Require.NotNull(@this, nameof(@this));
            return arg => @this(arg).Bind(second);
        }

        public static Func<TSource, Maybe<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Maybe<TResult>> @this,
            Func<TSource, Maybe<TMiddle>> second)
        {
            Require.NotNull(second, nameof(second));
            return arg => second(arg).Bind(@this);
        }
    }

    // Provides extension methods for IEnumerable<Maybe<T>>.
    // T4: EmitEnumerableExtensions().
    public static partial class Maybe
    {
        public static Maybe<IEnumerable<TSource>> Collect<TSource>(
            this IEnumerable<Maybe<TSource>> @this)
            => @this.CollectImpl();

        public static Maybe<TSource> Sum<TSource>(this IEnumerable<Maybe<TSource>> @this)
            => @this.SumImpl();
    }
}

namespace Narvalo.Internal
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Applicative;
    using Narvalo.Linq;

    // Provides default implementations for the extension methods for IEnumerable<Maybe<T>>.
    // You will certainly want to override them to improve performance.
    // T4: EmitEnumerableInternal().
    internal static partial class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            return Maybe<IEnumerable<TSource>>.η(CollectIterator(@this));
        }

        private static IEnumerable<TSource> CollectIterator<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Demand.NotNull(source);

            var item = default(TSource);

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var append = false;

                    iter.Current.Bind(
                        val =>
                        {
                            append = true;
                            item = val;

                            return Maybe.Unit;
                        });

                    if (append) { yield return item; }
                }
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<TSource> SumImpl<TSource>(
            this IEnumerable<Maybe<TSource>> @this)
        {
            Demand.NotNull(@this);
            return @this.Aggregate(Maybe<TSource>.None, (m, n) => m.OrElse(n));
        }
    }
}

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;
    using Narvalo.Internal;

    // Provides extension methods for IEnumerable<T>.
    // We do not use the standard LINQ names to avoid any confusion.
    // - Select    -> SelectWith
    // - Where     -> WhereBy
    // - Zip       -> ZipWith
    // - Aggregate -> Reduce or Fold
    // T4: EmitLinqCore().
    public static partial class Qperators
    {
        public static Maybe<IEnumerable<TResult>> SelectWith<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> selector)
            => @this.SelectWithImpl(selector);

        public static Maybe<IEnumerable<TSource>> WhereBy<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicate)
            => @this.WhereByImpl(predicate);

        public static Maybe<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Maybe<TResult>> resultSelector)
            => @this.ZipWithImpl(second, resultSelector);

        public static Maybe<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulator)
            => @this.FoldImpl(seed, accumulator);

        public static Maybe<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulator,
            Func<Maybe<TAccumulate>, bool> predicate)
            => @this.FoldImpl(seed, accumulator, predicate);

        public static Maybe<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Maybe<TSource>> accumulator)
            => @this.ReduceImpl(accumulator);

        public static Maybe<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Maybe<TSource>> accumulator,
            Func<Maybe<TSource>, bool> predicate)
            => @this.ReduceImpl(accumulator, predicate);
    }
}

namespace Narvalo.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Applicative;

    // Provides default implementations for the extension methods for IEnumerable<T>.
    // You will certainly want to override them to improve performance.
    // T4: EmitLinqInternal().
    internal static partial class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<IEnumerable<TResult>> SelectWithImpl<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> selector)
        {
            Demand.NotNull(@this);
            Demand.NotNull(selector);

            return @this.Select(selector).Collect();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<IEnumerable<TSource>> WhereByImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            return Maybe<IEnumerable<TSource>>.η(WhereByIterator(@this, predicate));
        }

        private static IEnumerable<TSource> WhereByIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Maybe<bool>> predicate)
        {
            Demand.NotNull(source);
            Demand.NotNull(predicate);

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    bool pass = false;
                    TSource item = iter.Current;

                    predicate(item).Bind(val =>
                    {
                        pass = val;

                        return Maybe.Unit;
                    });

                    if (pass) { yield return item; }
                }
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<IEnumerable<TResult>> ZipWithImpl<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Maybe<TResult>> resultSelector)
        {
            Demand.NotNull(resultSelector);
            Demand.NotNull(@this);
            Demand.NotNull(second);

            return @this.Zip(second, resultSelector).Collect();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulator)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));

            Maybe<TAccumulate> retval = Maybe<TAccumulate>.η(seed);

            using (var iter = @this.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }
            }

            return retval;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulator,
            Func<Maybe<TAccumulate>, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            Maybe<TAccumulate> retval = Maybe<TAccumulate>.η(seed);

            using (var iter = @this.GetEnumerator())
            {
                while (predicate(retval) && iter.MoveNext())
                {
                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }
            }

            return retval;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Maybe<TSource>> accumulator)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Maybe<TSource> retval = Maybe<TSource>.η(iter.Current);

                while (iter.MoveNext())
                {
                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }

                return retval;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Maybe<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Maybe<TSource>> accumulator,
            Func<Maybe<TSource>, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Maybe<TSource> retval = Maybe<TSource>.η(iter.Current);

                while (predicate(retval) && iter.MoveNext())
                {
                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }

                return retval;
            }
        }
    }
}
