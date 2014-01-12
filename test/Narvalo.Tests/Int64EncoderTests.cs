namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Extensions;

    public partial class Int64ConvertTests
    {
        public static IEnumerable<object[]> SampleData
        {
            get
            {
                yield return new object[] { String.Empty, 0 };
                yield return new object[] { "NQm6nKp8qFC", Int64.MaxValue };
            }
        }

        [Fact(DisplayName = "Int64Encoder.ToBase58String() then FromBase58String() is invariant")]
        public void RoundTrip_Succeeds()
        {
            // Arrange
            long value = 3471391110;
            // Act & Assert
            Assert.Equal(value, Int64Encoder.FromBase58String(Int64Encoder.ToBase58String(value)));
        }
    }
}
