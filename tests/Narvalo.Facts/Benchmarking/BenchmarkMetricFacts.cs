// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using NodaTime;
    using Xunit;

    public static class BenchmarkMetricFacts
    {
        [Fact]
        public static void ToString_ForNullFormat()
        {
            // Arrange
            var metric = new BenchmarkMetric("Category", "Name", Duration.Epsilon, 1);

            // Act & Assert
            Assert.Equal(metric.ToString(), metric.ToString(null, null));
        }

        [Fact]
        public static void ToString_ForDefaultFormat()
        {
            // Arrange
            var metric = new BenchmarkMetric("Category", "Name", Duration.Epsilon, 1);

            // Act & Assert
            Assert.Equal(metric.ToString(), metric.ToString("G", null));
        }
    }
}
