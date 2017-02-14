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

using global::Narvalo;
using global::Narvalo.Fx;

namespace Monads
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Monads.Internal;
    using Monads.Linq;

    /// <summary>
    /// Provides a set of static methods for <see cref="Monad{T}"/>.
    /// </summary>
    // NB: Sometimes we prefer extension methods over static methods to be able to override them locally.
    public static partial class Monad
    {
        /// <summary>
        /// The unique object of type <c>Monad&lt;Unit&gt;</c>.
        /// </summary>
        private static readonly Monad<global::Narvalo.Fx.Unit> s_Unit = Of(global::Narvalo.Fx.Unit.Single);

        /// <summary>
        /// Gets the unique object of type <c>Monad&lt;Unit&gt;</c>.
        /// </summary>
        /// <value>The unique object of type <c>Monad&lt;Unit&gt;</c>.</value>
        public static Monad<global::Narvalo.Fx.Unit> Unit
        {
            get
            {
                Warrant.NotNull<Monad<global::Narvalo.Fx.Unit>>();

                return s_Unit;
            }
        }


        /// <summary>
        /// Obtains an instance of the <see cref="Monad{T}"/> class for the specified value.
        /// </summary>
        /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into a <see cref="Monad{T}"/> object.</param>
        /// <returns>An instance of the <see cref="Monad{T}"/> class for the specified value.</returns>
        // Named "return" in Haskell parlance.
        public static Monad<T> Of<T>(T value)
            /* T4: C# indent */
        {
            Warrant.NotNull<Monad<T>>();

            return Monad<T>.η(value);
        }

        #region Generalisations of list functions (Prelude)

        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        // Named "join" in Haskell parlance.
        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
            /* T4: C# indent */
        {
            Expect.NotNull(square);

            return Monad<T>.μ(square);
        }

        #endregion

        #region Conditional execution of monadic expressions (Prelude)


        #endregion

        #region Monadic lifting operators (Prelude)

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values.
        /// </summary>
        // Named "liftM" in Haskell parlance.
        public static Func<Monad<T>, Monad<TResult>> Lift<T, TResult>(
            Func<T, TResult> thunk)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<Monad<T>, Monad<TResult>>>();

            return m =>
            {
                Require.NotNull(m, nameof(m));
                return m.Select(thunk);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        // Named "liftM2" in Haskell parlance.
        public static Func<Monad<T1>, Monad<T2>, Monad<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> thunk)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<Monad<T1>, Monad<T2>, Monad<TResult>>>();

            return (m1, m2) =>
            {
                Require.NotNull(m1, nameof(m1));
                return m1.Zip(m2, thunk);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        // Named "liftM3" in Haskell parlance.
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> thunk)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>>();

            return (m1, m2, m3) =>
            {
                Require.NotNull(m1, nameof(m1));
                return m1.Zip(m2, m3, thunk);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        // Named "liftM4" in Haskell parlance.
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> thunk)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>>();

            return (m1, m2, m3, m4) =>
            {
                Require.NotNull(m1, nameof(m1));
                return m1.Zip(m2, m3, m4, thunk);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        // Named "liftM5" in Haskell parlance.
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> thunk)
            /* T4: C# indent */
        {
            Warrant.NotNull<Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>>();

            return (m1, m2, m3, m4, m5) =>
            {
                Require.NotNull(m1, nameof(m1));
                return m1.Zip(m2, m3, m4, m5, thunk);
            };
        }

        #endregion
    } // End of Monad - T4: EmitMonadCore().

    // Provides the core monadic extension methods for Monad<T>.
    public static partial class Monad
    {
        #region Applicative

        // Named "<$" in Haskell parlance.
        public static Monad<TResult> Replace<TSource, TResult>(
            this Monad<TSource> @this,
            TResult value)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));

            return @this.Select(_ => value);
        }

        #endregion

        #region Basic Monad functions (Prelude)

        // Named "fmap", "liftA" or "<$>" in Haskell parlance.
        public static Monad<TResult> Select<TSource, TResult>(
            this Monad<TSource> @this,
            Func<TSource, TResult> selector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return @this.Bind(_ => Monad.Of(selector.Invoke(_)));
        }

        // Named ">>" in Haskell parlance.
        public static Monad<TResult> Then<TSource, TResult>(
            this Monad<TSource> @this,
            Monad<TResult> other)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));

            return @this.Bind(_ => other);
        }

        // Named "void" in Haskell parlance.
        public static Monad<global::Narvalo.Fx.Unit> Forget<TSource>(this Monad<TSource> @this)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Monad<global::Narvalo.Fx.Unit>>();

            return Monad.Unit;
        }

        #endregion

        #region Generalisations of list functions (Prelude)


        // Named "replicateM" in Haskell parlance.
        public static Monad<IEnumerable<TSource>> Repeat<TSource>(
            this Monad<TSource> @this,
            int count)
        {
            Require.NotNull(@this, nameof(@this));
            Require.Range(count >= 1, nameof(count));

            return @this.Select(_ => Enumerable.Repeat(_, count));
        }


        #endregion

        #region Monadic lifting operators (Prelude)

        /// <see cref="Lift{T1, T2, T3}" />
        // Named "liftA2" in Haskell parlance.
        public static Monad<TResult> Zip<TFirst, TSecond, TResult>(
            this Monad<TFirst> @this,
            Monad<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(v1 => second.Select(v2 => resultSelector.Invoke(v1, v2)));
        }

        /// <see cref="Lift{T1, T2, T3, T4}" />
        // Named "liftA3" in Haskell parlance.
        public static Monad<TResult> Zip<T1, T2, T3, TResult>(
            this Monad<T1> @this,
            Monad<T2> second,
            Monad<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, Monad<TResult>> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5}" />
        // Named "liftA4" in Haskell parlance.
        public static Monad<TResult> Zip<T1, T2, T3, T4, TResult>(
             this Monad<T1> @this,
             Monad<T2> second,
             Monad<T3> third,
             Monad<T4> fourth,
             Func<T1, T2, T3, T4, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, Monad<TResult>> g
                = t1 => second.Zip(
                    third,
                    fourth,
                    (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5, T6}" />
        // Named "liftA5" in Haskell parlance.
        public static Monad<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this Monad<T1> @this,
            Monad<T2> second,
            Monad<T3> third,
            Monad<T4> fourth,
            Monad<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(second, nameof(second));
            Require.NotNull(resultSelector, nameof(resultSelector));

            Func<T1, Monad<TResult>> g
                = t1 => second.Zip(
                    third,
                    fourth,
                    fifth,
                    (t2, t3, t4, t5) => resultSelector.Invoke(t1, t2, t3, t4, t5));

            return @this.Bind(g);
        }

        #endregion

        #region Query Expression Pattern


        /// <remarks>
        /// Kind of generalisation of <see cref="Zip{T1, T2, T3}" /> (liftM2).
        /// </remarks>
        public static Monad<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Monad<TSource> @this,
            Func<TSource, Monad<TMiddle>> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(valueSelector, nameof(valueSelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(
                _ => valueSelector.Invoke(_).Select(
                    middle => resultSelector.Invoke(_, middle)));
        }


        #endregion

        #region LINQ extensions


        #endregion
    } // End of Monad - T4: EmitMonadExtensions().

    // Provides extension methods for Func<T> in the Kleisli category + one Applicative.
    public static partial class Func
    {
        #region Applicative


        // Named "<**>" in Haskell parlance. Same as Gather (<*>) with its arguments flipped.
        public static Monad<TResult> Apply<TSource, TResult>(
            this Monad<Func<TSource, TResult>> @this,
            Monad<TSource> value)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(value, nameof(value));

            return @this.Bind(thunk => value.Select(v => thunk.Invoke(v)));
        }


        #endregion

        #region Basic Monad functions (Prelude)


        // Named "forM" in Haskell parlance. Same as Map (mapM) with its arguments flipped.
        public static Monad<IEnumerable<TResult>> ForEach<TSource, TResult>(
            this Func<TSource, Monad<TResult>> @this,
            IEnumerable<TSource> seq)
        {
            Expect.NotNull(@this);
            Expect.NotNull(seq);
            Warrant.NotNull<Monad<IEnumerable<TResult>>>();

            return seq.Map(@this);
        }


        // Named "=<<" in Haskell parlance. Same as Bind (>>=) with its arguments flipped.
        public static Monad<TResult> Invoke<TSource, TResult>(
            this Func<TSource, Monad<TResult>> @this,
            Monad<TSource> value)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Require.NotNull(value, nameof(value));

            return value.Bind(@this);
        }

        // Named ">=>" in Haskell parlance.
        public static Func<TSource, Monad<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Monad<TMiddle>> @this,
            Func<TMiddle, Monad<TResult>> thunk)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Expect.NotNull(thunk);
            Warrant.NotNull<Func<TSource, Monad<TResult>>>();

            return _ => @this.Invoke(_).Bind(thunk);
        }

        // Named "<=<" in Haskell parlance.
        public static Func<TSource, Monad<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Monad<TResult>> @this,
            Func<TSource, Monad<TMiddle>> thunk)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Func<TSource, Monad<TResult>>>();

            return _ => thunk.Invoke(_).Bind(@this);
        }

        #endregion
    } // End of Func - T4: EmitKleisliExtensions().

    // Provides extension methods for IEnumerable<Monad<T>>.
    public static partial class Sequence
    {
        #region Basic Monad functions (Prelude)


        // Named "sequence" in Haskell parlance.
        public static Monad<IEnumerable<TSource>> Collect<TSource>(
            this IEnumerable<Monad<TSource>> @this)
        {
            Expect.NotNull(@this);
            Warrant.NotNull<Monad<IEnumerable<TSource>>>();

            return @this.CollectImpl();
        }


        #endregion

    } // End of Sequence - T4: EmitMonadEnumerableExtensions().
}

namespace Monads.Extensions
{
    using System;

    // Provides more extension methods for Monad<T>.
    public static partial class MonadExtensions
    {
        #region Basic Monad functions (Prelude)

        // Named "forever" in Haskell parlance.
        public static Monad<TResult> Forever<TSource, TResult>(
            this Monad<TSource> @this,
            Func<Monad<TResult>> thunk
            )
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));

            return @this.Then(@this.Forever(thunk));
        }

        #endregion

        #region Conditional execution of monadic expressions (Prelude)

        // Named "when" in Haskell parlance. Haskell uses a different signature.
        public static void When<TSource>(
            this Monad<TSource> @this,
            Func<TSource, bool> predicate,
            Action<TSource> action)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            @this.Bind(
                _ => {
                    if (predicate.Invoke(_)) { action.Invoke(_); }

                    return Monad.Unit;
                });
        }

        // Named "unless" in Haskell parlance. Haskell uses a different signature.
        public static void Unless<TSource>(
            this Monad<TSource> @this,
            Func<TSource, bool> predicate,
            Action<TSource> action)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            @this.Bind(
                _ => {
                    if (!predicate.Invoke(_)) { action.Invoke(_); }

                    return Monad.Unit;
                });
        }

        #endregion

        #region Applicative


        // Named "<*>" in Haskell parlance. Same as Apply (<**>) with its arguments flipped.
        public static Monad<TResult> Gather<TSource, TResult>(
            this Monad<TSource> @this,
            Monad<Func<TSource, TResult>> applicative)
            /* T4: C# indent */
        {
            Require.NotNull(applicative, nameof(applicative));

            return applicative.Apply(@this);
        }

        public static Monad<Tuple<TSource, TOther>> Merge<TSource, TOther>(
            this Monad<TSource> @this,
            Monad<TOther> other)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));

            return @this.Zip(other, Tuple.Create);
        }


        #endregion

        public static Monad<TResult> Coalesce<TSource, TResult>(
            this Monad<TSource> @this,
            Func<TSource, bool> predicate,
            Monad<TResult> then,
            Monad<TResult> otherwise)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }

        public static void Do<TSource>(
            this Monad<TSource> @this,
            Action<TSource> action)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(action, nameof(action));

            @this.Bind(
                _ => {
                    action.Invoke(_);

                    return Monad.Unit;
                });
        }
    } // End of Monad - T4: EmitMonadExtraExtensions().
}

namespace Monads.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Provides default implementations for the extension methods for IEnumerable<Monad<T>>.
    // You will certainly want to override them to improve performance.
    internal static partial class EnumerableExtensions
    {

        internal static Monad<IEnumerable<TSource>> CollectImpl<TSource>(
            this IEnumerable<Monad<TSource>> @this)
        {
            Demand.NotNull(@this);
            Warrant.NotNull<Monad<IEnumerable<TSource>>>();

            var seed = Monad.Of(Enumerable.Empty<TSource>());
            //var seed = Monad.Of(Enumerable.Empty<TSource>());
            // Inlined LINQ Append method:
            Func<IEnumerable<TSource>, TSource, IEnumerable<TSource>> append = (m, item) => m.Append(item);

            // NB: Maybe.Lift(append) is the same as:
            // Func<Monad<IEnumerable<TSource>>, Monad<TSource>, Monad<IEnumerable<TSource>>> liftedAppend
            //     = (m, item) => m.Bind(list => Append(list, item));
            // where Append is defined below.
            var retval = @this.Aggregate(seed, Monad.Lift<IEnumerable<TSource>, TSource, IEnumerable<TSource>>(append));
            System.Diagnostics.Contracts.Contract.Assume(retval != null);

            return retval;
        }

        // NB: We do not inline this method to avoid the creation of an unused private field (CA1823 warning).
        //private static Monad<IEnumerable<TSource>> Append<TSource>(
        //    IEnumerable<TSource> list,
        //    Monad<TSource> m)
        //{
        //    Demand.NotNull(m);

        //    return m.Bind(item => Monad.Of(list.Concat(Enumerable.Repeat(item, 1))));
        //}

    } // End of EnumerableExtensions - T4: EmitMonadEnumerableInternalExtensions().
}

namespace Monads.Linq
{
    using System;
    using System.Collections.Generic;

    using Monads;
    using Monads.Internal;

    // Provides extension methods for IEnumerable<T>.
    // We do not use the standard LINQ names to avoid a confusing API.
    // - Select    -> Map
    // - Where     -> Filter
    // - Zip       -> ZipWith
    // - Aggregate -> Reduce or Fold
    public static partial class Operators
    {
        #region Basic Monad functions (Prelude)


        // Named "mapM" in Haskell parlance.
        public static Monad<IEnumerable<TResult>> Map<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<TResult>> selector)
        {
            Expect.NotNull(@this);
            Expect.NotNull(selector);
            Warrant.NotNull<Monad<IEnumerable<TResult>>>();

            return @this.MapImpl(selector);
        }


        #endregion

        #region Generalisations of list functions (Prelude)


        // Named "filterM" in Haskell parlance.
        public static Monad<IEnumerable<TSource>> Filter<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<bool>> predicate)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TSource>>();

            return @this.FilterImpl(predicate);
        }

        // Named "mapAndUnzipM" in Haskell parlance.
        public static Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            MapUnzip<TSource, TFirst, TSecond>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<Tuple<TFirst, TSecond>>> thunk)
        {
            Expect.NotNull(@this);
            Expect.NotNull(thunk);

            return @this.MapUnzipImpl(thunk);
        }

        // Named "zipWithM" in Haskell parlance.
        public static Monad<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelector)
        {
            Expect.NotNull(@this);
            Expect.NotNull(second);
            Expect.NotNull(resultSelector);
            Warrant.NotNull<Monad<IEnumerable<TResult>>>();

            return @this.ZipWithImpl(second, resultSelector);
        }


        // Named "foldM" in Haskell parlance.
        public static Monad<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);

            return @this.FoldImpl(seed, accumulator);
        }

        #endregion

        #region Aggregate Operators

        public static Monad<TAccumulate> FoldBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);

            return @this.FoldBackImpl(seed, accumulator);
        }

        public static Monad<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulator)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);

            return @this.ReduceImpl(accumulator);
        }

        public static Monad<TSource> ReduceBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulator)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);

            return @this.ReduceBackImpl(accumulator);
        }

        #endregion

        #region Catamorphisms

        // Haskell uses a different signature.
        public static Monad<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator,
            Func<Monad<TAccumulate>, bool> predicate)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);
            Expect.NotNull(predicate);

            return @this.FoldImpl(seed, accumulator, predicate);
        }

        // Haskell uses a different signature.
        public static Monad<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulator,
            Func<Monad<TSource>, bool> predicate)
            /* T4: C# indent */
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);
            Expect.NotNull(predicate);

            return @this.ReduceImpl(accumulator, predicate);
        }

        #endregion
    } // End of Iterable - T4: EmitEnumerableExtensions().
}

namespace Monads.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Monads.Linq;

    // Provides default implementations for the extension methods for IEnumerable<T>.
    // You will certainly want to override them to improve performance.
    internal static partial class EnumerableExtensions
    {

        internal static Monad<IEnumerable<TResult>> MapImpl<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<TResult>> selector)
        {
            Demand.NotNull(@this);
            Demand.NotNull(selector);
            Warrant.NotNull<Monad<IEnumerable<TResult>>>();

            return @this.Select(selector).EmptyIfNull().Collect();
        }

        internal static Monad<IEnumerable<TSource>> FilterImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<bool>> predicate)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            Func<bool, IEnumerable<TSource>, TSource, IEnumerable<TSource>> selector
                = (flg, list, item) => { if (flg) { return list.Prepend(item); } else { return list; } };

            Func<Monad<IEnumerable<TSource>>, TSource, Monad<IEnumerable<TSource>>> accumulator
                = (mlist, item) => predicate.Invoke(item).Zip(mlist, (flg, list) => selector.Invoke(flg, list, item));

            var seed = Monad.Of(Enumerable.Empty<TSource>());

            // REVIEW: Aggregate?
            return @this.AggregateBack(seed, accumulator);
        }

        internal static Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            MapUnzipImpl<TSource, TFirst, TSecond>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<Tuple<TFirst, TSecond>>> selector)
        {
            Demand.NotNull(@this);
            Demand.NotNull(selector);

            return @this.Map(selector).Select(
                tuples =>
                {
                    IEnumerable<TFirst> list1 = tuples.Select(_ => _.Item1);
                    IEnumerable<TSecond> list2 = tuples.Select(_ => _.Item2);

                    return new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>(list1, list2);
                });
        }

        internal static Monad<IEnumerable<TResult>> ZipWithImpl<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelector)
        {
            Require.NotNull(resultSelector, nameof(resultSelector));

            Demand.NotNull(@this);
            Demand.NotNull(second);
            Warrant.NotNull<Monad<IEnumerable<TResult>>>();

            Func<TFirst, TSecond, Monad<TResult>> selector
                = (v1, v2) => resultSelector.Invoke(v1, v2);

            IEnumerable<Monad<TResult>> seq = @this.Zip(second, selector);

            return seq.EmptyIfNull().Collect();
        }

        internal static Monad<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));

            Monad<TAccumulate> retval = Monad.Of(seed);

            foreach (TSource item in @this)
            {
                if (retval == null)
                {
                    return null;
                }

                retval = retval.Bind(_ => accumulator.Invoke(_, item));
            }

            return retval;
        }

        internal static Monad<TAccumulate> FoldBackImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator)
            /* T4: C# indent */
        {
            Demand.NotNull(@this);
            Demand.NotNull(accumulator);

            return @this.Reverse().EmptyIfNull().Fold(seed, accumulator);
        }

        internal static Monad<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulator)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Monad<TSource> retval = Monad.Of(iter.Current);

                while (iter.MoveNext())
                {
                    if (retval == null)
                    {
                        return null;
                    }

                    retval = retval.Bind(_ => accumulator.Invoke(_, iter.Current));
                }

                return retval;
            }
        }

        internal static Monad<TSource> ReduceBackImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulator)
            /* T4: C# indent */
        {
            Demand.NotNull(@this);
            Demand.NotNull(accumulator);

            return @this.Reverse().EmptyIfNull().Reduce(accumulator);
        }

        internal static Monad<TAccumulate> FoldImpl<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator,
            Func<Monad<TAccumulate>, bool> predicate)
            /* T4: C# indent */
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

            Monad<TAccumulate> retval = Monad.Of(seed);

            using (var iter = @this.GetEnumerator())
            {
                while (predicate.Invoke(retval) && iter.MoveNext())
                {
                    if (retval == null)
                    {
                        return null;
                    }

                    retval = retval.Bind(_ => accumulator.Invoke(_, iter.Current));
                }
            }

            return retval;
        }

        internal static Monad<TSource> ReduceImpl<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulator,
            Func<Monad<TSource>, bool> predicate)
            /* T4: C# indent */
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

                Monad<TSource> retval = Monad.Of(iter.Current);

                while (predicate.Invoke(retval) && iter.MoveNext())
                {
                    if (retval == null)
                    {
                        return null;
                    }

                    retval = retval.Bind(_ => accumulator.Invoke(_, iter.Current));
                }

                return retval;
            }
        }
    } // End of EnumerableExtensions - T4: EmitEnumerableInternalExtensions().
}

