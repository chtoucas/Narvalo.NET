// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class FormatFacts
    {
        [Fact]
        public static void Current_Guards()
        {
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1));
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1, 2));
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1, 2, 3));
        }

        [Fact]
        public static void Current_DoesNotThrow_ForNullArgs()
        {
            Format.Current("{0}", (string)null);
            Format.Current("{0}{1}", (string)null, (string)null);
            Format.Current("{0}{1}{2}", (string)null, (string)null, (string)null);
        }

        [Fact]
        public static void Current_ThrowsFormatException_ForInvalidFormat()
        {
            Assert.Throws<FormatException>(() => Format.Current("{0} {1}", 1));
            Assert.Throws<FormatException>(() => Format.Current("{0} {1} {2}", 1, 2));
            Assert.Throws<FormatException>(() => Format.Current("{0} {1} {2} {3}", 1, 2, 3));
        }
    }
}
