// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using Moq;
    using Xunit;

    public static class MvpPageFacts
    {
        public static class TheThrowIfNoPresenterBoundProperty
        {
            [Fact]
            public static void IsTrue_ForDefautConstructor()
            {
                // Arrange
                var page = new Mock<MvpPage>().Object;

                // Assert
                Assert.True(page.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsTrue_ForDefautConstructor_WhenGeneric()
            {
                // Arrange
                var page = new Mock<MvpPage<object>>().Object;

                // Assert
                Assert.True(page.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsFalse_ForCustomConstructor()
            {
                // Arrange
                var page = new Mock<MvpPage>(false).Object;

                // Assert
                Assert.False(page.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsFalse_ForCustomConstructor_WhenGeneric()
            {
                // Arrange
                var page = new Mock<MvpPage<object>>(false).Object;

                // Assert
                Assert.False(page.ThrowIfNoPresenterBound);
            }
        }
    }
}