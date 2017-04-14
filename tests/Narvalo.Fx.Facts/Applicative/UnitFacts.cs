// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    public static class UnitFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Unit), description) { }
        }

        [t("Default is default(Unit)")]
        public static void Default1() {
            var u = Unit.Default;
            var u1 = default(Unit);

            Assert.True(u == u1);
            Assert.True(u.Equals(u1));
        }

        [t("== and !=.")]
        public static void Equality1() {
            var u = Unit.Default;
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.True(u == u1);
            Assert.True(u1 == u);
            Assert.True(u1 == u2);

            Assert.False(u != u1);
            Assert.False(u1 != u);
            Assert.False(u1 != u2);
        }

        [t("== and != w/ ().")]
        public static void Equality2() {
            var u = Unit.Default;
            var u1 = new Unit();
            var t = new ValueTuple();

            Assert.True(u == t);
            Assert.True(t == u);
            Assert.True(u1 == t);
            Assert.True(t == u1);

            Assert.False(u != t);
            Assert.False(t != u);
            Assert.False(u1 != t);
            Assert.False(t != u1);
        }

        [t("Equals(unit) is always true.")]
        public static void Equals1() {
            var u = Unit.Default;
            var u1 = new Unit();
            var u2 = new Unit();

            Assert.True(u.Equals(u));
            Assert.True(u.Equals(u1));
            Assert.True(u.Equals(u2));
            Assert.True(u1.Equals(u));
            Assert.True(u1.Equals(u1));
            Assert.True(u1.Equals(u2));
        }

        [t("Equals(ValueTuple) is always true.")]
        public static void Equals2() {
            var u = Unit.Default;
            var u1 = new Unit();
            var t1 = new ValueTuple();
            // Boxed version.
            object t2 = new ValueTuple();

            Assert.True(u.Equals(t1));
            Assert.True(u.Equals(t2));
            Assert.True(u1.Equals(t1));
            Assert.True(u1.Equals(t2));
        }

        [t("Equals(non-unit or ()) is always false.")]
        public static void Equals3() {
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

            Assert.Equal(0, u.GetHashCode());
            Assert.Equal(0, u1.GetHashCode());
        }

        [t("ToString() always returns ().")]
        public static void ToString1() {
            var u = Unit.Default;
            var u1 = new Unit();

            Assert.Equal("()", u.ToString());
            Assert.Equal("()", u1.ToString());
        }
    }
}
