namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using Xunit;

    public static class ParseToFacts
    {
        #region Stubs

        struct StructStub_ { }

        enum EnumStub_
        {
            None = 0,
            ActualValue = 1,
            AliasValue = ActualValue,
        }

        enum EnumWithoutDefaultStub_
        {
            ActualValue1 = 1,
            ActualValue2 = 2,
        }

        [Flags]
        enum EnumFlagStub_
        {
            None = 0,
            ActualValue1 = 1 << 0,
            ActualValue2 = 1 << 1,
            ActualValue3 = 1 << 2,
            CompositeValue1 = ActualValue1 | ActualValue2,
            CompositeValue2 = ActualValue1 | ActualValue2 | ActualValue3
        }

        #endregion

        public static class TheEnumMethod
        {
            //// Validation du paramètre générique
            
#if DEBUG
            [Fact(Skip = Constants.SkipReleaseOnly)]
#else
            [Fact]
#endif
            public static void ThrowsArgumentException_WithInt32()
            {
                // Act & Assert
                Assert.Throws<ArgumentException>(() => ParseTo.Enum<int>("Whatever"));
            }

            
#if DEBUG
            [Fact(Skip = Constants.SkipReleaseOnly)]
#else
            [Fact]
#endif
            public static void ThrowsArgumentException_WithStruct()
            {
                // Act & Assert
                Assert.Throws<ArgumentException>(() => ParseTo.Enum<StructStub_>("Whatever"));
            }

            //// Analyse d'une valeur valide

            [Fact]
            public static void ReturnsCorrectMember_ForActualValue()
            {
                // Act
                EnumStub_ result = ParseTo.Enum<EnumStub_>("ActualValue");
                // Assert
                Assert.Equal(EnumStub_.ActualValue, result);
            }

            [Fact]
            public static void ReturnsCorrectMember_ForActualValue_WhenIgnoreCase()
            {
                // Act
                EnumStub_ result = ParseTo.Enum<EnumStub_>("actualvalue", true /* ignoreCase */);
                // Assert
                Assert.Equal(EnumStub_.ActualValue, result);
            }

            [Fact]
            public static void ThrowsArgumentException_ForActualValueAndBadCase()
            {
                // Act & Assert
                Assert.Throws<ArgumentException>(
                    () => ParseTo.Enum<EnumStub_>("actualvalue", false /* ignoreCase */));
            }

            //// Analyse d'une valeur invalide

            [Fact]
            public static void ThrowsArgumentException_ForInvalidValue()
            {
                // Act & Assert
                Assert.Throws<ArgumentException>(
                    () => ParseTo.Enum<EnumStub_>("InvalidValue"));
            }

            [Fact]
            public static void ThrowsArgumentException_ForInvalidValue_WhenIgnoreCase()
            {
                // Act & Assert
                Assert.Throws<ArgumentException>(
                    () => ParseTo.Enum<EnumStub_>("invalidvalue", true /* ignoreCase */));
            }

            [Fact]
            public static void ThrowsArgumentException_ForInvalidValueAndBadCase()
            {
                // Act & Assert
                Assert.Throws<ArgumentException>(
                    () => ParseTo.Enum<EnumStub_>("invalidvalue", false /* ignoreCase */));
            }
        }
    }
}
