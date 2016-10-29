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


namespace Narvalo.Fx
{
    using System;

    using Xunit;

    public static partial class MaybeFacts
    {
        #region Linq Operators

        [Fact]
        public static void Select_ThrowsArgumentNullException_ForNullSelector()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, int> selector = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.Select(selector));
        }


        [Fact]
        public static void Where_ThrowsArgumentNullException_ForNullPredicate()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, bool> predicate = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.Where(predicate));
        }


        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullValueSelector()
        {
            // Arrange
            var source = Maybe.Of(1);
            Func<int, Maybe<int>> valueSelector = null;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        }

        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullResultSelector()
        {
            // Arrange
            var source = Maybe.Of(1);
            var middle = Maybe.Of(2);
            Func<int, Maybe<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        }

        #endregion

        #region Monad Laws


        [Fact]
        public static void Maybe_SatisfiesFirstMonoidLaw()
        {
            // Arrange
            var monad = Maybe.Of(1);

            // Act
            var left = Maybe<int>.None.OrElse(monad);
            var right = monad;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesSecondMonoidLaw()
        {
            // Arrange
            var monad = Maybe.Of(1);

            // Act
            var left = monad.OrElse(Maybe<int>.None);
            var right = monad;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesThirdMonoidLaw()
        {
            // Arrange
            var monadA = Maybe.Of(1);
            var monadB = Maybe.Of(2);
            var monadC = Maybe.Of(3);

            // Act
            var left = monadA.OrElse(monadB.OrElse(monadC));
            var right = monadA.OrElse(monadB).OrElse(monadC);

            // Assert
            Assert.True(left.Equals(right));
        }


        [Fact]
        public static void Maybe_SatisfiesFirstMonadLaw()
        {
            // Arrange
            int value = 1;
            Func<int, Maybe<long>> kun = _ => Maybe.Of((long)2 * _);

            // Act
            var left = Maybe.Of(value).Bind(kun);
            var right = kun(value);

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesSecondMonadLaw()
        {
            // Arrange
            Func<int, Maybe<int>> create = _ => Maybe.Of(_);
            var monad = Maybe.Of(1);

            // Act
            var left = monad.Bind(create);
            var right = monad;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesThirdMonadLaw()
        {
            // Arrange
            Maybe<short> m = Maybe.Of((short)1);
            Func<short, Maybe<int>> f = _ => Maybe.Of((int)3 * _);
            Func<int, Maybe<long>> g = _ => Maybe.Of((long)2 * _);

            // Act
            var left = m.Bind(f).Bind(g);
            var right = m.Bind(_ => f(_).Bind(g));

            // Assert
            Assert.True(left.Equals(right));
        }


        [Fact]
        public static void Maybe_SatisfiesMonadZeroRule()
        {
            // Arrange
            Func<int, Maybe<long>> kun = _ => Maybe.Of((long)2 * _);

            // Act
            var left = Maybe<int>.None.Bind(kun);
            var right = Maybe<long>.None;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Maybe_SatisfiesMonadMoreRule()
        {
            // Act
            var leftSome = Maybe.Of(1).Bind(_ => Maybe<int>.None);
            var leftNone = Maybe<int>.None.Bind(_ => Maybe<int>.None);
            var right = Maybe<int>.None;

            // Assert
            Assert.True(leftSome.Equals(right));
            Assert.True(leftNone.Equals(right));
        }


        [Fact]
        public static void Maybe_SatisfiesMonadOrRule()
        {
            // Arrange
            var monad = Maybe.Of(2);

            // Act
            var leftSome = monad.OrElse(Maybe.Of(1));
            var leftNone = monad.OrElse(Maybe<int>.None);
            var right = monad;

            // Assert
            Assert.True(leftSome.Equals(right));
            Assert.True(leftNone.Equals(right));
        }

        [Fact]
        public static void Maybe_DoesNotSatisfyRightZeroForPlus()
        {
            // Arrange
            var monad = Maybe.Of(2);

            // Act
            var leftSome = Maybe.Of(1).OrElse(monad);
            var leftNone = Maybe<int>.None.OrElse(monad);
            var right = monad;

            // Assert
            Assert.False(leftSome.Equals(right));   // NB: Fails here the "Unit is a right zero for Plus".
            Assert.True(leftNone.Equals(right));
        }


        #endregion
    } // End of Maybe.
} // End of Narvalo.Fx.
