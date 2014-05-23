// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using Moq;
    using Xunit;

    public static class MvpUserControlFacts
    {
        public static class ThrowIfNoPresenterBoundProperty
        {
            [Fact]
            public static void IsTrue_ForDefautConstructor()
            {
                // Arrange & Act
                var control = new Mock<MvpUserControl>().Object;

                // Assert
                Assert.True(control.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsTrue_ForDefautConstructor_WhenGeneric()
            {
                // Arrange & Act
                var control = new Mock<MvpUserControl<Object>>().Object;

                // Assert
                Assert.True(control.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsFalse_ForCustomConstructor()
            {
                // Arrange & Act
                var control = new Mock<MvpUserControl>(false).Object;

                // Assert
                Assert.False(control.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsFalse_ForCustomConstructor_WhenGeneric()
            {
                // Arrange & Act
                var control = new Mock<MvpUserControl<Object>>(false).Object;

                // Assert
                Assert.False(control.ThrowIfNoPresenterBound);
            }
        }
    }
}