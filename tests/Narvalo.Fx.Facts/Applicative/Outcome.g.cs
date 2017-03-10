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

    using Xunit;

    public static partial class OutcomeFacts
    {
        #region Select()

        [Fact]
        public static void Select_ThrowsArgumentNullException_ForNullSelector()
        {
            var source = Outcome.Of(1);
            Func<int, int> selector = null;

            Assert.Throws<ArgumentNullException>(() => source.Select(selector));
        }

        #endregion

        #region SelectMany()

        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullValueSelector()
        {
            var source = Outcome.Of(1);
            Func<int, Outcome<int>> valueSelector = null;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        }

        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullResultSelector()
        {
            var source = Outcome.Of(1);
            var middle = Outcome.Of(2);
            Func<int, Outcome<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = null;

            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        }

        #endregion

        #region Functor Rules

        [Fact(DisplayName = "The identity map is a fixed point for Select.")]
        public static void Satisfies_FirstFunctorLaw()
        {
            // Arrange
            var me = Outcome.Of(1);

            // Act
            var left = me.Select(Stubs<int>.Identity);
            var right = Stubs<Outcome<int>>.Identity(me);

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact(DisplayName = "Select preserves the composition operator.")]
        public static void Satisfies_FunctorSecondRule()
        {
            // Arrange
            var me = Outcome.Of(1);
            Func<int, long> g = val => (long)2 * val;
            Func<long, long> f = val => 3 * val;

            // Act
            var left = me.Select(_ => f(g(_)));
            var right = me.Select(g).Select(f);

            // Assert
            Assert.True(left.Equals(right));
        }

        #endregion

        #region Monad Rules

        [Fact(DisplayName = "Of is a left identity for Bind.")]
        public static void Satisfies_FirstMonadRule()
        {
            // Arrange
            int value = 1;
            Func<int, Outcome<long>> binder = val => Outcome.Of((long)2 * val);

            // Act
            var left = Outcome.Of(value).Bind(binder);
            var right = binder(value);

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact(DisplayName = "Of is a right identity for Bind.")]
        public static void Satisfies_SecondMonadRule()
        {
            // Arrange
            Func<int, Outcome<int>> create = val => Outcome.Of(val);
            var monad = Outcome.Of(1);

            // Act
            var left = monad.Bind(create);
            var right = monad;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact(DisplayName = "Bind is associative.")]
        public static void Satisfies_ThirdMonadRule()
        {
            // Arrange
            Outcome<short> m = Outcome.Of((short)1);
            Func<short, Outcome<int>> f = val => Outcome.Of((int)3 * val);
            Func<int, Outcome<long>> g = val => Outcome.Of((long)2 * val);

            // Act
            var left = m.Bind(f).Bind(g);
            var right = m.Bind(val => f(val).Bind(g));

            // Assert
            Assert.True(left.Equals(right));
        }

        #endregion
    }
}
