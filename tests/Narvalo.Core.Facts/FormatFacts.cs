// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class FormatFacts
    {
        #region Current()

        [Fact]
        public static void Current_ThrowsArgumentNullException_ForNullFormat()
            => Assert.Throws<ArgumentNullException>(() => Format.Current(null));

        [Fact]
        public static void Current_ThrowsArgumentNullException_ForNullArgs()
            => Assert.Throws<ArgumentNullException>(() => Format.Current(String.Empty, null));

        [Fact]
        public static void Current_ThrowsFormatException_ForInvalidFormat()
            => Assert.Throws<FormatException>(() => Format.Current("{0}"));

        [Fact]
        public static void Current_ReturnsNonNull()
            => Assert.NotNull(Format.Current(String.Empty));

        #endregion

        #region Invariant()

        [Fact]
        public static void Invariant_ThrowsArgumentNullException_ForNullFormat()
            => Assert.Throws<ArgumentNullException>(() => Format.Invariant(null));

        [Fact]
        public static void Invariant_ThrowsArgumentNullException_ForNullArgs()
            => Assert.Throws<ArgumentNullException>(() => Format.Invariant(String.Empty, null));

        [Fact]
        public static void Invariant_ThrowsFormatException_ForInvalidFormat()
            => Assert.Throws<FormatException>(() => Format.Invariant("{0}"));

        [Fact]
        public static void Invariant_ReturnsNonNull()
            => Assert.NotNull(Format.Invariant(String.Empty));

        #endregion
    }
}
