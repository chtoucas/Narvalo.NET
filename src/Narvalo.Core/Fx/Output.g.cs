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

// See http://msdn.microsoft.com/en-us/library/ms244717.aspx for an explanation 
// of the effect of the SuppressMessage attribute at module scope.
// This suppresses the corresponding warnings for the code inside the generated file.
// We either favour T4 readibility over StyleCop rules or disable rules that do not make sense 
// for files generated by a Text Template.
[module: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess",
    Justification = "[GeneratedCode] Elements are correctly ordered in the T4 source file.")]
[module: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1210:UsingDirectivesMustBeOrderedAlphabeticallyByNamespace",
    Justification = "[GeneratedCode] Directives are correctly ordered in the T4 source file.")]
[module: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
    Justification = "[GeneratedCode] A T4 template may contain multiple classes..")]
[module: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1403:FileMayOnlyContainASingleNamespace",
    Justification = "[GeneratedCode] A T4 template may contain multiple namespaces.")]
[module: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1505:OpeningCurlyBracketsMustNotBeFollowedByBlankLine",
    Justification = "[GeneratedCode] Newline rules are disabled for T4 templates.")]
[module: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1507:CodeMustNotContainMultipleBlankLinesInARow",
    Justification = "[GeneratedCode] Newline rules are disabled for T4 templates.")]

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using global::Narvalo;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Output{T}" />.
    /// </summary>
    /// <remarks>
    /// Sometimes we prefer to use extension methods over static methods to be able to locally override them.
    /// </remarks>
    [global::System.CodeDom.Compiler.GeneratedCode("Microsoft.VisualStudio.TextTemplating.12.0", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCode]
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public static partial class Output
    {
        private static readonly Output<global::Narvalo.Fx.Unit> s_Unit = Success(global::Narvalo.Fx.Unit.Single);

        /// <summary>
        /// Gets the unique object of type <c>Output&lt;Unit&gt;</c>.
        /// </summary>
        /// <value>The unique object of type <c>Output&lt;Unit&gt;</c>.</value>
        public static Output<global::Narvalo.Fx.Unit> Unit { get { return s_Unit; } }


        /// <summary>
        /// Obtains an instance of the <see cref="Output{T}"/> class for the specified value.
        /// </summary>
        /// <remarks>
        /// Named <c>return</c> in Haskell parlance.
        /// </remarks>
        /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into a <see cref="Output{T}"/> object.</param>
        /// <returns>An instance of the <see cref="Output{T}"/> class for the specified value.</returns>
        public static Output<T> Success<T>(T value)
        {
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            return Output<T>.η(value);
        }
        
        #region Generalisations of list functions (Prelude)

        /// <summary>
        /// Removes one level of structure, projecting its bound value into the outer level.
        /// </summary>
        /// <remarks>
        /// Named <c>join</c> in Haskell parlance.
        /// </remarks>
        public static Output<T> Flatten<T>(Output<Output<T>> square)
        {
            Contract.Requires(square != null);
            Contract.Ensures(Contract.Result<Output<T>>() != null);

            return Output<T>.μ(square);
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        /// <summary>
        /// Promotes a function to use and return <see cref="Output{T}" /> values.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM</c> in Haskell parlance.
        /// </remarks>
        public static Func<Output<T>, Output<TResult>> Lift<T, TResult>(
            Func<T, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Output<T>, Output<TResult>>>() != null);

            return m => 
            {
                Require.NotNull(m, "m");
                return m.Select(fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Output{T}" /> values, scanning the 
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM2</c> in Haskell parlance.
        /// </remarks>
        public static Func<Output<T1>, Output<T2>, Output<TResult>>
            Lift<T1, T2, TResult>(Func<T1, T2, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Output<T1>, Output<T2>, Output<TResult>>>() != null);

            return (m1, m2) => 
            {
                Require.NotNull(m1, "m1");
                return m1.Zip(m2, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Output{T}" /> values, scanning the 
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM3</c> in Haskell parlance.
        /// </remarks>
        public static Func<Output<T1>, Output<T2>, Output<T3>, Output<TResult>>
            Lift<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Output<T1>, Output<T2>, Output<T3>, Output<TResult>>>() != null);

            return (m1, m2, m3) => 
            {
                Require.NotNull(m1, "m1");
                return m1.Zip(m2, m3, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Output{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM4</c> in Haskell parlance.
        /// </remarks>
        public static Func<Output<T1>, Output<T2>, Output<T3>, Output<T4>, Output<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Output<T1>, Output<T2>, Output<T3>, Output<T4>, Output<TResult>>>() != null);
            
            return (m1, m2, m3, m4) => 
            {
                Require.NotNull(m1, "m1");
                return m1.Zip(m2, m3, m4, fun);
            };
        }

        /// <summary>
        /// Promotes a function to use and return <see cref="Output{T}" /> values, scanning the
        /// monadic arguments from left to right.
        /// </summary>
        /// <remarks>
        /// Named <c>liftM5</c> in Haskell parlance.
        /// </remarks>
        public static Func<Output<T1>, Output<T2>, Output<T3>, Output<T4>, Output<T5>, Output<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> fun)
        {
            Contract.Ensures(Contract.Result<Func<Output<T1>, Output<T2>, Output<T3>, Output<T4>, Output<T5>, Output<TResult>>>() != null);
       
            return (m1, m2, m3, m4, m5) => 
            {
                Require.NotNull(m1, "m1");
                return m1.Zip(m2, m3, m4, m5, fun);
            };
        }

        #endregion
    } // End of the class Output.

    /// <content>
    /// Provides core Monad extension methods.
    /// </content>
    public static partial class Output
    {
        #region Basic Monad functions (Prelude)

        /// <remarks>
        /// Named <c>fmap</c> in Haskell parlance.
        /// </remarks>
        public static Output<TResult> Select<TSource, TResult>(
            this Output<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return @this.Bind(_ => Output.Success(selector.Invoke(_)));
        }

        /// <remarks>
        /// Named <c>&gt;&gt;</c> in Haskell parlance.
        /// </remarks>
        public static Output<TResult> Then<TSource, TResult>(
            this Output<TSource> @this,
            Output<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }
        
        #endregion

        #region Generalisations of list functions (Prelude)


        /// <remarks>
        /// Named <c>replicateM</c> in Haskell parlance.
        /// </remarks>
        public static Output<IEnumerable<TSource>> Repeat<TSource>(
            this Output<TSource> @this,
            int count)
        {
            Require.Object(@this);
            Require.GreaterThanOrEqualTo(count, 1, "count");

            return @this.Select(_ => Enumerable.Repeat(_, count));
        }
        
        #endregion

        #region Conditional execution of monadic expressions (Prelude)


        /// <remarks>
        /// <para>Named <c>when</c> in Haskell parlance.</para>
        /// <para>Haskell use a different signature. The method should return a <see cref="Narvalo.Fx.Unit"/>.</para>
        /// </remarks>
        public static Output<TSource> When<TSource>(
            this Output<TSource> @this, 
            bool predicate, 
            Action action)
        {
            Require.NotNull(action, "action");
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Output<TSource>>() != null);

            if (predicate) 
            {
                action.Invoke();
            }

            return @this;
        }

        /// <remarks>
        /// <para>Named <c>unless</c> in Haskell parlance.</para>
        /// <para>Haskell use a different signature. The method should return a <see cref="Narvalo.Fx.Unit"/>.</para>
        /// </remarks>
        public static Output<TSource> Unless<TSource>(
            this Output<TSource> @this,
            bool predicate,
            Action action)
        {
            Require.NotNull(action, "action");
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Output<TSource>>() != null);

            if (!predicate) 
            {
                action.Invoke();
            }

            return @this;
        }

        #endregion

        #region Monadic lifting operators (Prelude)

        /// <see cref="Lift{T1, T2, T3}" />
        public static Output<TResult> Zip<TFirst, TSecond, TResult>(
            this Output<TFirst> @this,
            Output<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(v1 => second.Select(v2 => resultSelector.Invoke(v1, v2)));
        }

        /// <see cref="Lift{T1, T2, T3, T4}" />
        public static Output<TResult> Zip<T1, T2, T3, TResult>(
            this Output<T1> @this,
            Output<T2> second,
            Output<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Output<TResult>> g
                = t1 => second.Zip(third, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5}" />
        public static Output<TResult> Zip<T1, T2, T3, T4, TResult>(
             this Output<T1> @this,
             Output<T2> second,
             Output<T3> third,
             Output<T4> fourth,
             Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Output<TResult>> g
                = t1 => second.Zip(
                    third,
                    fourth, 
                    (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        /// <see cref="Lift{T1, T2, T3, T4, T5, T6}" />
        public static Output<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
            this Output<T1> @this,
            Output<T2> second,
            Output<T3> third,
            Output<T4> fourth,
            Output<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            Func<T1, Output<TResult>> g
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
        public static Output<TResult> SelectMany<TSource, TMiddle, TResult>(
            this Output<TSource> @this,
            Func<TSource, Output<TMiddle>> valueSelectorM,
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
        
        public static Output<TResult> Coalesce<TSource, TResult>(
            this Output<TSource> @this,
            Func<TSource, bool> predicate,
            Output<TResult> then,
            Output<TResult> otherwise)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }


        public static void Apply<TSource>(
            this Output<TSource> @this,
            Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            @this.Bind(_ => { action.Invoke(_); return @this; });
        }


        #endregion
    } // End of the class Output.
}

// Extensions for core CLR types are put into a separate namespace.
namespace Narvalo.Fx.Extensions
{
    using System;
    using System.Diagnostics.Contracts;

    using global::Narvalo;

    /// <summary>
    /// Provides extension methods for <see cref="Func{T}"/> that depend on the <see cref="Output{T}"/> class.
    /// </summary>
    [SuppressMessage("Gendarme.Rules.Smells", "AvoidSpeculativeGeneralityRule",
        Justification = "[Intentionally] Delegation is an unavoidable annoyance of fluent interfaces on delegates.")]
    public static partial class FuncOutputExtensions
    {
        #region Basic Monad functions (Prelude)

        /// <remarks>
        /// Named <c>=&lt;&lt;</c> in Haskell parlance.
        /// </remarks>
        public static Output<TResult> Invoke<TSource, TResult>(
            this Func<TSource, Output<TResult>> @this,
            Output<TSource> value)
        {
            Require.NotNull(value, "value");
            Contract.Requires(@this != null);

            return value.Bind(@this);
        }

        /// <remarks>
        /// Named <c>&gt;=&gt;</c> in Haskell parlance.
        /// </remarks>
        public static Func<TSource, Output<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Output<TMiddle>> @this,
            Func<TMiddle, Output<TResult>> funM)
        {
            Require.Object(@this);
            Contract.Requires(funM != null);
            Contract.Ensures(Contract.Result<Func<TSource, Output<TResult>>>() != null);

            return _ => @this.Invoke(_).Bind(funM);
        }

        /// <remarks>
        /// Named <c>&lt;=&lt;</c> in Haskell parlance.
        /// </remarks>
        public static Func<TSource, Output<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Output<TResult>> @this,
            Func<TSource, Output<TMiddle>> funM)
        {
            Require.NotNull(funM, "funM");
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Func<TSource, Output<TResult>>>() != null);

            return _ => funM.Invoke(_).Bind(@this);
        }

        #endregion
    } // End of the class FuncOutputExtensions.
}
