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

    // Provides a set of static methods for Ident<T>.
    // T4: EmitHelpers().
    public static partial class Ident
    {
        /// <summary>
        /// The unique object of type <c>Ident&lt;Unit&gt;</c>.
        /// </summary>
        private static readonly Ident<_Unit_> s_Unit = Of(_Unit_.Default);

        /// <summary>
        /// Gets the unique object of type <c>Ident&lt;Unit&gt;</c>.
        /// </summary>
        public static Ident<_Unit_> Unit => s_Unit;

        /// <summary>
        /// Obtains an instance of the <see cref="Ident{T}"/> class for the specified value.
        /// </summary>
        /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into an object of type <see cref="Ident{T}"/>.</param>
        /// <returns>An instance of the <see cref="Ident{T}"/> class for the specified value.</returns>
        public static Ident<T> Of<T>(T value)
            => Ident<T>.η(value);

        public static Ident<IEnumerable<TSource>> Repeat<TSource>(
            Ident<TSource> source,
            int count)
        {
            /* T4: NotNull(source) */
            Require.Range(count >= 0, nameof(count));
            return source.Select(val => Enumerable.Repeat(val, count));
        }

        #region Lift()

        /// <summary>
        /// Promotes a function to use and return <see cref="Ident{T}" /> values.
        /// </summary>
        /// <seealso cref="Ident.Select{T, TResult}" />
        public static Func<Ident<T>, Ident<TResult>> Lift<T, TResult>(
            Func<T, TResult> func)
            => arg =>
            {
                /* T4: NotNull(arg) */
                return arg.Select(func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Ident{T}" /> values.
        /// </summary>
        /// <seealso cref="Ident.Zip{T1, T2, TResult}(Ident{T1}, Ident{T2}, Func{T1, T2, TResult})"/>
        public static Func<Ident<T1>, Ident<T2>, Ident<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> func)
            => (arg1, arg2) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Ident{T}" /> values.
        /// </summary>
        /// <seealso cref="Ident.Zip{T1, T2, T3, TResult}(Ident{T1}, Ident{T2}, Ident{T3}, Func{T1, T2, T3, TResult})"/>
        public static Func<Ident<T1>, Ident<T2>, Ident<T3>, Ident<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
            => (arg1, arg2, arg3) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Ident{T}" /> values.
        /// </summary>
        /// <seealso cref="Ident.Zip{T1, T2, T3, T4, TResult}(Ident{T1}, Ident{T2}, Ident{T3}, Ident{T4}, Func{T1, T2, T3, T4, TResult})"/>
        public static Func<Ident<T1>, Ident<T2>, Ident<T3>, Ident<T4>, Ident<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> func)
            => (arg1, arg2, arg3, arg4) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, arg4, func);
            };

        /// <summary>
        /// Promotes a function to use and return <see cref="Ident{T}" /> values.
        /// </summary>
        /// <seealso cref="Ident.Zip{T1, T2, T3, T4, T5, TResult}(Ident{T1}, Ident{T2}, Ident{T3}, Ident{T4}, Ident{T5},Func{T1, T2, T3, T4, T5, TResult})"/>
        public static Func<Ident<T1>, Ident<T2>, Ident<T3>, Ident<T4>, Ident<T5>, Ident<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> func)
            => (arg1, arg2, arg3, arg4, arg5) =>
            {
                /* T4: NotNull(arg1) */
                return arg1.Zip(arg2, arg3, arg4, arg5, func);
            };

        #endregion
    }

    // Provides extension methods for Ident<T>.
    // T4: EmitExtensions().
    public static partial class Ident
    {
        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        public static Ident<T> Flatten<T>(this Ident<Ident<T>> @this)
            => Ident<T>.μ(@this);

        /// <seealso cref="Ap.Apply{TSource, TResult}(Ident{Func{TSource, TResult}}, Ident{TSource})" />
        public static Ident<TResult> Gather<TSource, TResult>(
            this Ident<TSource> @this,
            Ident<Func<TSource, TResult>> applicative)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(applicative) */
            return applicative.Bind(func => @this.Select(func));
        }

        public static Ident<TResult> ReplaceBy<TSource, TResult>(
            this Ident<TSource> @this,
            TResult value)
        {
            /* T4: NotNull(@this) */
            return @this.Select(_ => value);
        }

        public static Ident<TResult> ContinueWith<TSource, TResult>(
            this Ident<TSource> @this,
            Ident<TResult> other)
        {
            /* T4: NotNull(@this) */
            return @this.Bind(_ => other);
        }

        public static Ident<TSource> PassThrough<TSource, TOther>(
            this Ident<TSource> @this,
            Ident<TOther> other)
        {
            /* T4: NotNull(@this) */
            Func<TSource, TOther, TSource> zipper = (arg, _) => arg;

            return @this.Zip(other, zipper);
        }

        public static Ident<_Unit_> Skip<TSource>(this Ident<TSource> @this)
        {
            /* T4: NotNull(@this) */
            return @this.ContinueWith(Ident.Unit);
        }

        #region Zip()

        /// <seealso cref="Ident.Lift{T1, T2, TResult}(Func{T1, T2, TResult})"/>
        public static Ident<TResult> Zip<T1, T2, TResult>(
            this Ident<T1> @this,
            Ident<T2> second,
            Func<T1, T2, TResult> zipper)
        {
            /* T4: NotNull(@this) */
            /* T4: NotNull(second) */
            Require.NotNull(zipper, nameof(zipper));

            return @this.Bind(
                arg1 => second.Select(
                    arg2 => zipper(arg1, arg2)));
        }

        /// <seealso cref="Ident.Lift{T1, T2, T3, TResult}(Func{T1, T2, T3, TResult})"/>
        public static Ident<TResult> Zip<T1, T2, T3, TResult>(
            this Ident<T1> @this,
            Ident<T2> second,
            Ident<T3> third,
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

        /// <seealso cref="Ident.Lift{T1, T2, T3, T4, TResult}(Func{T1, T2, T3, T4, TResult})"/>
        public static Ident<TResult> Zip<T1, T2, T3, T4, TResult>(
             this Ident<T1> @this,
             Ident<T2> second,
             Ident<T3> third,
             Ident<T4> fourth,
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

        /// <seealso cref="Ident.Lift{T1, T2, T3, T4, T5, TResult}(Func{T1, T2, T3, T4, T5, TResult})"/>
        public static Ident<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this Ident<T1> @this,
            Ident<T2> second,
            Ident<T3> third,
            Ident<T4> fourth,
            Ident<T5> fifth,
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
        public static Ident<TResult> Using<TSource, TResult>(
            this Ident<TSource> @this,
            Func<TSource, Ident<TResult>> selector)
            where TSource : IDisposable
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            return @this.Bind(val => { using (val) { return selector(val); } });
        }

        // Select() with automatic resource management.
        public static Ident<TResult> Using<TSource, TResult>(
            this Ident<TSource> @this,
            Func<TSource, TResult> selector)
            where TSource : IDisposable
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            return @this.Select(val => { using (val) { return selector(val); } });
        }

        #endregion

        #region Query Expression Pattern

        public static Ident<TResult> Select<TSource, TResult>(
            this Ident<TSource> @this,
            Func<TSource, TResult> selector)
        {
            /* T4: NotNull(@this) */
            Require.NotNull(selector, nameof(selector));
            return @this.Bind(val => Ident<TResult>.η(selector(val)));
        }

        // Generalizes both Bind() and Zip<T1, T2, TResult>().
        public static Ident<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Ident<TSource> @this,
            Func<TSource, Ident<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            /* T4: NotNull(@this) */
            Require.NotNull(valueSelector, nameof(valueSelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(
                val => valueSelector(val).Select(
                    middle => resultSelector(val, middle)));
        }

        #endregion
    }

    // Provides extension methods for Ident<Func<TSource, TResult>>.
    // T4: EmitApplicative().
    public static partial class Ap
    {
        /// <seealso cref="Ident.Gather{TSource, TResult}" />
        public static Ident<TResult> Apply<TSource, TResult>(
            this Ident<Func<TSource, TResult>> @this,
            Ident<TSource> value)
        {
            /* T4: NotNull(value) */
            return value.Gather(@this);
        }
    }

    // Provides extension methods for functions in the Kleisli category.
    // T4: EmitKleisli().
    public static partial class Kleisli
    {
        public static Ident<IEnumerable<TResult>> InvokeWith<TSource, TResult>(
            this Func<TSource, Ident<TResult>> @this,
            IEnumerable<TSource> seq)
            => seq.Select(@this).Collect();

        public static Ident<TResult> InvokeWith<TSource, TResult>(
            this Func<TSource, Ident<TResult>> @this,
            Ident<TSource> value)
        {
            /* T4: NotNull(value) */
            return value.Bind(@this);
        }

        public static Func<TSource, Ident<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Ident<TMiddle>> @this,
            Func<TMiddle, Ident<TResult>> second)
        {
            Require.NotNull(@this, nameof(@this));
            return arg => @this(arg).Bind(second);
        }

        public static Func<TSource, Ident<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Ident<TResult>> @this,
            Func<TSource, Ident<TMiddle>> second)
        {
            Require.NotNull(second, nameof(second));
            return arg => second(arg).Bind(@this);
        }
    }

    // Provides extension methods for IEnumerable<Ident<T>>.
    // T4: EmitEnumerableExtensions().
    public static partial class Ident
    {
        public static Ident<IEnumerable<TSource>> Collect<TSource>(
            this IEnumerable<Ident<TSource>> @this)
            => @this.CollectImpl();
    }
}

namespace Narvalo.Internal
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Applicative;
    using Narvalo.Linq;

    // Provides default implementations for the extension methods for IEnumerable<Ident<T>>.
    // You will certainly want to override them to improve performance.
    // T4: EmitEnumerableInternal().
    internal static partial class EnumerableExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[GeneratedCode] This method has been overridden locally.")]
        internal static Ident<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Ident<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Ident<IEnumerable<TSource>>.η(CollectIterator(@this));
        }

        private static IEnumerable<TSource> CollectIterator<TSource>(IEnumerable<Ident<TSource>> source)
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

                            return Ident.Unit;
                        });

                    if (append) { yield return item; }
                }
            }
        }
    }
}

namespace Narvalo.Applicative
{
    // T4: EmitComonadHelpers().
    public static partial class Ident
    {
        public static T Extract<T>(Ident<T> value) => Ident<T>.ε(value);

        public static Ident<Ident<T>> Duplicate<T>(Ident<T> value) => Ident<T>.δ(value);
    }
}

