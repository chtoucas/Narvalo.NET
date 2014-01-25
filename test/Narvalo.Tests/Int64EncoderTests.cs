namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public static partial class Int64EncoderTests
    {
        [Fact(DisplayName = "Int64Encoder.ToBase58String() then FromBase58String() is invariant")]
        public static void RoundTrip_Succeeds()
        {
            // Arrange
            long value = 3471391110;
            // Act & Assert
            Assert.Equal(value, Int64Encoder.FromBase58String(Int64Encoder.ToBase58String(value)));
        }

        public static class FromBase58String
        {
            public static IEnumerable<object[]> SampleData
            {
                get
                {
                    yield return new object[] { String.Empty, 0 };
                    yield return new object[] { "NQm6nKp8qFC", Int64.MaxValue };
                }
            }

            [Fact(DisplayName = "Int64Encoder.FromBase58String() null throws.")]
            public static void Decode_Null_Throws()
            {
                // Act & Assert
                Assert.Throws<ArgumentNullException>(() => Int64Encoder.FromBase58String(null));
            }

            [Theory(DisplayName = "Int64Encoder.FromBase58String() on sample data.")]
            [PropertyData("SampleData")]
            public static void Decode_SampleData_Succeeds(string value, long expectedValue)
            {
                // Act & Assert
                Assert.Equal(expectedValue, Int64Encoder.FromBase58String(value));
            }
        }

        public static class ToBase58String
        {
            public static IEnumerable<object[]> SampleData
            {
                get
                {
                    yield return new object[] { String.Empty, 0 };
                    yield return new object[] { "NQm6nKp8qFC", Int64.MaxValue };
                }
            }

            [Fact(DisplayName = "Int64Encoder.ToBase58String() a negative long throws")]
            public static void Encode_NegativeValue_Throws()
            {
                // Act & Assert
                Assert.Throws<ArgumentOutOfRangeException>(() => Int64Encoder.ToBase58String(-1));
            }

            [Theory(DisplayName = "Int64Encoder.ToBase58String() on sample data")]
            [PropertyData("SampleData")]
            public static void Encode_SampleData_Succeeds(string expectedValue, long value)
            {
                // Act & Assert
                Assert.Equal(expectedValue, Int64Encoder.ToBase58String(value));
            }
        }
    }
}
