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

    // Provides tests for Fallible<T>.
    // T4: EmitCore().
    public static partial class FallibleFacts
    {
        #region Repeat()

        [Fact]
        public static void Repeat_Guards()
        {
            var source = Fallible<int>.η(1);

            Assert.Throws<ArgumentOutOfRangeException>("count", () => Fallible.Repeat(source, -1));
        }

        #endregion

        #region Zip()

        [Fact]
        public static void Zip_Guards()
        {
            var first = Fallible<int>.η(1);
            var second = Fallible<int>.η(2);
            var third = Fallible<int>.η(3);
            var fourth = Fallible<int>.η(4);
            var fifth = Fallible<int>.η(5);
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
            Assert.Throws<ArgumentNullException>("zipper", () => FallibleExtensions.Zip(first, second, zipper2));
            Assert.Throws<ArgumentNullException>("zipper", () => FallibleExtensions.Zip(first, second, third, zipper3));
            Assert.Throws<ArgumentNullException>("zipper", () => FallibleExtensions.Zip(first, second, third, fourth, zipper4));
            Assert.Throws<ArgumentNullException>("zipper", () => FallibleExtensions.Zip(first, second, third, fourth, fifth, zipper5));
        }

        #endregion

        #region Select()

        [Fact]
        public static void Select_Guards()
        {
            var source = Fallible<int>.η(1);
            Func<int, long> selector = null;

            Assert.Throws<ArgumentNullException>("selector", () => source.Select(selector));
            Assert.Throws<ArgumentNullException>("selector", () => FallibleExtensions.Select(source, selector));
        }

        #endregion

        #region SelectMany()

        [Fact]
        public static void SelectMany_Guards()
        {
            var source = Fallible<short>.η(1);
            var middle = Fallible<int>.η(2);
            Func<short, Fallible<int>> valueSelector =  i => Fallible<int>.η(i);
            Func<short, int, long> resultSelector = (i, j) => i + j;

            // Extension method.
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectMany(null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector", () => source.SelectMany(valueSelector, (Func<short, int, long>)null));
            // Static method.
            Assert.Throws<ArgumentNullException>("selector", () => FallibleExtensions.SelectMany(source, null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector", () => FallibleExtensions.SelectMany(source, valueSelector, (Func<short, int, long>)null));
        }

        #endregion

    }

#if !NO_INTERNALS_VISIBLE_TO

    // Provides tests for Fallible<T> that need access to internals.
    // T4: EmitCore().
    public static partial class FallibleFacts
    {
        #region Bind()

        [Fact]
        public static void Bind_AppliesBinder()
        {
            // Arrange
            var source = Fallible<int>.η(1);
            Func<int, Fallible<int>> binder = val => Fallible<int>.η(2 * val);

            // Act
            var me = source.Bind(binder);

            // Assert
            Assert.Equal(2, me.Value);
        }

        #endregion

        #region Select()

        [Fact]
        public static void Select_AppliesSelector()
        {
            // Arrange
            var source = Fallible<int>.η(1);
            Func<int, int> selector = val => 2 * val;

            // Act
            var me = source.Select(selector);

            // Assert
            Assert.Equal(2, me.Value);
        }

        #endregion
    }

#endif

    // Provides tests for Fallible<T>: functor, monoid and monad laws.
    // T4: EmitRules().
    public static partial class FallibleFacts
    {
        #region Functor Rules

        [Property(DisplayName = "Fallible<T> - The identity map is a fixed point for Select (first functor law).")]
        public static bool Identity_IsFixedPointForSelect(int arg)
        {
            var me = Fallible<int>.η(arg);

            // fmap id  ==  id
            var left = me.Select(val => val);
            var right = me;

            return left.Equals(right);
        }

        [Property(DisplayName = "Fallible<T> - Select preserves the composition operator (second functor law).")]
        public static bool Select_PreservesComposition(short arg, Func<short, int> g, Func<int, long> f)
        {
            var me = Fallible<short>.η(arg);

            // fmap (f . g)  ==  fmap f . fmap g
            var left = me.Select(val => f(g(val)));
            var right = me.Select(g).Select(f);

           return left.Equals(right);
        }

        #endregion

        #region Monad Rules

        [Property(DisplayName = "Fallible<T> - Of is a left identity for Bind (first monad law).")]
        public static bool Of_IsLeftIdentityForBind(int arg0, float arg1)
        {
            Func<int, Fallible<float>> f = val => Fallible<float>.η(arg1 * val);

            // return a >>= k  ==  k a
            var left = Fallible<int>.η(arg0).Bind(f);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Fallible<T> - Of is a left identity for Compose (first monad law).")]
        public static bool Of_IsLeftIdentityForCompose(int arg0, float arg1)
        {
            Func<int, Fallible<int>> of = Fallible<int>.η;
            Func<int, Fallible<float>> f = val => Fallible<float>.η(arg1 * val);

            // return >=> g  ==  g
            var left = of.Compose(f).Invoke(arg0);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Fallible<T> - Of is a right identity for Bind (second monad law).")]
        public static bool Of_IsRightIdentityForBind(int arg0)
        {
            var me = Fallible<int>.η(arg0);

            // m >>= return  ==  m
            var left = me.Bind(Fallible<int>.η);
            var right = me;

            return left.Equals(right);
        }

        [Property(DisplayName = "Fallible<T> - Of is a right identity for Compose (second monad law).")]
        public static bool Of_IsRightIdentityForCompose(int arg0, float arg1)
        {
            Func<int, Fallible<float>> f = val => Fallible<float>.η(arg1 * val);

            // f >=> return  ==  f
            var left = f.Compose(Fallible<float>.η).Invoke(arg0);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Fallible<T> - Bind is associative (third monad law).")]
        public static bool Bind_IsAssociative(short arg0, int arg1, long arg2)
        {
            var me = Fallible<short>.η(arg0);

            Func<short, Fallible<int>> f = val => Fallible<int>.η(arg1 * val);
            Func<int, Fallible<long>> g = val => Fallible<long>.η(arg2 * val);

            // m >>= (\x -> f x >>= g)  ==  (m >>= f) >>= g
            var left = me.Bind(f).Bind(g);
            var right = me.Bind(val => f(val).Bind(g));

            return left.Equals(right);
        }

        [Property(DisplayName = "Fallible<T> - Compose is associative (third monad law).")]
        public static bool Compose_IsAssociative(short arg0, int arg1, long arg2, double arg3)
        {
            Func<short, Fallible<int>> f = val => Fallible<int>.η(arg1 * val);
            Func<int, Fallible<long>> g = val => Fallible<long>.η(arg2 * val);
            Func<long, Fallible<double>> h = val => Fallible<double>.η(arg3 * val);

            // f >=> (g >=> h)  ==  (f >=> g) >=> h
            var left = f.Compose(g.Compose(h)).Invoke(arg0);
            var right = f.Compose(g).Compose(h).Invoke(arg0);

            return left.Equals(right);
        }

        #endregion
    }
}

