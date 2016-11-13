// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static class StubsFacts
    {
        public static IEnumerable<object[]> Int32TestData
        {
            get
            {
                yield return new object[] { Int32.MaxValue };
                yield return new object[] { 1 };
                yield return new object[] { 0 };
                yield return new object[] { -1 };
                yield return new object[] { Int32.MinValue };
            }
        }

        public static IEnumerable<object[]> StringTestData
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { String.Empty };
                yield return new object[] { "value" };
            }
        }

        #region Noop

        [Fact]
        public static void Noop_IsNotNull()
        {
            // Act
            Assert.NotNull(Stubs.Noop);
        }

        #endregion

        #region AlwaysDefault

        [Fact]
        public static void AlwaysDefault_IsNotNull()
        {
            // Act
            Assert.NotNull(Stubs<string>.AlwaysDefault);
            Assert.NotNull(Stubs<int>.AlwaysDefault);
            Assert.NotNull(Stubs<long>.AlwaysDefault);
            Assert.NotNull(Stubs<object>.AlwaysDefault);

            Assert.NotNull(Stubs<string, int>.AlwaysDefault);
            Assert.NotNull(Stubs<int, int>.AlwaysDefault);
            Assert.NotNull(Stubs<long, int>.AlwaysDefault);
            Assert.NotNull(Stubs<object, int>.AlwaysDefault);
            Assert.NotNull(Stubs<string, string>.AlwaysDefault);
            Assert.NotNull(Stubs<int, long>.AlwaysDefault);
            Assert.NotNull(Stubs<long, object>.AlwaysDefault);
        }

        [Fact]
        public static void AlwaysDefault_ReturnsDefault()
        {
            // Act
            Assert.Equal(default(string), Stubs<string>.AlwaysDefault());
            Assert.Equal(default(int), Stubs<int>.AlwaysDefault());
            Assert.Equal(default(long), Stubs<long>.AlwaysDefault());
            Assert.Equal(default(object), Stubs<object>.AlwaysDefault());

            Assert.Equal(default(int), Stubs<string, int>.AlwaysDefault(String.Empty));
            Assert.Equal(default(int), Stubs<int, int>.AlwaysDefault(1));
            Assert.Equal(default(int), Stubs<long, int>.AlwaysDefault(1L));
            Assert.Equal(default(int), Stubs<object, int>.AlwaysDefault(new Object()));
            Assert.Equal(default(string), Stubs<string, string>.AlwaysDefault(String.Empty));
            Assert.Equal(default(long), Stubs<int, long>.AlwaysDefault(1));
            Assert.Equal(default(object), Stubs<long, object>.AlwaysDefault(1L));
        }

        #endregion

        #region AlwaysFalse

        [Fact]
        public static void AlwaysFalse_IsNotNull()
        {
            // Act
            Assert.NotNull(Stubs<string>.AlwaysFalse);
            Assert.NotNull(Stubs<int>.AlwaysFalse);
            Assert.NotNull(Stubs<long>.AlwaysFalse);
            Assert.NotNull(Stubs<object>.AlwaysFalse);
        }

        [Theory]
        [MemberData(nameof(StringTestData), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void AlwaysFalse_ReturnsFalse_StringTestSuite(string input)
        {
            // Act
            Assert.False(Stubs<string>.AlwaysFalse(input));
        }

        [Theory]
        [MemberData(nameof(Int32TestData), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void AlwaysFalse_ReturnsFalse_Int32TestSuite(int input)
        {
            // Act
            Assert.False(Stubs<int>.AlwaysFalse(input));
        }

        #endregion

        #region AlwaysTrue

        [Fact]
        public static void AlwaysTrue_IsNotNull()
        {
            // Act
            Assert.NotNull(Stubs<string>.AlwaysTrue);
            Assert.NotNull(Stubs<int>.AlwaysTrue);
            Assert.NotNull(Stubs<long>.AlwaysTrue);
            Assert.NotNull(Stubs<object>.AlwaysTrue);
        }

        [Theory]
        [MemberData(nameof(StringTestData), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void AlwaysTrue_ReturnsTrue_StringTestSuite(string input)
        {
            // Act
            Assert.True(Stubs<string>.AlwaysTrue(input));
        }

        [Theory]
        [MemberData(nameof(Int32TestData), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void AlwaysTrue_ReturnsTrue_Int32TestSuite(int input)
        {
            // Act
            Assert.True(Stubs<int>.AlwaysTrue(input));
        }

        #endregion

        #region Identity

        [Fact]
        public static void Identity_IsNotNull()
        {
            // Act
            Assert.NotNull(Stubs<string>.Identity);
            Assert.NotNull(Stubs<int>.Identity);
            Assert.NotNull(Stubs<long>.Identity);
            Assert.NotNull(Stubs<object>.Identity);
        }

        [Theory]
        [MemberData(nameof(StringTestData), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Identity_ReturnsBackInput_StringTestSuite(string input)
        {
            // Act
            Assert.Equal(input, Stubs<string>.Identity(input));
        }

        [Theory]
        [MemberData(nameof(Int32TestData), DisableDiscoveryEnumeration = true)]
        [CLSCompliant(false)]
        public static void Identity_ReturnsBackInput_Int32TestSuite(int input)
        {
            // Act
            Assert.Equal(input, Stubs<int>.Identity(input));
        }

        #endregion

        #region Ignore

        [Fact]
        public static void Ignore_IsNotNull()
        {
            // Act
            Assert.NotNull(Stubs<string>.Ignore);
            Assert.NotNull(Stubs<int>.Ignore);
            Assert.NotNull(Stubs<long>.Ignore);
            Assert.NotNull(Stubs<object>.Ignore);
        }

        #endregion
    }
}
