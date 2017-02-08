// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using Xunit;

    public static class OptimizationSectionFacts
    {
        #region EnableWhiteSpaceBusting

        [Fact]
        public static void EnableWhiteSpaceBusting_DefaultIsFalse()
        {
            // Arrange
            var section = new OptimizationSection();

            // Assert
            Assert.False(section.EnableWhiteSpaceBusting);
        }

        [Fact]
        public static void EnableWhiteSpaceBusting_SetterThenGetter()
        {
            // Arrange
            var section = new OptimizationSection();

            // Act & Assert
            section.EnableWhiteSpaceBusting = true;
            Assert.True(section.EnableWhiteSpaceBusting);

            section.EnableWhiteSpaceBusting = false;
            Assert.False(section.EnableWhiteSpaceBusting);
        }

        #endregion
    }
}
