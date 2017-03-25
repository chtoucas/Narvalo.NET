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

    // Provides tests for Maybe<T>.
    // T4: EmitMonadGuards().
    public static partial class MaybeFacts
    {
        #region Repeat()

        [Fact]
        public static void Repeat_Guards()
        {
            var source = Maybe<int>.η(1);

            Assert.Throws<ArgumentOutOfRangeException>("count", () => Maybe.Repeat(source, -1));
        }

        #endregion

        #region Zip()

        [Fact]
        public static void Zip_Guards()
        {
            var first = Maybe<int>.η(1);
            var second = Maybe<int>.η(2);
            var third = Maybe<int>.η(3);
            var fourth = Maybe<int>.η(4);
            var fifth = Maybe<int>.η(5);
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
            Assert.Throws<ArgumentNullException>("zipper", () => Maybe.Zip(first, second, zipper2));
            Assert.Throws<ArgumentNullException>("zipper", () => Maybe.Zip(first, second, third, zipper3));
            Assert.Throws<ArgumentNullException>("zipper", () => Maybe.Zip(first, second, third, fourth, zipper4));
            Assert.Throws<ArgumentNullException>("zipper", () => Maybe.Zip(first, second, third, fourth, fifth, zipper5));
        }

        #endregion

        #region Select()

        [Fact]
        public static void Select_Guards()
        {
            var source = Maybe<int>.η(1);
            Func<int, long> selector = null;

            Assert.Throws<ArgumentNullException>("selector", () => source.Select(selector));
            Assert.Throws<ArgumentNullException>("selector", () => Maybe.Select(source, selector));
        }

        #endregion

        #region Where()

        [Fact]
        public static void Where_Guards()
        {
            var source = Maybe<int>.η(1);

            Assert.Throws<ArgumentNullException>("predicate", () => source.Where(null));
            Assert.Throws<ArgumentNullException>("predicate", () => Maybe.Where(source, null));
        }

        #endregion

        #region SelectMany()

        [Fact]
        public static void SelectMany_Guards()
        {
            var source = Maybe<short>.η(1);
            var middle = Maybe<int>.η(2);
            Func<short, Maybe<int>> valueSelector =  i => Maybe<int>.η(i);
            Func<short, int, long> resultSelector = (i, j) => i + j;

            // Extension method.
            Assert.Throws<ArgumentNullException>("selector", () => source.SelectMany(null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector", () => source.SelectMany(valueSelector, (Func<short, int, long>)null));
            // Static method.
            Assert.Throws<ArgumentNullException>("selector", () => Maybe.SelectMany(source, null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector", () => Maybe.SelectMany(source, valueSelector, (Func<short, int, long>)null));
        }

        #endregion

        #region Join()

        [Fact]
        public static void Join_Guards()
        {
            var source = Maybe<int>.η(1);
            var inner = Maybe<int>.η(2);
            Func<int, int> outerKeySelector = val => val;
            Func<int, int> innerKeySelector = val => val;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Extension method.
            Assert.Throws<ArgumentNullException>("outerKeySelector",
                () => source.Join(inner, (Func<int, int>)null, innerKeySelector, resultSelector));
            Assert.Throws<ArgumentNullException>("innerKeySelector",
                () => source.Join(inner, outerKeySelector, (Func<int, int>)null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector",
                () => source.Join(inner, outerKeySelector, innerKeySelector, (Func<int, int, int>)null));
            Assert.Throws<ArgumentNullException>("comparer",
                () => source.Join(inner, outerKeySelector, innerKeySelector, resultSelector, null));
            // Static method.
            Assert.Throws<ArgumentNullException>("outerKeySelector",
                () => Maybe.Join(source, inner, (Func<int, int>)null, innerKeySelector, resultSelector));
            Assert.Throws<ArgumentNullException>("innerKeySelector",
                () => Maybe.Join(source, inner, outerKeySelector, (Func<int, int>)null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector",
                () => Maybe.Join(source, inner, outerKeySelector, innerKeySelector, (Func<int, int, int>)null));
            Assert.Throws<ArgumentNullException>("comparer",
                () => Maybe.Join(source, inner, outerKeySelector, innerKeySelector, resultSelector, null));
        }

        #endregion

        #region GroupJoin()

        [Fact]
        public static void GroupJoin_Guards()
        {
            var source = Maybe<int>.η(1);
            var inner = Maybe<int>.η(2);
            Func<int, int> outerKeySelector = val => val;
            Func<int, int> innerKeySelector = val => val;
            Func<int, Maybe<int>, int> resultSelector = (i, m) => 1;

            // Extension method.
            Assert.Throws<ArgumentNullException>("outerKeySelector",
                () => source.GroupJoin(inner, (Func<int, int>)null, innerKeySelector, resultSelector));
            Assert.Throws<ArgumentNullException>("innerKeySelector",
                () => source.GroupJoin(inner, outerKeySelector, (Func<int, int>)null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector",
                () => source.GroupJoin(inner, outerKeySelector, innerKeySelector, (Func<int, Maybe<int>, int>)null));
            Assert.Throws<ArgumentNullException>("comparer",
                () => source.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, null));
            // Static method.
            Assert.Throws<ArgumentNullException>("outerKeySelector",
                () => Maybe.GroupJoin(source, inner, (Func<int, int>)null, innerKeySelector, resultSelector));
            Assert.Throws<ArgumentNullException>("innerKeySelector",
                () => Maybe.GroupJoin(source, inner, outerKeySelector, (Func<int, int>)null, resultSelector));
            Assert.Throws<ArgumentNullException>("resultSelector",
                () => Maybe.GroupJoin(source, inner, outerKeySelector, innerKeySelector, (Func<int, Maybe<int>, int>)null));
            Assert.Throws<ArgumentNullException>("comparer",
                () => Maybe.GroupJoin(source, inner, outerKeySelector, innerKeySelector, resultSelector, null));
        }

        #endregion

    }

#if !NO_INTERNALS_VISIBLE_TO

    // Provides tests for Maybe<T> that need access to internals.
    // T4: EmitMonadInternal().
    public static partial class MaybeFacts
    {
        #region Bind()

        [Fact]
        public static void Bind_AppliesBinder()
        {
            var source = Maybe<int>.η(1);
            Func<int, Maybe<int>> binder = val => Maybe<int>.η(2 * val);

            var me = source.Bind(binder);

            Assert.Equal(2, me.Value);
        }

        #endregion

        #region Select()

        [Fact]
        public static void Select_AppliesSelector()
        {
            var source = Maybe<int>.η(1);
            Func<int, int> selector = val => 2 * val;

            var me = source.Select(selector);

            Assert.Equal(2, me.Value);
        }

        #endregion
    }

#endif

    // Provides tests for Maybe<T>: functor, monoid and monad laws.
    // T4: EmitMonadRules().
    public static partial class MaybeFacts
    {
        #region Functor Rules

        [Property(DisplayName = "The identity map is a fixed point for Select (first functor law).")]
        public static bool Identity_IsFixedPointForSelect(int arg)
        {
            var me = Maybe<int>.η(arg);

            // fmap id  ==  id
            var left = me.Select(val => val);
            var right = me;

            return left.Equals(right);
        }

        [Property(DisplayName = "Select() preserves the composition operator (second functor law).")]
        public static bool Select_PreservesComposition(short arg, Func<short, int> g, Func<int, long> f)
        {
            var me = Maybe<short>.η(arg);

            // fmap (f . g)  ==  fmap f . fmap g
            var left = me.Select(val => f(g(val)));
            var right = me.Select(g).Select(f);

           return left.Equals(right);
        }

        #endregion

        #region Monoid Rules

        [Property(DisplayName = "None is a left identity for OrElse.")]
        public static bool None_IsLeftIdentityForOrElse(int arg)
        {
            var me = Maybe<int>.η(arg);

            // mappend mempty x  ==  x
            var left = Maybe<int>.None.OrElse(me);
            var right = me;

            return left.Equals(right);
        }

        [Property(DisplayName = "None is a right identity for OrElse.")]
        public static bool None_IsRightIdentityForOrElse(int arg)
        {
            var me = Maybe<int>.η(arg);

            // mappend x mempty ==  x
            var left = me.OrElse(Maybe<int>.None);
            var right = me;

            return left.Equals(right);
        }

        [Property(DisplayName = "OrElse() is associative.")]
        public static bool OrElse_IsAssociative(int arg0, int arg1, int arg2)
        {
            var x = Maybe<int>.η(arg0);
            var y = Maybe<int>.η(arg1);
            var z = Maybe<int>.η(arg2);

            // mappend x (mappend y z)  ==  mappend (mappend x y) z
            var left = x.OrElse(y.OrElse(z));
            var right = x.OrElse(y).OrElse(z);

            return left.Equals(right);
        }

        #endregion

        #region Monad Rules

        [Property(DisplayName = "Maybe.Of() is a left identity for Bind (first monad law).")]
        public static bool Of_IsLeftIdentityForBind(int arg0, float arg1)
        {
            Func<int, Maybe<float>> f = val => Maybe<float>.η(arg1 * val);

            // return a >>= k  ==  k a
            var left = Maybe<int>.η(arg0).Bind(f);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Maybe.Of() is a left identity for Compose (first monad law).")]
        public static bool Of_IsLeftIdentityForCompose(int arg0, float arg1)
        {
            Func<int, Maybe<int>> of = Maybe<int>.η;
            Func<int, Maybe<float>> f = val => Maybe<float>.η(arg1 * val);

            // return >=> g  ==  g
            var left = of.Compose(f).Invoke(arg0);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Maybe.Of() is a right identity for Bind (second monad law).")]
        public static bool Of_IsRightIdentityForBind(int arg0)
        {
            var me = Maybe<int>.η(arg0);

            // m >>= return  ==  m
            var left = me.Bind(Maybe<int>.η);
            var right = me;

            return left.Equals(right);
        }

        [Property(DisplayName = "Maybe.Of() is a right identity for Compose (second monad law).")]
        public static bool Of_IsRightIdentityForCompose(int arg0, float arg1)
        {
            Func<int, Maybe<float>> f = val => Maybe<float>.η(arg1 * val);

            // f >=> return  ==  f
            var left = f.Compose(Maybe<float>.η).Invoke(arg0);
            var right = f(arg0);

            return left.Equals(right);
        }

        [Property(DisplayName = "Bind() is associative (third monad law).")]
        public static bool Bind_IsAssociative(short arg0, int arg1, long arg2)
        {
            var me = Maybe<short>.η(arg0);

            Func<short, Maybe<int>> f = val => Maybe<int>.η(arg1 * val);
            Func<int, Maybe<long>> g = val => Maybe<long>.η(arg2 * val);

            // m >>= (\x -> f x >>= g)  ==  (m >>= f) >>= g
            var left = me.Bind(f).Bind(g);
            var right = me.Bind(val => f(val).Bind(g));

            return left.Equals(right);
        }

        [Property(DisplayName = "Compose() is associative (third monad law).")]
        public static bool Compose_IsAssociative(short arg0, int arg1, long arg2, double arg3)
        {
            Func<short, Maybe<int>> f = val => Maybe<int>.η(arg1 * val);
            Func<int, Maybe<long>> g = val => Maybe<long>.η(arg2 * val);
            Func<long, Maybe<double>> h = val => Maybe<double>.η(arg3 * val);

            // f >=> (g >=> h)  ==  (f >=> g) >=> h
            var left = f.Compose(g.Compose(h)).Invoke(arg0);
            var right = f.Compose(g).Compose(h).Invoke(arg0);

            return left.Equals(right);
        }

        #endregion

        #region Monad Plus Rules

        [Property(DisplayName = "None is is a left zero for Bind (monad zero rule).")]
        public static bool None_IsLeftZeroForBind(long arg0)
        {
            Func<int, Maybe<long>> f = val => Maybe<long>.η(arg0 * val);

            // mzero >>= f  ==  mzero
            var left = Maybe<int>.None.Bind(f);
            var right = Maybe<long>.None;

            return left.Equals(right);
        }

        [Property(DisplayName = "None is is a right zero for Bind (monad more rule).")]
        public static bool None_IsRightZeroForBind(int arg0)
        {
            // m >>= (\x -> mzero) = mzero
            var left = Maybe<int>.η(arg0).Bind(_ => Maybe<long>.None);
            var right = Maybe<long>.None;

            return left.Equals(right);
        }

        [Fact]
        public static void Satisfies_MonadOrRule()
        {
            // Arrange
            var monad = Maybe<int>.η(2);

            // Act
            var leftSome = monad.OrElse(Maybe<int>.η(1));
            var leftNone = monad.OrElse(Maybe<int>.None);
            var right = monad;

            // Assert
            Assert.True(leftSome.Equals(right));
            Assert.True(leftNone.Equals(right));
        }

        [Fact]
        public static void DoesNotSatisfyRightZeroForPlus()
        {
            // Arrange
            var monad = Maybe<int>.η(2);

            // Act
            var leftSome = Maybe<int>.η(1).OrElse(monad);
            var leftNone = Maybe<int>.None.OrElse(monad);
            var right = monad;

            // Assert
            Assert.False(leftSome.Equals(right));   // NB: Fails here the "Unit is a right zero for Plus".
            Assert.True(leftNone.Equals(right));
        }

        #endregion
    }
}

