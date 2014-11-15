// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using NSubstitute;
    using Xunit;

    public static class MvpPageFacts
    {
        public static class ThrowIfNoPresenterBoundProperty
        {
            [Fact]
            public static void IsTrue_ForDefautConstructor()
            {
                // Arrange
                var page = Substitute.For<MvpPage>();

                // Assert
                Assert.True(page.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsTrue_ForDefautConstructor_WhenGeneric()
            {
                // Arrange
                var page = Substitute.For<MvpPage<Object>>();

                // Assert
                Assert.True(page.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsFalse_ForCustomConstructor()
            {
                // Arrange
                var page = Substitute.For<MvpPage>(false);

                // Assert
                Assert.False(page.ThrowIfNoPresenterBound);
            }

            [Fact]
            public static void IsFalse_ForCustomConstructor_WhenGeneric()
            {
                // Arrange
                var page = Substitute.For<MvpPage<Object>>(false);

                // Assert
                Assert.False(page.ThrowIfNoPresenterBound);
            }
        }
    }
}