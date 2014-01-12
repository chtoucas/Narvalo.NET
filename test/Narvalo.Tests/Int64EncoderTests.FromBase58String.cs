namespace Narvalo {
	using System;
	using System.Collections.Generic;
	using Xunit;
	using Xunit.Extensions;
	
	public partial class Int64ConvertTests {
        [Fact(DisplayName = "Int64Encoder.FromBase58String() null throws.")]
        public void Decode_Null_Throws() {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate { Int64Encoder.FromBase58String(null); });
        }

        [Theory(DisplayName = "Int64Encoder.FromBase58String() on sample data.")]
        [PropertyData("SampleData")]
        public void Decode_SampleData_Succeeds(string value, long expectedValue) {
            // Act & Assert
            Assert.Equal(expectedValue, Int64Encoder.FromBase58String(value));
        }
	}
}
