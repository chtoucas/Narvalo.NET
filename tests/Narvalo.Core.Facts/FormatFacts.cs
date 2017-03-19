// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class FormatFacts
    {
        #region Current()

        [Fact]
        public static void Current1_ThrowsArgumentNullException_ForNullFormat()
            => Assert.Throws<ArgumentNullException>(() => Format.Current(null, 1));

        [Fact]
        public static void Current2_ThrowsArgumentNullException_ForNullFormat()
            => Assert.Throws<ArgumentNullException>(() => Format.Current(null, 1, 2));

        [Fact]
        public static void Current3_ThrowsArgumentNullException_ForNullFormat()
            => Assert.Throws<ArgumentNullException>(() => Format.Current(null, 1, 2, 3));

        [Fact]
        public static void Current1_DoesNotThrow_ForNullArgs()
            => Format.Current("{0}", (string)null);

        [Fact]
        public static void Current2_DoesNotThrow_ForNullArgs()
            => Format.Current("{0}{1}", (string)null, (string)null);

        [Fact]
        public static void Current3_DoesNotThrow_ForNullArgs()
            => Format.Current("{0}{1}{2}", (string)null, (string)null, (string)null);

        [Fact]
        public static void Current1_ThrowsFormatException_ForInvalidFormat()
            => Assert.Throws<FormatException>(() => Format.Current("{0} {1}", 1));

        [Fact]
        public static void Current2_ThrowsFormatException_ForInvalidFormat()
            => Assert.Throws<FormatException>(() => Format.Current("{0} {1} {2}", 1, 2));

        [Fact]
        public static void Current3_ThrowsFormatException_ForInvalidFormat()
            => Assert.Throws<FormatException>(() => Format.Current("{0} {1} {2} {3}", 1, 2, 3));

        #endregion

        #region Invariant()

        [Fact]
        public static void Invariant1_ThrowsArgumentNullException_ForNullFormat()
            => Assert.Throws<ArgumentNullException>(() => Format.Invariant(null, 1));

        [Fact]
        public static void Invariant2_ThrowsArgumentNullException_ForNullFormat()
            => Assert.Throws<ArgumentNullException>(() => Format.Invariant(null, 1, 2));

        [Fact]
        public static void Invariant3_ThrowsArgumentNullException_ForNullFormat()
            => Assert.Throws<ArgumentNullException>(() => Format.Invariant(null, 1, 2, 3));

        [Fact]
        public static void Invariant1_DoesNotThrow_ForNullArgs()
            => Format.Invariant("{0}", (string)null);

        [Fact]
        public static void Invariant2_DoesNotThrow_ForNullArgs()
            => Format.Invariant("{0}{1}", (string)null, (string)null);

        [Fact]
        public static void Invariant3_DoesNotThrow_ForNullArgs()
            => Format.Invariant("{0}{1}{2}", (string)null, (string)null, (string)null);

        [Fact]
        public static void Invariant1_ThrowsFormatException_ForInvalidFormat()
            => Assert.Throws<FormatException>(() => Format.Invariant("{0} {1}", 1));

        [Fact]
        public static void Invariant2_ThrowsFormatException_ForInvalidFormat()
            => Assert.Throws<FormatException>(() => Format.Invariant("{0} {1} {2}", 1, 2));

        [Fact]
        public static void Invariant3_ThrowsFormatException_ForInvalidFormat()
            => Assert.Throws<FormatException>(() => Format.Invariant("{0} {1} {2} {3}", 1, 2, 3));

        #endregion
    }
}
