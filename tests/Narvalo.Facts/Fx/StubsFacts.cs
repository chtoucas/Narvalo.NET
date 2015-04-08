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
            Assert.True(Stubs.Noop != null);
        }

        #endregion

        #region AlwaysDefault

        [Fact]
        public static void AlwaysDefault_IsNotNull()
        {
            // Act
            Assert.True(Stubs<string>.AlwaysDefault != null);
            Assert.True(Stubs<int>.AlwaysDefault != null);
            Assert.True(Stubs<long>.AlwaysDefault != null);
            Assert.True(Stubs<object>.AlwaysDefault != null);

            Assert.True(Stubs<string, int>.AlwaysDefault != null);
            Assert.True(Stubs<int, int>.AlwaysDefault != null);
            Assert.True(Stubs<long, int>.AlwaysDefault != null);
            Assert.True(Stubs<object, int>.AlwaysDefault != null);
            Assert.True(Stubs<string, string>.AlwaysDefault != null);
            Assert.True(Stubs<int, long>.AlwaysDefault != null);
            Assert.True(Stubs<long, object>.AlwaysDefault != null);
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
            Assert.True(Stubs<string>.AlwaysFalse != null);
            Assert.True(Stubs<int>.AlwaysFalse != null);
            Assert.True(Stubs<long>.AlwaysFalse != null);
            Assert.True(Stubs<object>.AlwaysFalse != null);
        }

        [Theory]
        [MemberData("StringTestData")]
        [CLSCompliant(false)]
        public static void AlwaysFalse_ReturnsFalse_StringTestSuite(string input)
        {
            // Act
            Assert.False(Stubs<string>.AlwaysFalse(input));
        }

        [Theory]
        [MemberData("Int32TestData")]
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
            Assert.True(Stubs<string>.AlwaysTrue != null);
            Assert.True(Stubs<int>.AlwaysTrue != null);
            Assert.True(Stubs<long>.AlwaysTrue != null);
            Assert.True(Stubs<object>.AlwaysTrue != null);
        }

        [Theory]
        [MemberData("StringTestData")]
        [CLSCompliant(false)]
        public static void AlwaysTrue_ReturnsTrue_StringTestSuite(string input)
        {
            // Act
            Assert.True(Stubs<string>.AlwaysTrue(input));
        }

        [Theory]
        [MemberData("Int32TestData")]
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
            Assert.True(Stubs<string>.Identity != null);
            Assert.True(Stubs<int>.Identity != null);
            Assert.True(Stubs<long>.Identity != null);
            Assert.True(Stubs<object>.Identity != null);
        }

        [Theory]
        [MemberData("StringTestData")]
        [CLSCompliant(false)]
        public static void Identity_ReturnsBackInput_StringTestSuite(string input)
        {
            // Act
            Assert.Equal(input, Stubs<string>.Identity(input));
        }

        [Theory]
        [MemberData("Int32TestData")]
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
            Assert.True(Stubs<string>.Ignore != null);
            Assert.True(Stubs<int>.Ignore != null);
            Assert.True(Stubs<long>.Ignore != null);
            Assert.True(Stubs<object>.Ignore != null);
        }

        #endregion
    }
}
