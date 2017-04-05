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

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Narvalo.Internal;
    using Narvalo.Linq;

    // Provides a set of static methods for Result<T, TError>.
    // T4: EmitHelpers().
    public static partial class Result
    {
        public static Result<IEnumerable<TSource>, TError> Repeat<TSource, TError>(
            Result<TSource, TError> source,
            int count)
        {
            /* T4: NotNull(source) */
            Require.Range(count >= 0, nameof(count));
            return source.Select(val => Enumerable.Repeat(val, count));
        }

        #region Lift()

        /// <summary>
        /// Promotes a function to use and return <see cref="Result{T, TError}" /> values.
        /// </summary>
        /// <seealso cref="Result.Select{T, TResult, TError}" />
        public static Func<Result<T, TError>, Result<TResult, TError>> Lift<T, TResult, TError>(
            Func<T, TResult> func)
            => arg =>
            {
                /* T4: NotNull(arg) */
                return arg.Select(func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Result{T, TError}" /> values.
        /// </summary>
        /// <seealso cref="Result.Zip{T1, T2, TResult, TError}"/>
        public static Func<Result<T1, TError>, Result<T2, TError>, Result<TResult, TError>>
            Lift<T1, T2, TResult, TError>(Func<T1, T2, TResult> func)
            => (arg1, arg2) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Result{T, TError}" /> values.
        /// </summary>
        /// <seealso cref="Result.Zip{T1, T2, T3, TResult, TError}"/>
        public static Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<TResult, TError>>
            Lift<T1, T2, T3, TResult, TError>(Func<T1, T2, T3, TResult> func)
            => (arg1, arg2, arg3) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Result{T, TError}" /> values.
        /// </summary>
        /// <seealso cref="Result.Zip{T1, T2, T3, T4, TResult, TError}"/>
        public static Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<TResult, TError>>
            Lift<T1, T2, T3, T4, TResult, TError>(
            Func<T1, T2, T3, T4, TResult> func)
            => (arg1, arg2, arg3, arg4) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, arg4, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Result{T, TError}" /> values.
        /// </summary>
        /// <seealso cref="Result.Zip{T1, T2, T3, T4, T5, TResult, TError}"/>
        public static Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<TResult, TError>>
            Lift<T1, T2, T3, T4, T5, TResult, TError>(
            Func<T1, T2, T3, T4, T5, TResult> func)
            => (arg1, arg2, arg3, arg4, arg5) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, arg4, arg5, func);
            };

        #endregion
    }

    // Provides extension methods for Result<T, TError>.
    // T4: EmitExtensions().
    public static partial class Result
    {
        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        public static Result<T, TError> Flatten<T, TError>(this Result<Result<T, TError>, TError> @this)
            => Result<T, TError>.μ(@this);

        /// <seealso cref="Ap.Apply{TSource, TResult, TError}(Result{Func{TSource, TResult}, TError}, Result{TSource, TError})" />
        public static Result<TResult, TError> Gather<TSource, TResult, TError>(
            this Result<TSource, TError> @this,
            Result<Func<TSource, TResult>, TError> applicative)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(applicative) */
            return applicative.Bind(func => @this.Select(func));
        }

        public static Result<TResult, TError> ReplaceBy<TSource, TResult, TError>(
            this Result<TSource, TError> @this,
            TResult value)
        {
            /* T4: NotNull(@this) */
            return @this.Select(_ => value);
        }

        public static Result<TResult, TError> ContinueWith<TSource, TResult, TError>(
            this Result<TSource, TError> @this,
            Result<TResult, TError> other)
        {
            /* T4: NotNull(@this) */
            return @this.Bind(_ => other);
        }

        public static Result<TSource, TError> PassBy<TSource, TOther, TError>(
            this Result<TSource, TError> @this,
            Result<TOther, TError> other)
        {
            /* T4: NotNull(@this) */
            return @this.Zip(other, (arg, _) => arg);
        }

        public static Result<unit, TError> Skip<TSource, TError>(this Result<TSource, TError> @this)
        {
            /* T4: NotNull(@this) */
            return @this.ReplaceBy(unit.Default);
        }

        #region Zip()

        /// <seealso cref="Result.Lift{T1, T2, TResult, TError}"/>
        public static Result<TResult, TError> Zip<T1, T2, TResult, TError>(
            this Result<T1, TError> @this,
            Result<T2, TError> second,
            Func<T1, T2, TResult> zipper)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(second) */
            Require.NotNull(zipper, nameof(zipper));

            return @this.Bind(
                arg1 => second.Select(
                    arg2 => zipper(arg1, arg2)));
        }

        /// <seealso cref="Result.Lift{T1, T2, T3, TResult, TError}"/>
        public static Result<TResult, TError> Zip<T1, T2, T3, TResult, TError>(
            this Result<T1, TError> @this,
            Result<T2, TError> second,
            Result<T3, TError> third,
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

        /// <seealso cref="Result.Lift{T1, T2, T3, T4, TResult, TError}"/>
        public static Result<TResult, TError> Zip<T1, T2, T3, T4, TResult, TError>(
             this Result<T1, TError> @this,
             Result<T2, TError> second,
             Result<T3, TError> third,
             Result<T4, TError> fourth,
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

        /// <seealso cref="Result.Lift{T1, T2, T3, T4, T5, TResult, TError}"/>
        public static Result<TResult, TError> Zip<T1, T2, T3, T4, T5, TResult, TError>(
            this Result<T1, TError> @this,
            Result<T2, TError> second,
            Result<T3, TError> third,
            Result<T4, TError> fourth,
            Result<T5, TError> fifth,
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
        public static Result<TResult, TError> Using<TSource, TResult, TError>(
            this Result<TSource, TError> @this,
            Func<TSource, Result<TResult, TError>> binder)
            where TSource : IDisposable
        {
            /* T4: NotNull(@this) */
            Require.NotNull(binder, nameof(binder));
            return @this.Bind(val => { using (val) { return binder(val); } });
        }

        // Select() with automatic resource management.
        public static Result<TResult, TError> Using<TSource, TResult, TError>(
            this Result<TSource, TError> @this,
            Func<TSource, TResult> selector)
            where TSource : IDisposable
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            return @this.Select(val => { using (val) { return selector(val); } });
        }

        #endregion

        #region Query Expression Pattern

        public static Result<TResult, TError> Select<TSource, TResult, TError>(
            this Result<TSource, TError> @this,
            Func<TSource, TResult> selector)
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            return @this.Bind(val => Result<TResult, TError>.Of(selector(val)));
        }

        // Generalizes both Bind() and Zip<T1, T2, TResult>().
        public static Result<TResult, TError> SelectMany<TSource, TMiddle, TResult, TError>(
            this Result<TSource, TError> @this,
            Func<TSource, Result<TMiddle, TError>> selector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(
                val => selector(val).Select(
                    middle => resultSelector(val, middle)));
        }

        #endregion
    }

    // Provides extension methods for Result<Func<TSource, TResult>, TError>.
    // T4: EmitApplicative().
    public static partial class Ap
    {
        /// <seealso cref="Result.Gather{TSource, TResult, TError}" />
        public static Result<TResult, TError> Apply<TSource, TResult, TError>(
            this Result<Func<TSource, TResult>, TError> @this,
            Result<TSource, TError> value)
        {
            /* T4: NotNull(value) */
            return value.Gather(@this);
        }
    }

    // Provides extension methods for functions in the Kleisli category.
    // T4: EmitKleisli().
    public static partial class Kleisli
    {
        public static Result<IEnumerable<TResult>, TError> InvokeWith<TSource, TResult, TError>(
            this Func<TSource, Result<TResult, TError>> @this,
            IEnumerable<TSource> seq)
            => seq.SelectWith(@this);

        public static Result<TResult, TError> InvokeWith<TSource, TResult, TError>(
            this Func<TSource, Result<TResult, TError>> @this,
            Result<TSource, TError> value)
        {
            /* T4: NotNull(value) */
            return value.Bind(@this);
        }

        public static Func<TSource, Result<TResult, TError>> Compose<TSource, TMiddle, TResult, TError>(
            this Func<TSource, Result<TMiddle, TError>> @this,
            Func<TMiddle, Result<TResult, TError>> second)
        {
            Require.NotNull(@this, nameof(@this));
            return arg => @this(arg).Bind(second);
        }

        public static Func<TSource, Result<TResult, TError>> ComposeBack<TSource, TMiddle, TResult, TError>(
            this Func<TMiddle, Result<TResult, TError>> @this,
            Func<TSource, Result<TMiddle, TError>> second)
        {
            Require.NotNull(second, nameof(second));
            return arg => second(arg).Bind(@this);
        }
    }

    // Provides extension methods for IEnumerable<Result<T, TError>>.
    // T4: EmitEnumerableExtensions().
    public static partial class Result
    {
        public static Result<IEnumerable<TSource>, TError> Collect<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> source)
            => source.CollectImpl();
    }
}

namespace Narvalo.Internal
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Applicative;

    // Provides default implementations for the extension methods for IEnumerable<Result<T, TError>>.
    // You will certainly want to override them to improve performance.
    // T4: EmitEnumerableInternal().
    internal static partial class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Result<IEnumerable<TSource>, TError> CollectImpl<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> source)
        {
            Require.NotNull(source, nameof(source));
            return Result<IEnumerable<TSource>, TError>.Of(CollectIterator(source));
        }

        private static IEnumerable<TSource> CollectIterator<TSource, TError>(IEnumerable<Result<TSource, TError>> source)
        {
            Debug.Assert(source != null);

            var unit = Result<Unit, TError>.Of(Unit.Default);
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

                            return unit;
                        });

                    if (append) { yield return item; }
                }
            }
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
        public static Result<IEnumerable<TResult>, TError> SelectWith<TSource, TResult, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<TResult, TError>> selector)
            => source.SelectWithImpl(selector);

        public static Result<IEnumerable<TSource>, TError> WhereBy<TSource, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<bool, TError>> predicate)
            => source.WhereByImpl(predicate);

        public static Result<IEnumerable<TResult>, TError> ZipWith<TFirst, TSecond, TResult, TError>(
            this IEnumerable<TFirst> source,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Result<TResult, TError>> resultSelector)
            => source.ZipWithImpl(second, resultSelector);

        public static Result<TAccumulate, TError> Fold<TSource, TAccumulate, TError>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Result<TAccumulate, TError>> accumulator)
            => source.FoldImpl(seed, accumulator);

        public static Result<TAccumulate, TError> Fold<TSource, TAccumulate, TError>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Result<TAccumulate, TError>> accumulator,
            Func<Result<TAccumulate, TError>, bool> predicate)
            => source.FoldImpl(seed, accumulator, predicate);

        public static Result<TSource, TError> Reduce<TSource, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, Result<TSource, TError>> accumulator)
            => source.ReduceImpl(accumulator);

        public static Result<TSource, TError> Reduce<TSource, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, Result<TSource, TError>> accumulator,
            Func<Result<TSource, TError>, bool> predicate)
            => source.ReduceImpl(accumulator, predicate);
    }
}

namespace Narvalo.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Applicative;

    // Provides default implementations for the extension methods for IEnumerable<T>.
    // You will certainly want to override them to improve performance.
    // T4: EmitLinqInternal().
    internal static partial class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Result<IEnumerable<TResult>, TError> SelectWithImpl<TSource, TResult, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<TResult, TError>> selector)
        {
            Debug.Assert(source != null);
            Debug.Assert(selector != null);

            return source.Select(selector).Collect();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Result<IEnumerable<TSource>, TError> WhereByImpl<TSource, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, Result<bool, TError>> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(predicate, nameof(predicate));

            return Result<IEnumerable<TSource>, TError>.Of(WhereByIterator(source, predicate));
        }

        private static IEnumerable<TSource> WhereByIterator<TSource, TError>(
            IEnumerable<TSource> source,
            Func<TSource, Result<bool, TError>> predicate)
        {
            Debug.Assert(source != null);
            Debug.Assert(predicate != null);

            var unit = Result<Unit, TError>.Of(Unit.Default);

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    bool pass = false;
                    TSource item = iter.Current;

                    predicate(item).Bind(val =>
                    {
                        pass = val;

                        return unit;
                    });

                    if (pass) { yield return item; }
                }
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Result<IEnumerable<TResult>, TError> ZipWithImpl<TFirst, TSecond, TResult, TError>(
            this IEnumerable<TFirst> source,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Result<TResult, TError>> resultSelector)
        {
            Debug.Assert(resultSelector != null);
            Debug.Assert(source != null);
            Debug.Assert(second != null);

            return source.Zip(second, resultSelector).Collect();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Result<TAccumulate, TError> FoldImpl<TSource, TAccumulate, TError>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Result<TAccumulate, TError>> accumulator)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));

            Result<TAccumulate, TError> retval = Result<TAccumulate, TError>.Of(seed);

            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }
            }

            return retval;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Result<TAccumulate, TError> FoldImpl<TSource, TAccumulate, TError>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Result<TAccumulate, TError>> accumulator,
            Func<Result<TAccumulate, TError>, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            Result<TAccumulate, TError> retval = Result<TAccumulate, TError>.Of(seed);

            using (var iter = source.GetEnumerator())
            {
                while (predicate(retval) && iter.MoveNext())
                {
                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }
            }

            return retval;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Result<TSource, TError> ReduceImpl<TSource, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, Result<TSource, TError>> accumulator)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));

            using (var iter = source.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Result<TSource, TError> retval = Result<TSource, TError>.Of(iter.Current);

                while (iter.MoveNext())
                {
                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }

                return retval;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Result<TSource, TError> ReduceImpl<TSource, TError>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, Result<TSource, TError>> accumulator,
            Func<Result<TSource, TError>, bool> predicate)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            using (var iter = source.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Result<TSource, TError> retval = Result<TSource, TError>.Of(iter.Current);

                while (predicate(retval) && iter.MoveNext())
                {
                    retval = retval.Bind(val => accumulator(val, iter.Current));
                }

                return retval;
            }
        }
    }
}
