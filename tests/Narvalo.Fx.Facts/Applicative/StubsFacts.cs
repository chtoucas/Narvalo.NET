// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections.Generic;

    using Xunit;

    public static partial class StubsFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Stubs), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(Stubs), description) { }
        }

        [t("Noop is not null.")]
        public static void Noop1() => Assert.NotNull(Stubs.Noop);

        [t("AlwaysDefault is not null.")]
        public static void AlwaysDefault1() {
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

        [t("AlwaysDefault is constant default.")]
        public static void AlwaysDefault2() {
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

        [t("AlwaysFalse is not null.")]
        public static void AlwaysFalse1() {
            Assert.NotNull(Stubs<string>.AlwaysFalse);
            Assert.NotNull(Stubs<int>.AlwaysFalse);
            Assert.NotNull(Stubs<long>.AlwaysFalse);
            Assert.NotNull(Stubs<object>.AlwaysFalse);
        }

        [T("AlwaysFalse is constant false.")]
        [MemberData(nameof(StringTestData), DisableDiscoveryEnumeration = true)]
        public static void AlwaysFalse2(string input)
            => Assert.False(Stubs<string>.AlwaysFalse(input));

        [T("AlwaysFalse is constant false.")]
        [MemberData(nameof(Int32TestData), DisableDiscoveryEnumeration = true)]
        public static void AlwaysFalse3(int input)
            => Assert.False(Stubs<int>.AlwaysFalse(input));

        [t("AlwaysTrue is not null.")]
        public static void AlwaysTrue1() {
            Assert.NotNull(Stubs<string>.AlwaysTrue);
            Assert.NotNull(Stubs<int>.AlwaysTrue);
            Assert.NotNull(Stubs<long>.AlwaysTrue);
            Assert.NotNull(Stubs<object>.AlwaysTrue);
        }

        [T("AlwaysTrue is constant true.")]
        [MemberData(nameof(StringTestData), DisableDiscoveryEnumeration = true)]
        public static void AlwaysTrue2(string input)
            => Assert.True(Stubs<string>.AlwaysTrue(input));

        [T("AlwaysTrue is constant true.")]
        [MemberData(nameof(Int32TestData), DisableDiscoveryEnumeration = true)]
        public static void AlwaysTrue3(int input)
            => Assert.True(Stubs<int>.AlwaysTrue(input));

        [t("Ident is not null.")]
        public static void Ident1() {
            Assert.NotNull(Stubs<string>.Ident);
            Assert.NotNull(Stubs<int>.Ident);
            Assert.NotNull(Stubs<long>.Ident);
            Assert.NotNull(Stubs<object>.Ident);
        }

        [T("Ident is identity.")]
        [MemberData(nameof(StringTestData), DisableDiscoveryEnumeration = true)]
        public static void Ident2(string input)
            => Assert.Equal(input, Stubs<string>.Ident(input));

        [T("Ident is identity.")]
        [MemberData(nameof(Int32TestData), DisableDiscoveryEnumeration = true)]
        public static void Ident3(int input)
            => Assert.Equal(input, Stubs<int>.Ident(input));

        [t("Ignore is not null.")]
        public static void Ignore1() {
            Assert.NotNull(Stubs<string>.Ignore);
            Assert.NotNull(Stubs<int>.Ignore);
            Assert.NotNull(Stubs<long>.Ignore);
            Assert.NotNull(Stubs<object>.Ignore);
        }
    }

    public static partial class StubsFacts {
        public static IEnumerable<object[]> Int32TestData {
            get {
                yield return new object[] { Int32.MaxValue };
                yield return new object[] { 1 };
                yield return new object[] { 0 };
                yield return new object[] { -1 };
                yield return new object[] { Int32.MinValue };
            }
        }

        public static IEnumerable<object[]> StringTestData {
            get {
                yield return new object[] { null };
                yield return new object[] { String.Empty };
                yield return new object[] { "value" };
            }
        }
    }
}
