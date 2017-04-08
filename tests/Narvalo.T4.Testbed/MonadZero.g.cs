﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.42000
// Microsoft.VisualStudio.TextTemplating: 15.0
// </auto-generated>
//------------------------------------------------------------------------------

using unit = global::Narvalo.Applicative.Unit;

namespace Narvalo.T4.Testbed
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Narvalo.T4.Testbed.Internal;
    using Narvalo.T4.Testbed.Linq;

    // Provides a set of static methods for MonadZero<T>.
    // T4: EmitHelpers().
    public static partial class MonadZero
    {
        /// <summary>
        /// The unique object of type <c>MonadZero&lt;Unit&gt;</c>.
        /// </summary>
        private static readonly MonadZero<unit> s_Unit = Of(unit.Default);

        /// <summary>
        /// Gets the unique object of type <c>MonadZero&lt;Unit&gt;</c>.
        /// </summary>
        public static MonadZero<unit> Unit => s_Unit;

        /// <summary>
        /// Gets the zero for <see cref="MonadZero{T}.Bind"/>.
        /// </summary>
        public static MonadZero<unit> Zero => MonadZero<unit>.Zero;

        /// <summary>
        /// Obtains an instance of the <see cref="MonadZero{T}"/> class for the specified value.
        /// </summary>
        /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into an object of type <see cref="MonadZero{T}"/>.</param>
        /// <returns>An instance of the <see cref="MonadZero{T}"/> class for the specified value.</returns>
        public static MonadZero<T> Of<T>(T value) => MonadZero<T>.η(value);

        public static MonadZero<unit> Guard(bool predicate) => predicate ? Unit : Zero;

        public static MonadZero<IEnumerable<TSource>> Repeat<TSource>(
            MonadZero<TSource> source,
            int count)
        {
            Require.NotNull(source, nameof(source));
            Require.Range(count >= 0, nameof(count));
            return source.Select(val => Enumerable.Repeat(val, count));
        }

        #region Lift()

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadZero{T}" /> values.
        /// </summary>
        /// <seealso cref="MonadZero.Select{T, TResult}" />
        public static Func<MonadZero<T>, MonadZero<TResult>> Lift<T, TResult>(
            Func<T, TResult> func)
            => arg =>
            {
                Require.NotNull(arg, nameof(arg));
                return arg.Select(func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadZero{T}" /> values.
        /// </summary>
        /// <seealso cref="MonadZero.Zip{T1, T2, TResult}"/>
        public static Func<MonadZero<T1>, MonadZero<T2>, MonadZero<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> func)
            => (arg1, arg2) =>
            {
                Require.NotNull(arg1, nameof(arg1));
                return arg1.Zip(arg2, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadZero{T}" /> values.
        /// </summary>
        /// <seealso cref="MonadZero.Zip{T1, T2, T3, TResult}"/>
        public static Func<MonadZero<T1>, MonadZero<T2>, MonadZero<T3>, MonadZero<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
            => (arg1, arg2, arg3) =>
            {
                Require.NotNull(arg1, nameof(arg1));
                return arg1.Zip(arg2, arg3, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadZero{T}" /> values.
        /// </summary>
        /// <seealso cref="MonadZero.Zip{T1, T2, T3, T4, TResult}"/>
        public static Func<MonadZero<T1>, MonadZero<T2>, MonadZero<T3>, MonadZero<T4>, MonadZero<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> func)
            => (arg1, arg2, arg3, arg4) =>
            {
                Require.NotNull(arg1, nameof(arg1));
                return arg1.Zip(arg2, arg3, arg4, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="MonadZero{T}" /> values.
        /// </summary>
        /// <seealso cref="MonadZero.Zip{T1, T2, T3, T4, T5, TResult}"/>
        public static Func<MonadZero<T1>, MonadZero<T2>, MonadZero<T3>, MonadZero<T4>, MonadZero<T5>, MonadZero<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> func)
            => (arg1, arg2, arg3, arg4, arg5) =>
            {
                Require.NotNull(arg1, nameof(arg1));
                return arg1.Zip(arg2, arg3, arg4, arg5, func);
            };

        #endregion
    }

    // Provides extension methods for MonadZero<T>.
    // T4: EmitExtensions().
    public static partial class MonadZero
    {
        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        public static MonadZero<T> Flatten<T>(this MonadZero<MonadZero<T>> @this)
            => MonadZero<T>.μ(@this);

        /// <seealso cref="Ap.Apply{TSource, TResult}(MonadZero{Func{TSource, TResult}}, MonadZero{TSource})" />
        public static MonadZero<TResult> Gather<TSource, TResult>(
            this MonadZero<TSource> @this,
            MonadZero<Func<TSource, TResult>> applicative)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(applicative, nameof(applicative));
            return applicative.Bind(func => @this.Select(func));
        }

        public static MonadZero<TResult> ReplaceBy<TSource, TResult>(
            this MonadZero<TSource> @this,
            TResult value)
        {
            Require.NotNull(@this, nameof(@this));
            return @this.Select(_ => value);
        }

        public static MonadZero<TResult> ContinueWith<TSource, TResult>(
            this MonadZero<TSource> @this,
            MonadZero<TResult> other)
        {
            Require.NotNull(@this, nameof(@this));
            return @this.Bind(_ => other);
        }

        public static MonadZero<TSource> PassBy<TSource, TOther>(
            this MonadZero<TSource> @this,
            MonadZero<TOther> other)
        {
            Require.NotNull(@this, nameof(@this));
            return @this.Zip(other, (arg, _) => arg);
        }

        public static MonadZero<unit> Skip<TSource>(this MonadZero<TSource> @this)
        {
            Require.NotNull(@this, nameof(@this));
            return @this.ContinueWith(MonadZero.Unit);
        }

        #region Zip()

        /// <seealso cref="MonadZero.Lift{T1, T2, TResult}"/>
        public static MonadZero<TResult> Zip<T1, T2, TResult>(
            this MonadZero<T1> @this,
            MonadZero<T2> second,
            Func<T1, T2, TResult> zipper)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(zipper, nameof(zipper));

            return @this.Bind(
                arg1 => second.Select(
                    arg2 => zipper(arg1, arg2)));
        }

        /// <seealso cref="MonadZero.Lift{T1, T2, T3, TResult}"/>
        public static MonadZero<TResult> Zip<T1, T2, T3, TResult>(
            this MonadZero<T1> @this,
            MonadZero<T2> second,
            MonadZero<T3> third,
            Func<T1, T2, T3, TResult> zipper)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(third, nameof(third));
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

        /// <seealso cref="MonadZero.Lift{T1, T2, T3, T4, TResult}"/>
        public static MonadZero<TResult> Zip<T1, T2, T3, T4, TResult>(
             this MonadZero<T1> @this,
             MonadZero<T2> second,
             MonadZero<T3> third,
             MonadZero<T4> fourth,
             Func<T1, T2, T3, T4, TResult> zipper)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(third, nameof(third));
            Require.NotNull(fourth, nameof(fourth));
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

        /// <seealso cref="MonadZero.Lift{T1, T2, T3, T4, T5, TResult}"/>
        public static MonadZero<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this MonadZero<T1> @this,
            MonadZero<T2> second,
            MonadZero<T3> third,
            MonadZero<T4> fourth,
            MonadZero<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> zipper)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(third, nameof(third));
            Require.NotNull(fourth, nameof(fourth));
            Require.NotNull(fifth, nameof(fifth));
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
        public static MonadZero<TResult> Using<TSource, TResult>(
            this MonadZero<TSource> @this,
            Func<TSource, MonadZero<TResult>> binder)
            where TSource : IDisposable
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(binder, nameof(binder));
            return @this.Bind(val => { using (val) { return binder(val); } });
        }

        // Select() with automatic resource management.
        public static MonadZero<TResult> Using<TSource, TResult>(
            this MonadZero<TSource> @this,
            Func<TSource, TResult> selector)
            where TSource : IDisposable
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            return @this.Select(val => { using (val) { return selector(val); } });
        }

        #endregion

        #region Query Expression Pattern

        public static MonadZero<TResult> Select<TSource, TResult>(
            this MonadZero<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            return @this.Bind(val => MonadZero<TResult>.η(selector(val)));
        }

        public static MonadZero<TSource> Where<TSource>(
            this MonadZero<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            return @this.Bind(val => predicate(val) ? MonadZero<TSource>.η(val) : MonadZero<TSource>.Zero);
        }

        // Generalizes both Bind() and Zip<T1, T2, TResult>().
        public static MonadZero<TResult> SelectMany<TSource, TMiddle, TResult>(
            this MonadZero<TSource> @this,
            Func<TSource, MonadZero<TMiddle>> selector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(
                val => selector(val).Select(
                    middle => resultSelector(val, middle)));
        }

        public static MonadZero<TResult> Join<TSource, TInner, TKey, TResult>(
            this MonadZero<TSource> @this,
            MonadZero<TInner> inner,
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

        public static MonadZero<TResult> Join<TSource, TInner, TKey, TResult>(
            this MonadZero<TSource> @this,
            MonadZero<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(inner, nameof(inner));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(comparer, nameof(comparer));

            var lookup = GetKeyLookup(inner, innerKeySelector, comparer);
            Func<TSource, MonadZero<TInner>> valueSelector = outer => lookup(outerKeySelector(outer));

            return @this.SelectMany(valueSelector, resultSelector);
        }

        public static MonadZero<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this MonadZero<TSource> @this,
            MonadZero<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, MonadZero<TInner>, TResult> resultSelector)
            => GroupJoin(
                @this,
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                EqualityComparer<TKey>.Default);

        public static MonadZero<TResult> GroupJoin<TSource, TInner, TKey, TResult>(
            this MonadZero<TSource> @this,
            MonadZero<TInner> inner,
            Func<TSource, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TSource, MonadZero<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(inner, nameof(inner));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(comparer, nameof(comparer));

            var lookup = GetKeyLookup(inner, innerKeySelector, comparer);
            Func<TSource, MonadZero<TInner>> selector = outer => lookup(outerKeySelector(outer));

            return @this.Select(outer => resultSelector(outer, selector(outer)));
        }

        private static Func<TKey, MonadZero<TInner>> GetKeyLookup<TInner, TKey>(
            MonadZero<TInner> inner,
            Func<TInner, TKey> innerKeySelector,
            IEqualityComparer<TKey> comparer)
        {
            Debug.Assert(inner != null);
            Debug.Assert(innerKeySelector != null);
            Debug.Assert(comparer != null);

            return outerKey =>
                inner.Select(innerKeySelector)
                    .Where(innerKey => comparer.Equals(innerKey, outerKey))
                    .ContinueWith(inner);
        }

        #endregion
    }

    // Provides extension methods for MonadZero<Func<TSource, TResult>>.
    // T4: EmitApplicative().
    public static partial class Ap
    {
        /// <seealso cref="MonadZero.Gather{TSource, TResult}" />
        public static MonadZero<TResult> Apply<TSource, TResult>(
            this MonadZero<Func<TSource, TResult>> @this,
            MonadZero<TSource> value)
        {
            Require.NotNull(value, nameof(value));
            return value.Gather(@this);
        }
    }

    // Provides extension methods for functions in the Kleisli category.
    // T4: EmitKleisli().
    public static partial class Kleisli
    {
        public static MonadZero<IEnumerable<TResult>> InvokeWith<TSource, TResult>(
            this Func<TSource, MonadZero<TResult>> @this,
            IEnumerable<TSource> seq)
            => seq.Select(@this).Collect();

        public static MonadZero<TResult> InvokeWith<TSource, TResult>(
            this Func<TSource, MonadZero<TResult>> @this,
            MonadZero<TSource> value)
        {
            Require.NotNull(value, nameof(value));
            return value.Bind(@this);
        }

        public static Func<TSource, MonadZero<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, MonadZero<TMiddle>> @this,
            Func<TMiddle, MonadZero<TResult>> second)
        {
            Require.NotNull(@this, nameof(@this));
            return arg => @this(arg).Bind(second);
        }

        public static Func<TSource, MonadZero<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, MonadZero<TResult>> @this,
            Func<TSource, MonadZero<TMiddle>> second)
        {
            Require.NotNull(second, nameof(second));
            return arg => second(arg).Bind(@this);
        }
    }

    // Provides extension methods for IEnumerable<MonadZero<T>>.
    // T4: EmitEnumerableExtensions().
    public static partial class MonadZero
    {
        public static IEnumerable<TSource> CollectAny<TSource>(
            this IEnumerable<MonadZero<TSource>> source)
        {
            Require.NotNull(source, nameof(source));
            return source.CollectAnyImpl();
        }

        // Hidden because this operator is not composable.
        // Do not disable, we use it in Kleisli.InvokeWith().
        internal static MonadZero<IEnumerable<TSource>> Collect<TSource>(
            this IEnumerable<MonadZero<TSource>> source)
        {
            Require.NotNull(source, nameof(source));
            return source.CollectImpl();
        }
    }
}

namespace Narvalo.T4.Testbed.Internal
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.T4.Testbed;

    // Provides default implementations for the extension methods for IEnumerable<MonadZero<T>>.
    // You will certainly want to override them to improve performance.
    // T4: EmitEnumerableInternal().
    internal static partial class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static IEnumerable<TSource> CollectAnyImpl<TSource>(
            this IEnumerable<MonadZero<TSource>> source)
        {
            Debug.Assert(source != null);

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

                            return MonadZero.Unit;
                        });

                    if (append) { yield return item; }
                }
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static MonadZero<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<MonadZero<TSource>> source)
        {
            Debug.Assert(source != null);
            return MonadZero<IEnumerable<TSource>>.η(CollectAnyImpl(source));
        }
    }
}

namespace Narvalo.T4.Testbed.Linq
{
    using System;
    using System.Collections.Generic;

    using Narvalo.T4.Testbed;
    using Narvalo.T4.Testbed.Internal;

    // Provides extension methods for IEnumerable<T>.
    // We do not use the standard LINQ names to avoid any confusion.
    // - Select    -> SelectWith
    // - Where     -> WhereBy
    // - Zip       -> ZipWith
    // - Aggregate -> Reduce or Fold
    // T4: EmitLinqCore().
    public static partial class Qperators
    {
        public static IEnumerable<TSource> WhereAny<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, MonadZero<bool>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));
            return source.WhereAnyImpl(predicate);
        }

        //
        // Disabled because these operators are not composable.
        //
        //
        //internal static MonadZero<IEnumerable<TSource>> WhereBy<TSource>(
        //    this IEnumerable<TSource> source,
        //    Func<TSource, MonadZero<bool>> predicate)
        //{
        //    Require.NotNull(source, nameof(source));
        //    Require.NotNull(predicate, nameof(predicate));
        //    return source.WhereByImpl(predicate);
        //}
        //
        //internal static MonadZero<IEnumerable<TResult>> SelectWith<TSource, TResult>(
        //    this IEnumerable<TSource> source,
        //    Func<TSource, MonadZero<TResult>> selector)
        //    => source.SelectWithImpl(selector);
        //
        //internal static MonadZero<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
        //    this IEnumerable<TFirst> source,
        //    IEnumerable<TSecond> second,
        //    Func<TFirst, TSecond, MonadZero<TResult>> resultSelector)
        //    => source.ZipWithImpl(second, resultSelector);
        //

        public static MonadZero<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadZero<TAccumulate>> accumulator)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));
            return source.FoldImpl(seed, accumulator);
        }

        public static MonadZero<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadZero<TAccumulate>> accumulator,
            Func<MonadZero<TAccumulate>, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));
            return source.FoldImpl(seed, accumulator, predicate);
        }

        public static MonadZero<TSource> Reduce<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, MonadZero<TSource>> accumulator)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));
            return source.ReduceImpl(accumulator);
        }

        public static MonadZero<TSource> Reduce<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, MonadZero<TSource>> accumulator,
            Func<MonadZero<TSource>, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));
            return source.ReduceImpl(accumulator, predicate);
        }
    }
}

namespace Narvalo.T4.Testbed.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.T4.Testbed;

    // Provides default implementations for the extension methods for IEnumerable<T>.
    // You will certainly want to override them to improve performance.
    // T4: EmitLinqInternal().
    internal static partial class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static IEnumerable<TSource> WhereAnyImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, MonadZero<bool>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    bool pass = false;
                    TSource item = iter.Current;

                    predicate(item).Bind(val =>
                    {
                        pass = val;

                        return MonadZero.Unit;
                    });

                    if (pass) { yield return item; }
                }
            }
        }

        //
        // Parent operators are disabled because they are not composable.
        //
        //
        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        //internal static MonadZero<IEnumerable<TSource>> WhereByImpl<TSource>(
        //    this IEnumerable<TSource> source,
        //    Func<TSource, MonadZero<bool>> predicate)
        //{
        //    Debug.Assert(source != null);
        //    Debug.Assert(predicate != null);
        //
        //    return MonadZero<IEnumerable<TSource>>.η(WhereAnyImpl(source, predicate));
        //}
        //
        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        //internal static MonadZero<IEnumerable<TResult>> SelectWithImpl<TSource, TResult>(
        //    this IEnumerable<TSource> source,
        //    Func<TSource, MonadZero<TResult>> selector)
        //    => source.Select(selector).Collect();
        //
        //[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        //internal static MonadZero<IEnumerable<TResult>> ZipWithImpl<TFirst, TSecond, TResult>(
        //    this IEnumerable<TFirst> source,
        //    IEnumerable<TSecond> second,
        //    Func<TFirst, TSecond, MonadZero<TResult>> resultSelector)
        //    => source.Zip(second, resultSelector).Collect();
        //

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static MonadZero<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadZero<TAccumulate>> accumulator)
        {
            Debug.Assert(source != null);
            Debug.Assert(accumulator != null);

            MonadZero<TAccumulate> retval = MonadZero<TAccumulate>.η(seed);

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    if (retval == null) { continue; }

                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }
            }

            return retval;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static MonadZero<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, MonadZero<TAccumulate>> accumulator,
            Func<MonadZero<TAccumulate>, bool> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(accumulator != null);
            Debug.Assert(predicate != null);

            MonadZero<TAccumulate> retval = MonadZero<TAccumulate>.η(seed);

            using (var iter = source.GetEnumerator())
            {
                while (predicate(retval) && iter.MoveNext())
                {
                    if (retval == null) { continue; }

                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }
            }

            return retval;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static MonadZero<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, MonadZero<TSource>> accumulator)
        {
            Debug.Assert(source != null);
            Debug.Assert(accumulator != null);

            using (var iter = source.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                MonadZero<TSource> retval = MonadZero<TSource>.η(iter.Current);

                while (iter.MoveNext())
                {
                    if (retval == null) { continue; }

                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }

                return retval;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static MonadZero<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, MonadZero<TSource>> accumulator,
            Func<MonadZero<TSource>, bool> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(accumulator != null);
            Debug.Assert(predicate != null);

            using (var iter = source.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                MonadZero<TSource> retval = MonadZero<TSource>.η(iter.Current);

                while (predicate(retval) && iter.MoveNext())
                {
                    if (retval == null) { continue; }

                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }

                return retval;
            }
        }
    }
}

