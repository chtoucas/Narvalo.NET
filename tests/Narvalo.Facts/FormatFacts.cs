// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class FormatFacts
    {
        #region CurrentCulture()

        [Fact]
        public static void CurrentCulture_ThrowsArgumentNullException_ForNullFormat()
        {
            Assert.Throws<ArgumentNullException>(() => Format.CurrentCulture(null));
        }

        [Fact]
        public static void CurrentCulture_ThrowsArgumentNullException_ForNullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => Format.CurrentCulture(String.Empty, null));
        }

        [Fact]
        public static void CurrentCulture_ThrowsFormatException_ForInvalidFormat()
        {
            Assert.Throws<FormatException>(() => Format.CurrentCulture("{0}"));
        }

        [Fact]
        public static void CurrentCulture_ReturnsNonNull()
        {
            Assert.NotNull(Format.CurrentCulture(String.Empty));
        }

        #endregion

        #region InvariantCulture()

        [Fact]
        public static void InvariantCulture_ThrowsArgumentNullException_ForNullFormat()
        {
            Assert.Throws<ArgumentNullException>(() => Format.InvariantCulture(null));
        }

        [Fact]
        public static void InvariantCulture_ThrowsArgumentNullException_ForNullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => Format.InvariantCulture(String.Empty, null));
        }

        [Fact]
        public static void InvariantCulture_ThrowsFormatException_ForInvalidFormat()
        {
            Assert.Throws<FormatException>(() => Format.InvariantCulture("{0}"));
        }

        [Fact]
        public static void InvariantCulture_ReturnsNonNull()
        {
            Assert.NotNull(Format.InvariantCulture(String.Empty));
        }

        #endregion

        #region Resource()

        [Fact]
        public static void Resource_ThrowsArgumentNullException_ForNullFormat()
        {
            Assert.Throws<ArgumentNullException>(() => Format.Resource(null));
        }

        [Fact]
        public static void Resource_ThrowsArgumentNullException_ForNullArgs()
        {
            Assert.Throws<ArgumentNullException>(() => Format.Resource(String.Empty, null));
        }

        [Fact]
        public static void Resource_ThrowsFormatException_ForInvalidFormat()
        {
            Assert.Throws<FormatException>(() => Format.Resource("{0}"));
        }

        [Fact]
        public static void Resource_ReturnsNonNull()
        {
            Assert.NotNull(Format.Resource(String.Empty));
        }

        #endregion
    }
}
