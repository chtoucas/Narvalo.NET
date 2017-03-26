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

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;

    using FsCheck.Xunit;
    using Xunit;

    // Provides tests for Outcome<T>.
    // T4: EmitMonadGuards().
    public static partial class OutcomeFacts
    {
        #region Repeat()

        [fact("Repeat() guards.")]
        public static void Repeat_guards()
        {
            var source = Outcome<int>.η(1);

            Assert.Throws<ArgumentOutOfRangeException>("count", () => Outcome.Repeat(source, -1));
        }

        #endregion

        #region Zip()

        [fact("Zip() guards.")]
        public static void Zip_guards()
        {
            var first = Outcome<int>.η(1);
            var second = Outcome<int>.η(2);
            var third = Outcome<int>.η(3);
            var fourth = Outcome<int>.η(4);
            var fifth = Outcome<int>.η(5);
            Func<int, int, int> zipper2 = null;
            Func<int, int, int, int> zipper3 = null;
            Func<int, int, int, int, int> zipper4 = null;
            Func<int, int, int, int, int, int> zipper5 = null;

            // Extension method.
            Assert.Throws<ArgumentNullException>("zipper", () => first.Zip(second, zipper2));
            Assert.Throws<ArgumentNullException>("zipper", () => first.Zip(second, third, zipper3));
            Assert.Throws<ArgumentNullException>("zipper", () => first.Zip(second, third, fourth, zipper4));
            Assert.Throws<ArgumentNullException>("zipper", () => first.Zip(second, third, fourth, fifth, zipper5));
            // Static method.
            Assert.Throws<ArgumentNullException>("zipper", () => OutcomeExtensions.Zip(first, second, zipper2));
            Assert.Throws<ArgumentNullException>("zipper", () => OutcomeExtensions.Zip(first, second, third, zipper3));
            Assert.Throws<ArgumentNullException>("zipper", () => OutcomeExtensions.Zip(first, second, third, fourth, zipper4));
            Assert.Throws<ArgumentNullException>("zipper", () => OutcomeExtensions.Zip(first, second, third, fourth, fifth, zipper5));
        }

        #endregion

        #region Select()

        [fact("Select() guards.")]
        public static void Select_guards()
        {
            var source = Outcome<int>.η(1);
            Func<int, long> selector = null;

            Assert.Throws<ArgumentNullException>("selector", () => source.Select(selector));
            Assert.Throws<ArgumentNullException>("selector", () => OutcomeExtensions.Select(source, selector));
        }

        #endregion

        #region SelectMany()

        [fact("SelectMany() guards.")]
        public static void SelectMany_guards()
        {
            var source = Outcome<short>.η(1);
            var middle = Outcome<int>.η(2);
            Func<short, Outcome<int>> valueSelector =  i => Outcome<int>.η(i);
            Func<short, int, long> resultSelector = (i, j) => i + j;

            // Extension method.
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectMany(null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector", () => source.SelectMany(valueSelector, (Func<short, int, long>)null));
            // Static method.
            Assert.Throws<ArgumentNullException>("selector", () => OutcomeExtensions.SelectMany(source, null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector", () => OutcomeExtensions.SelectMany(source, valueSelector, (Func<short, int, long>)null));
        }

        #endregion

    }

#if !NO_INTERNALS_VISIBLE_TO

    // Provides tests for Outcome<T> that need access to internals.
    // T4: EmitMonadInternal().
    public static partial class OutcomeFacts
    {
        #region Bind()

        [fact("Bind() applies the binder to the underlying value.")]
        public static void Bind_calls_binder()
        {
            var source = Outcome<int>.η(1);
            Func<int, Outcome<int>> binder = val => Outcome<int>.η(2 * val);

            var me = source.Bind(binder);

            Assert.Equal(2, me.Value);
        }

        #endregion

        #region Select()

        [fact("Select() applies the selector to the underlying value.")]
        public static void Select_calls_selector()
        {
            var source = Outcome<int>.η(1);
            Func<int, int> selector = val => 2 * val;

            var me = source.Select(selector);

            Assert.Equal(2, me.Value);
        }

        #endregion
    }

#endif

    // Provides tests for Outcome<T>: functor, monoid and monad laws.
    // T4: EmitMonadRules().
    public static partial class OutcomeFacts
    {
        #region Functor Rules

        [Property(DisplayName = "Outcome - The identity map is a fixed point for Select (first functor law).")]
        public static bool Identity_is_fixed_pointSelect(int arg)
        {
            var me = Outcome<int>.η(arg);

            // fmap id  ==  id
            var left = me.Select(val => val);
            var right = me;

            return left.Equals(right);
        }

        [Property(DisplayName = "Outcome - Select() preserves the composition operator (second functor law).")]
        public static bool Select_preserves_composition(short arg, Func<short, int> g, Func<int, long> f)
        {
            var me = Outcome<short>.η(arg);

            // fmap (f . g)  ==  fmap f . fmap g
            var left = me.Select(val => f(g(val)));
            var right = me.Select(g).Select(f);

           return left.Equals(right);
        }

        #endregion

        #region Monad Rules

        [Property(DisplayName = "Outcome - Of() is a left identity for Bind (first monad law).")]
        public static bool Of_is_left_identity_for_bind(int arg0, float arg1)
        {
            Func<int, Outcome<float>> f = val => Outcome<float>.η(arg1 * val);

            // return a >>= k  ==  k a
            var left = Outcome<int>.η(arg0).Bind(f);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Outcome - Of() is a left identity for Compose (first monad law).")]
        public static bool Of_is_left_identity_for_compose(int arg0, float arg1)
        {
            Func<int, Outcome<int>> of = Outcome<int>.η;
            Func<int, Outcome<float>> f = val => Outcome<float>.η(arg1 * val);

            // return >=> g  ==  g
            var left = of.Compose(f).Invoke(arg0);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Outcome - Of() is a right identity for Bind (second monad law).")]
        public static bool Of_is_right_identity_for_bind(int arg0)
        {
            var me = Outcome<int>.η(arg0);

            // m >>= return  ==  m
            var left = me.Bind(Outcome<int>.η);
            var right = me;

            return left.Equals(right);
        }

        [Property(DisplayName = "Outcome - Of() is a right identity for Compose (second monad law).")]
        public static bool Of_is_right_identity_for_compose(int arg0, float arg1)
        {
            Func<int, Outcome<float>> f = val => Outcome<float>.η(arg1 * val);

            // f >=> return  ==  f
            var left = f.Compose(Outcome<float>.η).Invoke(arg0);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Outcome - Bind() is associative (third monad law).")]
        public static bool Bind_is_associative(short arg0, int arg1, long arg2)
        {
            var me = Outcome<short>.η(arg0);

            Func<short, Outcome<int>> f = val => Outcome<int>.η(arg1 * val);
            Func<int, Outcome<long>> g = val => Outcome<long>.η(arg2 * val);

            // m >>= (\x -> f x >>= g)  ==  (m >>= f) >>= g
            var left = me.Bind(f).Bind(g);
            var right = me.Bind(val => f(val).Bind(g));

            return left.Equals(right);
        }

        [Property(DisplayName = "Outcome - Compose() is associative (third monad law).")]
        public static bool Compose_is_associative(short arg0, int arg1, long arg2, double arg3)
        {
            Func<short, Outcome<int>> f = val => Outcome<int>.η(arg1 * val);
            Func<int, Outcome<long>> g = val => Outcome<long>.η(arg2 * val);
            Func<long, Outcome<double>> h = val => Outcome<double>.η(arg3 * val);

            // f >=> (g >=> h)  ==  (f >=> g) >=> h
            var left = f.Compose(g.Compose(h)).Invoke(arg0);
            var right = f.Compose(g).Compose(h).Invoke(arg0);

            return left.Equals(right);
        }

        #endregion
    }
}

