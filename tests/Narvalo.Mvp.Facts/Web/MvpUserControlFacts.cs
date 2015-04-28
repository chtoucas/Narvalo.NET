// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;

    using NSubstitute;
    using Xunit;

    public static class MvpUserControlFacts
    {
        #region ThrowIfNoPresenterBound

        [Fact]
        public static void ThrowIfNoPresenterBound_IsTrue_ForDefautConstructor()
        {
            // Arrange & Act
            var control = Substitute.For<MvpUserControl>();

            // Assert
            Assert.True(control.ThrowIfNoPresenterBound);
        }

        [Fact]
        public static void ThrowIfNoPresenterBound_IsTrue_ForDefautConstructor_WhenGeneric()
        {
            // Arrange & Act
            var control = Substitute.For<MvpUserControl<Object>>();

            // Assert
            Assert.True(control.ThrowIfNoPresenterBound);
        }

        [Fact]
        public static void ThrowIfNoPresenterBound_IsFalse_ForCustomConstructor()
        {
            // Arrange & Act
            var control = Substitute.For<MvpUserControl>(false);

            // Assert
            Assert.False(control.ThrowIfNoPresenterBound);
        }

        [Fact]
        public static void ThrowIfNoPresenterBound_IsFalse_ForCustomConstructor_WhenGeneric()
        {
            // Arrange & Act
            var control = Substitute.For<MvpUserControl<Object>>(false);

            // Assert
            Assert.False(control.ThrowIfNoPresenterBound);
        }

        #endregion
    }
}