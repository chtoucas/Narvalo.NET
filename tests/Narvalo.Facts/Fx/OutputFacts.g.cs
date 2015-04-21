// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    using Xunit;

    public static partial class OutputFacts
    {
        #region Linq Operators

        [Fact]
        public static void Select_ThrowsArgumentNullException_ForNullSelector()
        {
            // Arrange
            var source = Output.Success(1);
            Func<int, int> selector = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.Select(selector));
        }


        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullValueSelector()
        {
            // Arrange
            var source = Output.Success(1);
            Func<int, Output<int>> valueSelector = null;
            Func<int, int, int> resultSelector = (i, j) => i + j;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        }

        [Fact]
        public static void SelectMany_ThrowsArgumentNullException_ForNullResultSelector()
        {
            // Arrange
            var source = Output.Success(1);
            var middle = Output.Success(2);
            Func<int, Output<int>> valueSelector = _ => middle;
            Func<int, int, int> resultSelector = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.SelectMany(valueSelector, resultSelector));
        }

        #endregion
        
        #region Monad Laws
        

        [Fact]
        public static void Output_SatisfiesFirstMonadLaw()
        {
            // Arrange
            int value = 1;
            Func<int, Output<long>> kun = _ => Output.Success((long)2 * _);

            // Act
            var left = Output.Success(value).Bind(kun);
            var right = kun(value);

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Output_SatisfiesSecondMonadLaw()
        {
            // Arrange
            Func<int, Output<int>> create = _ => Output.Success(_);
            var option = Output.Success(1);

            // Act
            var left = option.Bind(create);
            var right = option;

            // Assert
            Assert.True(left.Equals(right));
        }

        [Fact]
        public static void Output_SatisfiesThirdMonadLaw()
        {
            // Arrange
            Output<short> m = Output.Success((short)1);
            Func<short, Output<int>> f = _ => Output.Success((int)3 * _);
            Func<int, Output<long>> g = _ => Output.Success((long)2 * _);

            // Act
            var left = m.Bind(f).Bind(g);
            var right = m.Bind(_ => f(_).Bind(g));

            // Assert
            Assert.True(left.Equals(right));
        }
        

        #endregion
    }
}
