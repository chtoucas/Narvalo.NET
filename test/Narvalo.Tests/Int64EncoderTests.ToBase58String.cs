namespace Narvalo {
	using System;
	using System.Collections.Generic;
	using Xunit;
	using Xunit.Extensions;
	
	public partial class Int64ConvertTests {
        [Fact(DisplayName = "Int64Encoder.ToBase58String() a negative long throws")]
        public void Encode_NegativeValue_Throws() {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(delegate { Int64Encoder.ToBase58String(-1); });
        }

        [Theory(DisplayName = "Int64Encoder.ToBase58String() on sample data")]
        [PropertyData("SampleData")]
        public void Encode_SampleData_Succeeds(string expectedValue, long value) {
            // Act & Assert
            Assert.Equal(expectedValue, Int64Encoder.ToBase58String(value));
        }
	}
}
