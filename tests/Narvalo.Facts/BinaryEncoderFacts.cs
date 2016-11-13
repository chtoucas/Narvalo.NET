// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Xunit;

    public static partial class BinaryEncoderFacts
    {
        public static IEnumerable<object[]> ZBase32TestData
        {
            get
            {
                yield return new object[] { String.Empty, String.Empty };
                yield return new object[] { "BA", "ejyo" };
                yield return new object[] { "a", "cr" };
            }
        }

        #region ToZBase32String()

        //[Fact]
        //public static void ToZBase32String_ThrowsArgumentNullException_ForNullInput()
        //{
        //    // Act & Assert
        //    Assert.Throws<ArgumentNullException>(() => BinaryEncoder.ToZBase32String(null));
        //}

        //[Fact]
        //public static void ToZBase32String_XXX()
        //{
        //    // Arrange
        //    byte[] value = new byte[] { 0xF0, 0xBF, 0xC7 };

        //    // Act & Assert
        //    Assert.Equal("6n9hq", BinaryEncoder.ToZBase32String(value));
        //}

        //[Theory]
        //[MemberData(nameof(ZBase32TestData), DisableDiscoveryEnumeration = true)]
        //[CLSCompliant(false)]
        //public static void ToZBase32String_ReturnsExpectedString(string value, string expectedValue)
        //{
        //    // Act & Assert
        //    Assert.Equal(expectedValue, BinaryEncoder.ToZBase32String(Encoding.ASCII.GetBytes(value)));
        //}

        #endregion
    }
}
