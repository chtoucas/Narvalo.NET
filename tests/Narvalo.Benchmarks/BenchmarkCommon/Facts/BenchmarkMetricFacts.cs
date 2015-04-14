// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.BenchmarkCommon.Facts
{
    using System;

    using Xunit;

    public static class BenchmarkMetricFacts
    {
        private static readonly TimeSpan s_Epsilon = TimeSpan.FromTicks(1L);

        #region ToString()

        [Fact]
        public static void ToString_IsSameAsNoFormat_ForNullFormat()
        {
            // Arrange
            var metric = new BenchmarkMetric("Category", "Name", s_Epsilon, 1);

            // Act & Assert
            Assert.Equal(metric.ToString(), metric.ToString(null, null));
        }

        [Fact]
        public static void ToString_IsSameAsNoFormat_ForDefaultFormat()
        {
            // Arrange
            var metric = new BenchmarkMetric("Category", "Name", s_Epsilon, 1);

            // Act & Assert
            Assert.Equal(metric.ToString(), metric.ToString("G", null));
        }

        #endregion
    }
}
