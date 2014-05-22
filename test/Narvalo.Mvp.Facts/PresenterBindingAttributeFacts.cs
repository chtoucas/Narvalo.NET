// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using Xunit;

    public static class PresenterBindingAttributeFacts
    {
        public static class TheConstructor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullPresenterType()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new PresenterBindingAttribute(null));
            }
        }

        public static class TheBindingModeProperty
        {
            [Fact]
            public static void IsDefault_ForDefaultConstructor()
            {
                // Arrange
                var presenterType = typeof(Stubs.PresenterForIView);
                // Act
                var attribute = new PresenterBindingAttribute(presenterType);
                // Assert
                Assert.Equal(PresenterBindingMode.Default, attribute.BindingMode);
            }
        }

        public static class ThePresenterTypeProperty
        {
            [Fact]
            public static void IsSetCorrectly()
            {
                // Arrange
                var presenterType = typeof(Stubs.PresenterForIView);
                // Act
                var attribute = new PresenterBindingAttribute(presenterType);
                // Assert
                Assert.Equal(presenterType, attribute.PresenterType);
            }
        }

        public static class TheViewTypeProperty
        {
            [Fact]
            public static void IsNull_ForDefaultConstructor()
            {
                // Arrange
                var presenterType = typeof(Stubs.PresenterForIView);
                // Act
                var attribute = new PresenterBindingAttribute(presenterType);
                // Assert
                Assert.Null(attribute.ViewType);
            }
        }
    }
}