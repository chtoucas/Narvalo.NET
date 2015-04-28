// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    using NSubstitute;
    using Xunit;

    public static class PresenterBindingAttributeFacts
    {
        #region Ctor()

        [Fact]
        public static void Ctor_ThrowsArgumentNullException_ForNullPresenterType()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PresenterBindingAttribute(presenterType: null));
        }

        #endregion

        #region BindingMode

        [Fact]
        public static void BindingMode_IsDefault_ForDefaultConstructor()
        {
            // Arrange
            var presenterType = Substitute.For<IPresenter<IView>>().GetType();

            // Act
            var attribute = new PresenterBindingAttribute(presenterType);

            // Assert
            Assert.Equal(PresenterBindingMode.Default, attribute.BindingMode);
        }

        #endregion

        #region PresenterType

        [Fact]
        public static void PresenterType_IsSetCorrectly()
        {
            // Arrange
            var presenterType = Substitute.For<IPresenter<IView>>().GetType();

            // Act
            var attribute = new PresenterBindingAttribute(presenterType);

            // Assert
            Assert.Equal(presenterType, attribute.PresenterType);
        }

        #endregion

        #region ViewType

        [Fact]
        public static void ViewType_IsNull_ForDefaultConstructor()
        {
            // Arrange
            var presenterType = Substitute.For<IPresenter<IView>>().GetType();

            // Act
            var attribute = new PresenterBindingAttribute(presenterType);

            // Assert
            Assert.Null(attribute.ViewType);
        }

        #endregion
    }
}