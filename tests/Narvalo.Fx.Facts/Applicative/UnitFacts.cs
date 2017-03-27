// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    public static class UnitFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Unit), description) { }
        }

        [t("== is always true.")]
        public static void Equality1() {
            var u = Unit.Default;
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.True(u1 == u);
            Assert.True(u == u1);
            Assert.True(u1 == u2);
        }

        [t("!= is always false.")]
        public static void Inequality1() {
            var u = Unit.Default;
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.False(u1 != u);
            Assert.False(u != u1);
            Assert.False(u1 != u2);
        }

        [t("Equals(unit) is always true.")]
        public static void Equals1() {
            var u = Unit.Default;
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.True(u1.Equals(u1));
            Assert.True(u1.Equals(u2));
            Assert.True(u1.Equals(u));
            Assert.True(u.Equals(u1));
            Assert.True(u.Equals(u2));
            Assert.True(u.Equals(u));
        }

        [t("Equals(non-unit) is always false.")]
        public static void Equals2() {
            var u = Unit.Default;
            var u1 = new Unit();

            var obj = new Object();

            Assert.False(u1.Equals(null));
            Assert.False(u1.Equals(obj));
            Assert.False(u.Equals(null));
            Assert.False(u.Equals(obj));
            Assert.False(obj.Equals(u1));
            Assert.False(obj.Equals(u));
        }

        [t("GetHashCode() always returns zero.")]
        public static void GetHashCode1() {
            var u = Unit.Default;
            var u1 = new Unit();

            Assert.Equal(0, u1.GetHashCode());
            Assert.Equal(0, u.GetHashCode());
        }

        [t("ToString() always returns ().")]
        public static void ToString1() {
            var u = Unit.Default;
            var u1 = new Unit();

            Assert.Equal("()", u1.ToString());
            Assert.Equal("()", u.ToString());
        }
    }
}
