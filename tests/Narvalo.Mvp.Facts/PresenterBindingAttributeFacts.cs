// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using NSubstitute;
    using Xunit;

    public static class PresenterBindingAttributeFacts
    {
        public static class Ctor
        {
            [Fact]
            public static void ThrowsArgumentNullException_ForNullPresenterType()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => new PresenterBindingAttribute(presenterType: null));
            }
        }

        public static class BindingModeProperty
        {
            [Fact]
            public static void IsDefault_ForDefaultConstructor()
            {
                // Arrange
                var presenterType = Substitute.For<IPresenter<IView>>().GetType();

                // Act
                var attribute = new PresenterBindingAttribute(presenterType);

                // Assert
                Assert.Equal(PresenterBindingMode.Default, attribute.BindingMode);
            }
        }

        public static class PresenterTypeProperty
        {
            [Fact]
            public static void IsSetCorrectly()
            {
                // Arrange
                var presenterType = Substitute.For<IPresenter<IView>>().GetType();

                // Act
                var attribute = new PresenterBindingAttribute(presenterType);

                // Assert
                Assert.Equal(presenterType, attribute.PresenterType);
            }
        }

        public static class ViewTypeProperty
        {
            [Fact]
            public static void IsNull_ForDefaultConstructor()
            {
                // Arrange
                var presenterType = Substitute.For<IPresenter<IView>>().GetType();

                // Act
                var attribute = new PresenterBindingAttribute(presenterType);

                // Assert
                Assert.Null(attribute.ViewType);
            }
        }
    }
}