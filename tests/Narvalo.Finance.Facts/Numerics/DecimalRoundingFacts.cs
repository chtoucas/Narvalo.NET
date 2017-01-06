// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class DecimalRoundingFacts
    {
        #region Round()

        [Fact]
        public static void Round_Down()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.Down);

            Assert.Equal(1m, round(1.6m));
            Assert.Equal(1m, round(1.5m));
            Assert.Equal(1m, round(1.4m));
            Assert.Equal(1m, round(1m));
            Assert.Equal(0m, round(0.6m));
            Assert.Equal(0m, round(0.5m));
            Assert.Equal(0m, round(0.4m));
            Assert.Equal(0m, round(0m));
            Assert.Equal(-1m, round(-0.4m));
            Assert.Equal(-1m, round(-0.5m));
            Assert.Equal(-1m, round(-0.6m));
            Assert.Equal(-1m, round(-1m));
            Assert.Equal(-2m, round(-1.4m));
            Assert.Equal(-2m, round(-1.5m));
            Assert.Equal(-2m, round(-1.6m));

            Assert.Equal(Decimal.MaxValue, round(Decimal.MaxValue));
            Assert.Equal(Decimal.MinValue, round(Decimal.MinValue));
        }

        [Fact]
        public static void Round_Up()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.Up);

            Assert.Equal(2m, round(1.6m));
            Assert.Equal(2m, round(1.5m));
            Assert.Equal(2m, round(1.4m));
            Assert.Equal(1m, round(1m));
            Assert.Equal(1m, round(0.6m));
            Assert.Equal(1m, round(0.5m));
            Assert.Equal(1m, round(0.4m));
            Assert.Equal(0m, round(0m));
            Assert.Equal(0m, round(-0.4m));
            Assert.Equal(0m, round(-0.5m));
            Assert.Equal(0m, round(-0.6m));
            Assert.Equal(-1m, round(-1m));
            Assert.Equal(-1m, round(-1.4m));
            Assert.Equal(-1m, round(-1.5m));
            Assert.Equal(-1m, round(-1.6m));

            Assert.Equal(Decimal.MaxValue, round(Decimal.MaxValue));
            Assert.Equal(Decimal.MinValue, round(Decimal.MinValue));
        }

        [Fact]
        public static void Round_TowardsZero()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.TowardsZero);

            Assert.Equal(1m, round(1.6m));
            Assert.Equal(1m, round(1.5m));
            Assert.Equal(1m, round(1.4m));
            Assert.Equal(1m, round(1m));
            Assert.Equal(0m, round(0.6m));
            Assert.Equal(0m, round(0.5m));
            Assert.Equal(0m, round(0.4m));
            Assert.Equal(0m, round(0m));
            Assert.Equal(0m, round(-0.4m));
            Assert.Equal(0m, round(-0.5m));
            Assert.Equal(0m, round(-0.6m));
            Assert.Equal(-1m, round(-1m));
            Assert.Equal(-1m, round(-1.4m));
            Assert.Equal(-1m, round(-1.5m));
            Assert.Equal(-1m, round(-1.6m));

            Assert.Equal(Decimal.MaxValue, round(Decimal.MaxValue));
            Assert.Equal(Decimal.MinValue, round(Decimal.MinValue));
        }

        [Fact]
        public static void Round_AwayFromZero()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.AwayFromZero);

            Assert.Equal(2m, round(1.6m));
            Assert.Equal(2m, round(1.5m));
            Assert.Equal(2m, round(1.4m));
            Assert.Equal(1m, round(1m));
            Assert.Equal(1m, round(0.6m));
            Assert.Equal(1m, round(0.5m));
            Assert.Equal(1m, round(0.4m));
            Assert.Equal(0m, round(0m));
            Assert.Equal(-1m, round(-0.4m));
            Assert.Equal(-1m, round(-0.5m));
            Assert.Equal(-1m, round(-0.6m));
            Assert.Equal(-1m, round(-1m));
            Assert.Equal(-2m, round(-1.4m));
            Assert.Equal(-2m, round(-1.5m));
            Assert.Equal(-2m, round(-1.6m));

            Assert.Equal(Decimal.MaxValue, round(Decimal.MaxValue));
            Assert.Equal(Decimal.MinValue, round(Decimal.MinValue));
        }

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_HalfDown(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalRounding.Round(value, NumberRounding.HalfDown));

        [Fact]
        public static void Round_HalfDown_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.HalfDown);

            Assert.Equal(1m, round(1.5m));
            Assert.Equal(0m, round(0.5m));
            Assert.Equal(-1m, round(-0.5m));
            Assert.Equal(-2m, round(-1.5m));
            Assert.Equal(Decimal.MinValue, round(Decimal.MinValue));
        }

        [Fact]
        public static void Round_HalfDown_ForMinValue()
            => Assert.Equal(Decimal.MinValue, DecimalRounding.Round(Decimal.MinValue, NumberRounding.HalfDown));

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_HalfUp(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalRounding.Round(value, NumberRounding.HalfUp));

        [Fact]
        public static void Round_HalfUp_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.HalfUp);

            Assert.Equal(2m, round(1.5m));
            Assert.Equal(1m, round(0.5m));
            Assert.Equal(0m, round(-0.5m));
            Assert.Equal(-1m, round(-1.5m));
        }

        [Fact]
        public static void Round_HalfUp_ForMaxValue()
            => Assert.Equal(Decimal.MaxValue, DecimalRounding.Round(Decimal.MaxValue, NumberRounding.HalfUp));

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_HalfTowardsZero(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalRounding.Round(value, NumberRounding.HalfTowardsZero));

        [Fact]
        public static void Round_HalfTowardsZero_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.HalfTowardsZero);

            Assert.Equal(1m, round(1.5m));
            Assert.Equal(0m, round(0.5m));
            Assert.Equal(0m, round(-0.5m));
            Assert.Equal(-1m, round(-1.5m));
        }

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_HalfAwayFromZero(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalRounding.Round(value, NumberRounding.HalfAwayFromZero));

        [Fact]
        public static void Round_HalfAwayFromZero_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.HalfAwayFromZero);

            Assert.Equal(2m, round(1.5m));
            Assert.Equal(1m, round(0.5m));
            Assert.Equal(-1m, round(-0.5m));
            Assert.Equal(-2m, round(-1.5m));
        }

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_ToEven(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalRounding.Round(value, NumberRounding.ToEven));

        [Fact]
        public static void Round_ToEven_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.ToEven);

            Assert.Equal(2m, round(1.5m));
            Assert.Equal(0m, round(0.5m));
            Assert.Equal(0m, round(-0.5m));
            Assert.Equal(-2m, round(-1.5m));
        }

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_ToOdd(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalRounding.Round(value, NumberRounding.ToOdd));

        [Fact]
        public static void Round_ToOdd_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalRounding.Round(_, NumberRounding.ToOdd);

            Assert.Equal(1m, round(1.5m));
            Assert.Equal(1m, round(0.5m));
            Assert.Equal(-1m, round(-0.5m));
            Assert.Equal(-1m, round(-1.5m));
        }

        [Theory(Skip = "Temporary test.")]
        [MemberData(nameof(Test1Data), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Test1(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, Math.Ceiling(value));

        #endregion
    }

    public static partial class DecimalRoundingFacts
    {
        public static IEnumerable<object[]> Test1Data
        {
            get
            {
                // When the input can not be represented by a decimal, C# rounds it using the
                // default rounding mode (MidpointRounding.ToEven).
                // Here the input is rounded to 79228162514264337593543950335m.
                yield return new object[] { 79228162514264337593543950334.9m, 79228162514264337593543950335m };
                yield return new object[] { 79228162514264337593543950334.8m, 79228162514264337593543950335m };
                yield return new object[] { 79228162514264337593543950334.7m, 79228162514264337593543950335m };
                yield return new object[] { 79228162514264337593543950334.6m, 79228162514264337593543950335m };
                // Here the input is rounded to 79228162514264337593543950334m.
                yield return new object[] { 79228162514264337593543950334.5m, 79228162514264337593543950334m };
                yield return new object[] { 79228162514264337593543950334.4m, 79228162514264337593543950334m };
                yield return new object[] { 79228162514264337593543950334.3m, 79228162514264337593543950334m };
                yield return new object[] { 79228162514264337593543950334.2m, 79228162514264337593543950334m };
                yield return new object[] { 79228162514264337593543950334.1m, 79228162514264337593543950334m };
                yield return new object[] { 79228162514264337593543950334.0m, 79228162514264337593543950334m };

                yield return new object[] { 6999999999999999999999999999.9m, 7000000000000000000000000000m };
                yield return new object[] { 6999999999999999999999999999.8m, 7000000000000000000000000000m };
                yield return new object[] { 6999999999999999999999999999.7m, 7000000000000000000000000000m };
                yield return new object[] { 6999999999999999999999999999.6m, 7000000000000000000000000000m };
                yield return new object[] { 6999999999999999999999999999.5m, 7000000000000000000000000000m };
                yield return new object[] { 6999999999999999999999999999.4m, 7000000000000000000000000000m };
                yield return new object[] { 6999999999999999999999999999.3m, 7000000000000000000000000000m };
                yield return new object[] { 6999999999999999999999999999.2m, 7000000000000000000000000000m };
                yield return new object[] { 6999999999999999999999999999.1m, 7000000000000000000000000000m };
                //yield return new object[] { 8999999999999999999999999999.0m, 8999999999999999999999999999m };
            }
        }

        public static IEnumerable<object[]> NearestRounding
        {
            get
            {
                yield return new object[] { 3m, 3m };
                yield return new object[] { 2m, 2m };
                yield return new object[] { 1.6m, 2m };
                yield return new object[] { 1.4m, 1m };
                yield return new object[] { 1m, 1m };
                yield return new object[] { 0.6m, 1m };
                yield return new object[] { 0.4m, 0m };
                yield return new object[] { 0m, 0m };
                yield return new object[] { -0.4m, 0m };
                yield return new object[] { -0.6m, -1m };
                yield return new object[] { -1m, -1m };
                yield return new object[] { -1.4m, -1m };
                yield return new object[] { -1.6m, -2m };
                yield return new object[] { -2m, -2m };
                yield return new object[] { -3m, -3m };
            }
        }
    }
}
