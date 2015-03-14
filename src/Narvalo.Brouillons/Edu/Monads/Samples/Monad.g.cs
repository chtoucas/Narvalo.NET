﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.34209
// </auto-generated>
//------------------------------------------------------------------------------

using global::System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
    Justification = "This rule is disabled for files generated by a Text Template.")]
[module: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1403:FileMayOnlyContainASingleNamespace",
    Justification = "This rule is disabled for files generated by a Text Template.")]

[module: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess",
    Justification = "For files generated by Text Template, we favour T4 readibility over StyleCop rules.")]
[module: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1505:OpeningCurlyBracketsMustNotBeFollowedByBlankLine",
    Justification = "For files generated by Text Template, we favour T4 readibility over StyleCop rules.")]
[module: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1507:CodeMustNotContainMultipleBlankLinesInARow",
    Justification = "For files generated by Text Template, we favour T4 readibility over StyleCop rules.")]

[module: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1210:UsingDirectivesMustBeOrderedAlphabeticallyByNamespace",
    Justification = "The directives are correctly ordered in the T4 source file.")]

namespace Narvalo.Edu.Monads.Samples 
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using global::Narvalo;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Monad{T}" />.
    /// </summary>
    /// <remarks>
    /// Sometimes we prefer to use extension methods over static methods to be able to locally override them.
    /// </remarks>
    [global::System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.TextTemplating.12.0", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCode]
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public static partial class Monad
    {
        private static readonly Monad<global::Narvalo.Fx.Unit> s_Unit = Return(global::Narvalo.Fx.Unit.Single);

        /// <summary>
        /// Gets the unique object of type <c>Monad&lt;Unit&gt;</c>.
        /// </summary>
        /// <value>The unique object of type <c>Monad&lt;Unit&gt;</c>.</value>
        public static Monad<global::Narvalo.Fx.Unit> Unit { get { return s_Unit; } }


        /// <summary>
        /// Obtains an instance of the <see cref="Monad{T}"/> class for the specified value.
        /// </summary>
        /// <remarks>
        /// Named <c>return</c> in Haskell parlance.
        /// </remarks>
        /// <typeparam name="T">The underlying type of the <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into a <see cref="Monad{T}"/> object.</param>
        /// <returns>An instance of the <see cref="Monad{T}"/> class for the specified value.</returns>
        public static Monad<T> Return<T>(T value)
        {

            return Monad<T>.η(value);
        }
        
        #region Generalisations of list functions (Prelude)

        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        /// <remarks>
        /// Named <c>join</c> in Haskell parlance.
        /// </remarks>
        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
        {
            Contract.Requires(square != null);

            return Monad<T>.μ(square);
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM</c> in Haskell parlance.
        /// </remarks>
        public static Func<Monad<T>, Monad<TResult>> Lift<T, TResult>(
            Func<T, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Monad<T>, Monad<TResult>>>() != null);

            return m => 
            {
                Require.NotNull(m, "m"); // Null-reference check: normally we don't need to check for null-reference since "Select" is an extension method but it could have been overriden by a normal method.
                return m.Select(fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values, scanning the 
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM2</c> in Haskell parlance.
        /// </remarks>
        public static Func<Monad<T1>, Monad<T2>, Monad<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Monad<T1>, Monad<T2>, Monad<TResult>>>() != null);

            return (m1, m2) => 
            {
                Require.NotNull(m1, "m1"); // Null-reference check: normally we don't need to check for null-reference since "Zip" is an extension method but it could have been overriden by a normal method.
                return m1.Zip(m2, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values, scanning the 
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM3</c> in Haskell parlance.
        /// </remarks>
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>>() != null);

            return (m1, m2, m3) => 
            {
                Require.NotNull(m1, "m1"); // Null-reference check: normally we don't need to check for null-reference since "Zip" is an extension method but it could have been overriden by a normal method.
                return m1.Zip(m2, m3, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM4</c> in Haskell parlance.
        /// </remarks>
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>>() != null);
            
            return (m1, m2, m3, m4) => 
            {
                Require.NotNull(m1, "m1"); // Null-reference check: normally we don't need to check for null-reference since "Zip" is an extension method but it could have been overriden by a normal method.
                return m1.Zip(m2, m3, m4, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Monad{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM5</c> in Haskell parlance.
        /// </remarks>
        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>>() != null);
       
            return (m1, m2, m3, m4, m5) => 
            {
                Require.NotNull(m1, "m1"); // Null-reference check: normally we don't need to check for null-reference since "Zip" is an extension method but it could have been overriden by a normal method.
                return m1.Zip(m2, m3, m4, m5, fun);
            };
        }

        #endregion
    } // End of the class Monad.

    /// <content>
    /// Provides core Monad extension methods.
    /// </content>
    public static partial class Monad
    {
        #region Basic Monad functions (Prelude)

        /// <remarks>
        /// Named <c>fmap</c> in Haskell parlance.
        /// </remarks>
        public static Monad<TResult> Select<TSource, TResult>(
            this Monad<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return @this.Bind(_ => Monad.Return(selector.Invoke(_)));
        }

        /// <remarks>
        /// Named <c>&gt;&gt;</c> in Haskell parlance.
        /// </remarks>
        public static Monad<TResult> Then<TSource, TResult>(
            this Monad<TSource> @this,
            Monad<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }
        
        #endregion

        #region Generalisations of list functions (Prelude)


        /// <remarks>
        /// Named <c>replicateM</c> in Haskell parlance.
        /// </remarks>
        public static Monad<IEnumerable<TSource>> Repeat<TSource>(
            this Monad<TSource> @this,
            int count)
        {
            Require.Object(@this); // Null-reference check: normally we don't need to check for null-reference since "Select" is an extension method but it could have been overriden by a normal method.
            Require.GreaterThanOrEqualTo(count, 1, "count");

            return @this.Select(_ => Enumerable.Repeat(_, count));
        }
        
        #endregion

        #region Conditional execution of monadic expressions (Prelude)


        /// <remarks>
        /// Named <c>when</c> in Haskell parlance.
        /// </remarks>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "Extension method intended to be used in a fluent way.")]
        public static Monad<global::Narvalo.Fx.Unit> When<TSource>(
            this Monad<TSource> @this, 
            bool predicate, 
            Action action)
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<Monad<global::Narvalo.Fx.Unit>>() != null);

            if (predicate) 
            {
                action.Invoke();
            }

            return Monad.Unit;
        }

        /// <remarks>
        /// Named <c>unless</c> in Haskell parlance.
        /// </remarks>
        public static Monad<global::Narvalo.Fx.Unit> Unless<TSource>(
            this Monad<TSource> @this,
            bool predicate,
            Action action)
        {
            Require.Object(@this);
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<Monad<global::Narvalo.Fx.Unit>>() != null);

            return @this.When(!predicate, action);
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        /// <see cref="Lift{T1, T2, T3}" />
        public static Monad<TResult> Zip<TFirst, TSecond, TResult>(
            this Monad<TFirst> @this,
            Monad<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second"); // Null-reference check: normally we don't need to check for null-reference since "Select" is an extension method but it could have been overriden by a normal method.
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(v1 => second.Select(v2 => resultSelector.Invoke(v1, v2)));
        }

        /// <see cref="Lift{T1, T2, T3, T4}" />
        public static Monad<TResult> Zip<T1, T2, T3, TResult>(
            this Monad<T1> @this,
            Monad<T2> second,
            Monad<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second"); // Null-reference check: normally we don't need to check for null-reference since "Zip" is an extension method but it could have been overriden by a normal method.
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Monad<TResult>> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5}" />
        public static Monad<TResult> Zip<T1, T2, T3, T4, TResult>(
             this Monad<T1> @this,
             Monad<T2> second,
             Monad<T3> third,
             Monad<T4> fourth,
             Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second"); // Null-reference check: normally we don't need to check for null-reference since "Zip" is an extension method but it could have been overriden by a normal method.
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Monad<TResult>> g
                = t1 => second.Zip(
                    third,
                    fourth, 
                    (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5, T6}" />
        public static Monad<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this Monad<T1> @this,
            Monad<T2> second,
            Monad<T3> third,
            Monad<T4> fourth,
            Monad<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second"); // Null-reference check: normally we don't need to check for null-reference since "Zip" is an extension method but it could have been overriden by a normal method.
            Require.NotNull(resultSelector, "resultSelector");

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
        /// Kind of generalisation of Zip (liftM2).
        /// </remarks>
        public static Monad<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Monad<TSource> @this,
            Func<TSource, Monad<TMiddle>> valueSelectorM,
            Func<TSource, TMiddle, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(valueSelectorM, "valueSelectorM");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(
                _ => valueSelectorM.Invoke(_).Select(
                    middle => resultSelector.Invoke(_, middle)));
        }


        #endregion
        
        #region LINQ extensions


        #endregion

        #region Non-standard extensions
        
        public static Monad<TResult> Coalesce<TSource, TResult>(
            this Monad<TSource> @this,
            Func<TSource, bool> predicate,
            Monad<TResult> then,
            Monad<TResult> otherwise)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }


        public static Monad<TSource> Run<TSource>(
            this Monad<TSource> @this,
            Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Bind(_ => { action.Invoke(_); return @this; });
        }


        #endregion
    } // End of the class Monad.

    /// <summary>
    /// Provides extension methods for <c>Func&lt;TSource, Monad&lt;TResult&gt;&gt;</c>.
    /// </summary>
    public static partial class FuncExtensions
    {
        #region Basic Monad functions (Prelude)

        /// <remarks>
        /// Named <c>=&lt;&lt;</c> in Haskell parlance.
        /// </remarks>
        public static Monad<TResult> Invoke<TSource, TResult>(
            this Func<TSource, Monad<TResult>> @this,
            Monad<TSource> value)
        {
            Require.NotNull(value, "value");
            Contract.Requires(@this != null);

            return value.Bind(@this);
        }

        /// <remarks>
        /// Named <c>&gt;=&gt;</c> in Haskell parlance.
        /// </remarks>
        public static Func<TSource, Monad<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Monad<TMiddle>> @this,
            Func<TMiddle, Monad<TResult>> funM)
        {
            Require.Object(@this);
            Contract.Requires(funM != null);
            Contract.Ensures(Contract.Result<Func<TSource, Monad<TResult>>>() != null);

            return _ => @this.Invoke(_).Bind(funM);
        }

        /// <remarks>
        /// Named <c>&lt;=&lt;</c> in Haskell parlance.
        /// </remarks>
        public static Func<TSource, Monad<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Monad<TResult>> @this,
            Func<TSource, Monad<TMiddle>> funM)
        {
            Require.NotNull(funM, "funM");
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Func<TSource, Monad<TResult>>>() != null);

            return _ => funM.Invoke(_).Bind(@this);
        }

        #endregion
    } // End of the class FuncExtensions.
}

namespace Narvalo.Edu.Monads.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using Narvalo.Edu.Monads.Samples;
    using Narvalo.Edu.Monads.Samples.Internal;

    /// <summary>
    /// Provides extension methods for <c>IEnumerable&lt;Monad&lt;T&gt;&gt;</c>.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.TextTemplating.12.0", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCode]
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public static partial class EnumerableMonadExtensions
    {
        #region Basic Monad functions (Prelude)

        /// <remarks>
        /// Named <c>sequence</c> in Haskell parlance.
        /// </remarks>
        public static Monad<IEnumerable<TSource>> Collect<TSource>(
            this IEnumerable<Monad<TSource>> @this)
        {
            // No need to check for null-reference, "CollectCore" is an extension method.
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Monad<IEnumerable<TSource>>>() != null);

            return @this.CollectCore();
        }

        #endregion
    } // End of the class EnumerableMonadExtensions.

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        #region Basic Monad functions (Prelude)

        /// <remarks>
        /// Named <c>mapM</c> in Haskell parlance.
        /// </remarks>
        public static Monad<IEnumerable<TResult>> Map<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<TResult>> funM)
        {
            // No need to check for null-reference, "MapCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(funM != null);
            Contract.Ensures(Contract.Result<Monad<IEnumerable<TResult>>>() != null);

            return @this.MapCore(funM);
        }

        #endregion

        #region Generalisations of list functions (Prelude)

        /// <remarks>
        /// Named <c>filterM</c> in Haskell parlance.
        /// </remarks>
        public static IEnumerable<TSource> Filter<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<bool>> predicateM)
        {
            // No need to check for null-reference, "FilterCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(predicateM != null);
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            return @this.FilterCore(predicateM);
        }

        /// <remarks>
        /// Named <c>mapAndUnzipM</c> in Haskell parlance.
        /// </remarks>
        public static Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            MapAndUnzip<TSource, TFirst, TSecond>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<Tuple<TFirst, TSecond>>> funM)
        {
            // No need to check for null-reference, "MapAndUnzipCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(funM != null);

            return @this.MapAndUnzipCore(funM);
        }

        /// <remarks>
        /// Named <c>zipWithM</c> in Haskell parlance.
        /// </remarks>
        public static Monad<IEnumerable<TResult>> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelectorM)
        {
            // No need to check for null-reference, "ZipCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(second != null);
            Contract.Requires(resultSelectorM != null);
            Contract.Ensures(Contract.Result<Monad<IEnumerable<TResult>>>() != null);

            return @this.ZipCore(second, resultSelectorM);
        }

        /// <remarks>
        /// Named <c>foldM</c> in Haskell parlance.
        /// </remarks>
        public static Monad<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulatorM)
        {
            // No need to check for null-reference, "FoldCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(accumulatorM != null);

            return @this.FoldCore(seed, accumulatorM);
        }

        #endregion

        #region Aggregate Operators

        public static Monad<TAccumulate> FoldBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulatorM)
        {
            // No need to check for null-reference, "FoldBackCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(accumulatorM != null);

            return @this.FoldBackCore(seed, accumulatorM);
        }

        public static Monad<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulatorM)
        {
            // No need to check for null-reference, "ReduceCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(accumulatorM != null);

            return @this.ReduceCore(accumulatorM);
        }

        public static Monad<TSource> ReduceBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulatorM)
        {
            // No need to check for null-reference, "ReduceBackCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(accumulatorM != null);

            return @this.ReduceBackCore(accumulatorM);
        }

        #endregion

        #region Catamorphisms

        public static Monad<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulatorM,
            Func<Monad<TAccumulate>, bool> predicate)
        {
            // No need to check for null-reference, "FoldCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(accumulatorM != null);
            Contract.Requires(predicate != null);

            return @this.FoldCore(seed, accumulatorM, predicate);
        }

        public static Monad<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulatorM,
            Func<Monad<TSource>, bool> predicate)
        {
            // No need to check for null-reference, "ReduceCore" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(accumulatorM != null);
            Contract.Requires(predicate != null);

            return @this.ReduceCore(accumulatorM, predicate);
        }

        #endregion
    } // End of the class EnumerableExtensions.
}

namespace Narvalo.Edu.Monads.Samples.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using global::Narvalo;
    using Narvalo.Edu.Monads.Samples;

    /// <summary>
    /// Provides extension methods for <c>IEnumerable&lt;Monad&lt;T&gt;&gt;</c>
    /// and <see cref="IEnumerable{T}"/>.
    /// </summary>
    internal static partial class EnumerableMonadExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<IEnumerable<TSource>> CollectCore<TSource>(
            this IEnumerable<Monad<TSource>> @this)
        {
            // No need to check for null-reference, "Enumerable.Aggregate" is an extension method.
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Monad<IEnumerable<TSource>>>() != null);

            var seed = Monad.Return(Enumerable.Empty<TSource>());
            Func<Monad<IEnumerable<TSource>>, Monad<TSource>, Monad<IEnumerable<TSource>>> fun
                = (m, n) => m.Bind(
                    list =>
                    {
                        return n.Bind(item => Monad.Return(
                            list.Concat(Enumerable.Repeat(item, 1))));
                    });

            return @this.Aggregate(seed, fun).AssumeNotNull_();
        }

        /// <summary>
        /// Instructs code analysis tools to assume that the specified value is not null,
        /// even if it cannot be statically proven to always be not null.
        /// When Code Contracts is disabled, this method is meant to be erased by the JIT compiler.
        /// </summary>
        [DebuggerHidden]
#if !CONTRACTS_FULL
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        private static T AssumeNotNull_<T>(this T @this) where T : class
        {
            Contract.Ensures(Contract.Result<T>() == @this);
            Contract.Ensures(Contract.Result<T>() != null);
            Contract.Assume(@this != null);

            return @this;
        }
    } // End of the class EnumerableMonadExtensions.

    /// <content>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </content>
    internal static partial class EnumerableMonadExtensions
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<IEnumerable<TResult>> MapCore<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<TResult>> funM)
        {
            // No need to check for null-reference, "Enumerable.Select" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(funM != null);
            Contract.Ensures(Contract.Result<Monad<IEnumerable<TResult>>>() != null);

            return @this.Select(funM).AssumeNotNull_().Collect();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static IEnumerable<TSource> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<bool>> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            // NB: Haskell uses tail recursion, we don't.
            var list = new List<TSource>();

            foreach (var item in @this)
            {
                var m = predicateM.Invoke(item);

                if (m != null)
                {
                    m.Run(
                        _ =>
                        {
                            if (_ == true)
                            {
                                list.Add(item);
                            }
                        });
                }
            }

            return list;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            MapAndUnzipCore<TSource, TFirst, TSecond>(
            this IEnumerable<TSource> @this,
            Func<TSource, Monad<Tuple<TFirst, TSecond>>> funM)
        {
            // No need to check for null-reference, "Enumerable.Select" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(funM != null);

            var m = @this.Select(funM).AssumeNotNull_().Collect();

            return m.Select(
                tuples =>
                {
                    IEnumerable<TFirst> list1 = tuples.Select(_ => _.Item1);
                    IEnumerable<TSecond> list2 = tuples.Select(_ => _.Item2);

                    return new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>(list1, list2);
                });
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<IEnumerable<TResult>> ZipCore<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelectorM)
        {
            Require.NotNull(resultSelectorM, "resultSelectorM");

            // No need to check for null-reference, "Enumerable.Zip" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(second != null);
            Contract.Ensures(Contract.Result<Monad<IEnumerable<TResult>>>() != null);

            Func<TFirst, TSecond, Monad<TResult>> resultSelector
                = (v1, v2) => resultSelectorM.Invoke(v1, v2);

            // WARNING: Do not remove "resultSelector", otherwise .NET will make a recursive call
            // instead of using the Zip from LINQ.
            return @this.Zip(second, resultSelector: resultSelector).AssumeNotNull_().Collect();
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<TAccumulate> FoldCore<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulatorM)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulatorM");

            Monad<TAccumulate> result = Monad.Return(seed);

            foreach (TSource item in @this)
            {
                if (result == null) 
                {
                    return null;
                }

                result = result.Bind(_ => accumulatorM.Invoke(_, item));
            }

            return result;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<TAccumulate> FoldBackCore<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulatorM)
        {
            // No need to check for null-reference, "Enumerable.Reverse" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(accumulatorM != null);

            return @this.Reverse().AssumeNotNull_().Fold(seed, accumulatorM);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<TSource> ReduceCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulatorM)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulatorM");

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Monad<TSource> result = Monad.Return(iter.Current);

                while (iter.MoveNext())
                {
                    if (result == null) 
                    {
                        return null;
                    }

                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }

                return result;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<TSource> ReduceBackCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulatorM)
        {
            // No need to check for null-reference, "Enumerable.Reverse" is an extension method. 
            Contract.Requires(@this != null);
            Contract.Requires(accumulatorM != null);

            return @this.Reverse().AssumeNotNull_().Reduce(accumulatorM);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<TAccumulate> FoldCore<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulatorM,
            Func<Monad<TAccumulate>, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulatorM");
            Require.NotNull(predicate, "predicate");

            Monad<TAccumulate> result = Monad.Return(seed);

            using (var iter = @this.GetEnumerator())
            {
                while (predicate.Invoke(result) && iter.MoveNext())
                {
                    if (result == null)
                    {
                        return null;
                    }

                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }
            }

            return result;
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
            Justification = "This method has been localy overriden.")]
        internal static Monad<TSource> ReduceCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Monad<TSource>> accumulatorM,
            Func<Monad<TSource>, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulatorM");
            Require.NotNull(predicate, "predicate");

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Monad<TSource> result = Monad.Return(iter.Current);

                while (predicate.Invoke(result) && iter.MoveNext())
                {
                    if (result == null)
                    {
                        return null;
                    }

                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }

                return result;
            }
        }
    } // End of the class EnumerableMonadExtensions.
}
