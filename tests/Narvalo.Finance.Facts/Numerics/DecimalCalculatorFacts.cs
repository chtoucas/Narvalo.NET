// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class DecimalCalculatorFacts
    {
        #region Round()

        [Fact]
        public static void Round_ToOdd_ThrowsNotImplementedException()
            => Assert.Throws<NotImplementedException>(() => DecimalCalculator.Round(1m, NumberRounding.ToOdd));

        [Fact]
        public static void Round_Stochastic_ThrowsNotImplementedException()
            => Assert.Throws<NotImplementedException>(() => DecimalCalculator.Round(1m, NumberRounding.Stochastic));

        [Fact]
        public static void Round_Down()
        {
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.Down);

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
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.Up);

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
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.TowardsZero);

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
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.AwayFromZero);

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
            => Assert.Equal(expectedValue, DecimalCalculator.Round(value, NumberRounding.HalfDown));

        [Fact]
        public static void Round_HalfDown_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.HalfDown);

            Assert.Equal(1m, round(1.5m));
            Assert.Equal(0m, round(0.5m));
            Assert.Equal(-1m, round(-0.5m));
            Assert.Equal(-2m, round(-1.5m));
        }

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_HalfUp(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalCalculator.Round(value, NumberRounding.HalfUp));

        [Fact]
        public static void Round_HalfUp_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.HalfUp);

            Assert.Equal(2m, round(1.5m));
            Assert.Equal(1m, round(0.5m));
            Assert.Equal(0m, round(-0.5m));
            Assert.Equal(-1m, round(-1.5m));
        }

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_HalfTowardsZero(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalCalculator.Round(value, NumberRounding.HalfTowardsZero));

        [Fact]
        public static void Round_HalfTowardsZero_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.HalfTowardsZero);

            Assert.Equal(1m, round(1.5m));
            Assert.Equal(0m, round(0.5m));
            Assert.Equal(0m, round(-0.5m));
            Assert.Equal(-1m, round(-1.5m));
        }

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_HalfAwayFromZero(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalCalculator.Round(value, NumberRounding.HalfAwayFromZero));

        [Fact]
        public static void Round_HalfAwayFromZero_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.HalfAwayFromZero);

            Assert.Equal(2m, round(1.5m));
            Assert.Equal(1m, round(0.5m));
            Assert.Equal(-1m, round(-0.5m));
            Assert.Equal(-2m, round(-1.5m));
        }

        [Theory]
        [MemberData(nameof(NearestRounding), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Round_ToEven(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, DecimalCalculator.Round(value, NumberRounding.ToEven));

        [Fact]
        public static void Round_ToEven_ForHalfWayPoints()
        {
            Func<decimal, decimal> round = _ => DecimalCalculator.Round(_, NumberRounding.ToEven);

            Assert.Equal(2m, round(1.5m));
            Assert.Equal(0m, round(0.5m));
            Assert.Equal(0m, round(-0.5m));
            Assert.Equal(-2m, round(-1.5m));
        }

        [Theory]
        [MemberData(nameof(Test1Data), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Test1(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, Math.Ceiling(value));

        [Theory]
        [MemberData(nameof(Test2Data), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Test2(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, value - 0.5m);

        [Theory]
        [MemberData(nameof(Test3Data), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Test3(decimal value, decimal expectedValue)
            => Assert.Equal(expectedValue, Math.Truncate(value));

        #endregion
    }

    public static partial class DecimalCalculatorFacts
    {
        public static IEnumerable<object[]> Test1Data
        {
            get
            {
                //yield return new object[] { 79228162514264337593543950334.5M, 79228162514264337593543950335M };
                yield return new object[] { 79228162514264337593543950333.5M, 79228162514264337593543950334M };
                //yield return new object[] { 79228162514264337593543950332.5M, 79228162514264337593543950333M };
                yield return new object[] { 79228162514264337593543950331.5M, 79228162514264337593543950332M };
                //yield return new object[] { 79228162514264337593543950330.5M, 79228162514264337593543950331M };
                yield return new object[] { 79228162514264337593543950329.5M, 79228162514264337593543950330M };
                //yield return new object[] { 79228162514264337593543950328.5M, 79228162514264337593543950329M };
                yield return new object[] { 79228162514264337593543950327.5M, 79228162514264337593543950328M };
                //yield return new object[] { 79228162514264337593543950326.5M, 79228162514264337593543950327M };
                yield return new object[] { 79228162514264337593543950325.5M, 79228162514264337593543950326M };
                //yield return new object[] { 79228162514264337593543950324.5M, 79228162514264337593543950325M };
                yield return new object[] { 79228162514264337593543950323.5M, 79228162514264337593543950324M };

                // Even integer part: the result looks like it's rounding nearest to even.
                yield return new object[] { 79228162514264337593543950324.9M, 79228162514264337593543950325M };
                yield return new object[] { 79228162514264337593543950324.8M, 79228162514264337593543950325M };
                yield return new object[] { 79228162514264337593543950324.7M, 79228162514264337593543950325M };
                yield return new object[] { 79228162514264337593543950324.6M, 79228162514264337593543950325M };

                yield return new object[] { 79228162514264337593543950324.5M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.4M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.3M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.2M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.1M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.0M, 79228162514264337593543950324M };

                // Odd integer part: the result looks like it's rounding nearest to even.
                yield return new object[] { 79228162514264337593543950323.9M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950323.8M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950323.7M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950323.6M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950323.5M, 79228162514264337593543950324M };

                yield return new object[] { 79228162514264337593543950323.4M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.3M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.2M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.1M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.0M, 79228162514264337593543950323M };

                // OK
                yield return new object[] { 5999999999223372036854775807.9M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.8M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.7M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.6M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.5M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.4M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.3M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.2M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.1M, 5999999999223372036854775808M };
                yield return new object[] { 5999999999223372036854775807.0M, 5999999999223372036854775807M };

                // OK. Equal or above Int64.MaxValue.
                yield return new object[] { 9223372036854775807.9M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.8M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.7M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.6M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.5M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.4M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.3M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.2M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.1M, 9223372036854775808M };
                yield return new object[] { 9223372036854775807.0M, 9223372036854775807M };

                // OK. Below Int64.MaxValue.
                yield return new object[] { 9223372036854775806.9M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.8M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.7M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.6M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.5M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.4M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.3M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.2M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.1M, 9223372036854775807M };
                yield return new object[] { 9223372036854775806.0M, 9223372036854775806M };

                // OK
                yield return new object[] { 2.9M, 3M };
                yield return new object[] { 2.8M, 3M };
                yield return new object[] { 2.7M, 3M };
                yield return new object[] { 2.6M, 3M };
                yield return new object[] { 2.5M, 3M };
                yield return new object[] { 2.4M, 3M };
                yield return new object[] { 2.3M, 3M };
                yield return new object[] { 2.2M, 3M };
                yield return new object[] { 2.1M, 3M };
                yield return new object[] { 2.0M, 2M };

                // OK
                yield return new object[] { 1.9M, 2M };
                yield return new object[] { 1.8M, 2M };
                yield return new object[] { 1.7M, 2M };
                yield return new object[] { 1.6M, 2M };
                yield return new object[] { 1.5M, 2M };
                yield return new object[] { 1.4M, 2M };
                yield return new object[] { 1.3M, 2M };
                yield return new object[] { 1.2M, 2M };
                yield return new object[] { 1.1M, 2M };
                yield return new object[] { 1.0M, 1M };
            }
        }

        public static IEnumerable<object[]> Test2Data
        {
            get
            {
                yield return new object[] { 79228162514264337593543950335M, 79228162514264337593543950334.5M };
                yield return new object[] { 79228162514264337593543950334M, 79228162514264337593543950333.5M };
                yield return new object[] { 79228162514264337593543950333M, 79228162514264337593543950332.5M };
                yield return new object[] { 79228162514264337593543950332M, 79228162514264337593543950331.5M };
                yield return new object[] { 79228162514264337593543950331M, 79228162514264337593543950330.5M };
                yield return new object[] { 79228162514264337593543950330M, 79228162514264337593543950329.5M };
                yield return new object[] { 79228162514264337593543950329M, 79228162514264337593543950328.5M };
                yield return new object[] { 79228162514264337593543950328M, 79228162514264337593543950327.5M };
                yield return new object[] { 79228162514264337593543950327M, 79228162514264337593543950326.5M };
                yield return new object[] { 79228162514264337593543950326M, 79228162514264337593543950325.5M };
                yield return new object[] { 79228162514264337593543950325M, 79228162514264337593543950324.5M };
            }
        }

        public static IEnumerable<object[]> Test3Data
        {
            get
            {
                yield return new object[] { 79228162514264337593543950334.5M, 79228162514264337593543950334M };
                yield return new object[] { 79228162514264337593543950333.5M, 79228162514264337593543950333M };
                yield return new object[] { 79228162514264337593543950332.5M, 79228162514264337593543950332M };
                yield return new object[] { 79228162514264337593543950331.5M, 79228162514264337593543950331M };
                yield return new object[] { 79228162514264337593543950330.5M, 79228162514264337593543950330M };
                yield return new object[] { 79228162514264337593543950329.5M, 79228162514264337593543950329M };
                yield return new object[] { 79228162514264337593543950328.5M, 79228162514264337593543950329M };
                yield return new object[] { 79228162514264337593543950327.5M, 79228162514264337593543950328M };
                yield return new object[] { 79228162514264337593543950326.5M, 79228162514264337593543950327M };
                yield return new object[] { 79228162514264337593543950325.5M, 79228162514264337593543950326M };
                yield return new object[] { 79228162514264337593543950324.5M, 79228162514264337593543950325M };
                yield return new object[] { 79228162514264337593543950323.5M, 79228162514264337593543950324M };

                // Even integer part: the result looks like it's rounding nearest to even.
                yield return new object[] { 79228162514264337593543950324.9M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.8M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.7M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.6M, 79228162514264337593543950324M };

                yield return new object[] { 79228162514264337593543950324.5M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.4M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.3M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.2M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.1M, 79228162514264337593543950324M };
                yield return new object[] { 79228162514264337593543950324.0M, 79228162514264337593543950324M };

                // Odd integer part: the result looks like it's rounding nearest to even.
                yield return new object[] { 79228162514264337593543950323.9M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.8M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.7M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.6M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.5M, 79228162514264337593543950323M };

                yield return new object[] { 79228162514264337593543950323.4M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.3M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.2M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.1M, 79228162514264337593543950323M };
                yield return new object[] { 79228162514264337593543950323.0M, 79228162514264337593543950323M };

                // OK
                yield return new object[] { 5999999999223372036854775807.9M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.8M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.7M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.6M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.5M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.4M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.3M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.2M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.1M, 5999999999223372036854775807M };
                yield return new object[] { 5999999999223372036854775807.0M, 5999999999223372036854775807M };

                // OK. Equal or above Int64.MaxValue.
                yield return new object[] { 9223372036854775807.9M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.8M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.7M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.6M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.5M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.4M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.3M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.2M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.1M, 9223372036854775807M };
                yield return new object[] { 9223372036854775807.0M, 9223372036854775807M };

                // OK. Below Int64.MaxValue.
                yield return new object[] { 9223372036854775806.9M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.8M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.7M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.6M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.5M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.4M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.3M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.2M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.1M, 9223372036854775806M };
                yield return new object[] { 9223372036854775806.0M, 9223372036854775806M };

                // OK
                yield return new object[] { 2.9M, 2M };
                yield return new object[] { 2.8M, 2M };
                yield return new object[] { 2.7M, 2M };
                yield return new object[] { 2.6M, 2M };
                yield return new object[] { 2.5M, 2M };
                yield return new object[] { 2.4M, 2M };
                yield return new object[] { 2.3M, 2M };
                yield return new object[] { 2.2M, 2M };
                yield return new object[] { 2.1M, 2M };
                yield return new object[] { 2.0M, 2M };

                // OK
                yield return new object[] { 1.9M, 1M };
                yield return new object[] { 1.8M, 1M };
                yield return new object[] { 1.7M, 1M };
                yield return new object[] { 1.6M, 1M };
                yield return new object[] { 1.5M, 1M };
                yield return new object[] { 1.4M, 1M };
                yield return new object[] { 1.3M, 1M };
                yield return new object[] { 1.2M, 1M };
                yield return new object[] { 1.1M, 1M };
                yield return new object[] { 1.0M, 1M };
            }
        }

        public static IEnumerable<object[]> NearestRounding
        {
            get
            {
                //yield return new object[] { Decimal.MaxValue, Decimal.MaxValue };
                yield return new object[] { Decimal.MaxValue - 1, Decimal.MaxValue - 1 };
                //yield return new object[] { Decimal.MaxValue - 2, Decimal.MaxValue - 2 };
                yield return new object[] { Decimal.MaxValue - 3, Decimal.MaxValue - 3 };
                //yield return new object[] { Decimal.MaxValue - 4, Decimal.MaxValue - 4 };
                yield return new object[] { 3M, 3M };
                yield return new object[] { 2M, 2M };
                yield return new object[] { 1.6M, 2M };
                yield return new object[] { 1.4M, 1M };
                yield return new object[] { 1M, 1M };
                yield return new object[] { 0.6M, 1M };
                yield return new object[] { 0.4M, 0M };
                yield return new object[] { 0M, 0M };
                yield return new object[] { -0.4M, 0M };
                yield return new object[] { -0.6M, -1M };
                yield return new object[] { -1M, -1M };
                yield return new object[] { -1.4M, -1M };
                yield return new object[] { -1.6M, -2M };
                yield return new object[] { -2M, -2M };
                yield return new object[] { -3M, -3M };
                //yield return new object[] { Decimal.MinValue + 4, Decimal.MinValue + 4 };
                yield return new object[] { Decimal.MinValue + 3, Decimal.MinValue + 3 };
                //yield return new object[] { Decimal.MinValue + 2, Decimal.MinValue + 2 };
                yield return new object[] { Decimal.MinValue + 1, Decimal.MinValue + 1 };
                //yield return new object[] { Decimal.MinValue, Decimal.MinValue };
            }
        }
    }
}
