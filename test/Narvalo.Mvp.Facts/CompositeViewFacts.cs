// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Linq;
    using Moq;
    using Xunit;

    public static class CompositeViewFacts
    {
        public static class TheAddMethod
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullView()
            {
                // Arrange
                var compositeView = new Mock<CompositeView<IView<Object>>>().Object;

                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => compositeView.Add(view: null));
            }

            [Fact]
            public static void ThrowsArgumentException_ForViewOfWrongType()
            {
                // Arrange
                var compositeView = new Mock<CompositeView<IView<Object>>>().Object;

                // Act & Assert
                Assert.Throws<ArgumentException>(() => compositeView.Add(new Mock<IView>().Object));
            }

            [Fact]
            public static void AddsViewsToList()
            {
                // Arrange
                var compositeView = new Mock<CompositeView<IView<Object>>>().Object;
                var view1 = new Mock<IView<Object>>().Object;
                var view2 = new Mock<IView<Object>>().Object;

                // Act
                compositeView.Add(view1);
                compositeView.Add(view2);

                // Assert
                var expected = new[] { view1, view2 };
                Assert.True(expected.SequenceEqual(compositeView.Views));
            }
        }
    }
}