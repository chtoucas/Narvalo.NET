// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    using static global::My;

    public static partial class ResultFacts {
        [t("GetHashCode() guards.")]
        public static void GetHashCode0() {
            var ok = (IStructuralEquatable)Result<Obj, string>.Of(new Obj());
            Assert.Throws<ArgumentNullException>("comparer", () => ok.GetHashCode(null));

            var nok = (IStructuralEquatable)Result<Obj, string>.FromError("error");
            Assert.Throws<ArgumentNullException>("comparer", () => nok.GetHashCode(null));
        }

        [t("GetHashCode() returns the same result when called repeatedly.")]
        public static void GetHashCode1() {
            var nok = Result<Obj, string>.FromError("error");
            Assert.Equal(nok.GetHashCode(), nok.GetHashCode());

            var ok1 = Result<Obj, string>.Of(new Obj());
            Assert.Equal(ok1.GetHashCode(), ok1.GetHashCode());

            var ok2 = Result<int, string>.Of(1);
            Assert.Equal(ok2.GetHashCode(), ok2.GetHashCode());
        }

        [t("GetHashCode(comparer) returns the same result when called repeatedly.")]
        public static void GetHashCode2() {
            var nok = Result<Obj, string>.FromError("error");
            var v = (IStructuralEquatable)nok;
            Assert.Equal(v.GetHashCode(s_Comparer), v.GetHashCode(s_Comparer));

            var ok1 = Result<Obj, string>.Of(new Obj());
            var v1 = (IStructuralEquatable)ok1;
            Assert.Equal(v1.GetHashCode(s_Comparer), v1.GetHashCode(s_Comparer));

            var ok2 = Result<int, string>.Of(1);
            var v2 = (IStructuralEquatable)ok2;
            Assert.Equal(v2.GetHashCode(s_Comparer), v2.GetHashCode(s_Comparer));
        }

        [t("GetHashCode() returns different results for non-equal instances.")]
        public static void GetHashCode3() {
            var nok = Result<Obj, string>.FromError("error");
            var ok = Result<Obj, string>.Of(new Obj());

            Assert.NotEqual(nok.GetHashCode(), ok.GetHashCode());

            var nok1 = Result<Obj, string>.FromError("error1");
            var nok2 = Result<Obj, string>.FromError("error2");

            Assert.NotEqual(nok1, nok2);
            Assert.NotEqual(nok1.GetHashCode(), nok2.GetHashCode());

            var ok1 = Result<Tuple<string>, string>.Of(Tuple.Create("1"));
            var ok2 = Result<Tuple<string>, string>.Of(Tuple.Create("2"));

            Assert.NotEqual(ok1, ok2);
            Assert.NotEqual(ok1.GetHashCode(), ok2.GetHashCode());
        }

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode4() {
            var nok1 = Result<Obj, string>.FromError("error");
            var nok2 = Result<Obj, string>.FromError("error");

            Assert.NotSame(nok1, nok2);
            Assert.Equal(nok1, nok2);
            Assert.Equal(nok1.GetHashCode(), nok2.GetHashCode());

            var ok1 = Result<Tuple<string>, string>.Of(Tuple.Create("1"));
            var ok2 = Result<Tuple<string>, string>.Of(Tuple.Create("1"));

            Assert.NotSame(ok1, ok2);
            Assert.Equal(ok1, ok2);
            Assert.Equal(ok1.GetHashCode(), ok2.GetHashCode());
        }

        [t("GetHashCode(comparer) returns the same result for equal instances if OKs.")]
        public static void GetHashCode5() {
            var ok1 = Result<Tuple<string>, string>.Of(Tuple.Create("1"));
            var v1 = (IStructuralEquatable)ok1;

            var ok2 = Result<Tuple<string>, string>.Of(Tuple.Create("1"));
            var v2 = (IStructuralEquatable)ok2;

            Assert.Equal(v1.GetHashCode(s_Comparer), v2.GetHashCode(s_Comparer));
        }

        [t("GetHashCode(comparer) returns the same result for equal instances if NOKs.")]
        public static void GetHashCode6() {
            var nok1 = Result<Obj, string>.FromError("error");
            var v1 = (IStructuralEquatable)nok1;

            var nok2 = Result<Obj, string>.FromError("error");
            var v2 = (IStructuralEquatable)nok2;

            Assert.Equal(v1.GetHashCode(s_Comparer), v2.GetHashCode(s_Comparer));
        }

        [t("ToString() result contains a string representation of the value if OK, of the error if NOK.")]
        public static void ToString1() {
            var value = new Obj("My value");
            var ok = Result<Obj, Obj>.Of(value);
            Assert.Contains(value.ToString(), ok.ToString(), StringComparison.OrdinalIgnoreCase);

            var error = new Obj("My error");
            var nok = Result<Obj, Obj>.FromError(error);
            Assert.Contains(error.ToString(), nok.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public static partial class ResultFacts {
        [t("ToEnumerable() result is empty if NOK.")]
        public static void ToEnumerable1() {
            var nok = Result<Obj, string>.FromError("error");
            var seq = nok.ToEnumerable();

            Assert.Empty(seq);
        }

        [t("ToEnumerable() result is a sequence made of exactly one element if OK.")]
        public static void ToEnumerable2() {
            var obj = new Obj();
            var ok = Result<Obj, string>.Of(obj);
            var seq = ok.ToEnumerable();

            Assert.Equal(Enumerable.Repeat(obj, 1), seq);
        }

        [t("GetEnumerator() does not iterate if NOK.")]
        public static void GetEnumerator1() {
            var nok = Result<Obj, string>.FromError("error");
            var count = 0;

            foreach (var x in nok) { count++; }

            Assert.Equal(0, count);
        }

        [t("GetEnumerator() iterates only once if OK.")]
        public static void GetEnumerator2() {
            var exp = new Obj();
            var ok = Result<Obj, string>.Of(exp);
            var count = 0;

            foreach (var x in ok) { count++; Assert.Same(exp, x); }

            Assert.Equal(1, count);
        }

        [t("Contains() guards.")]
        public static void Contains0() {
            var value = new Obj();

            var ok = Result<Obj, string>.Of(new Obj());
            Assert.Throws<ArgumentNullException>("comparer", () => ok.Contains(value, null));

            var nok = Result<Obj, string>.FromError("error");
            Assert.Throws<ArgumentNullException>("comparer", () => nok.Contains(value, null));
        }

        [t("Match() guards.")]
        public static void Match0() {
            var ok = Result<Obj, string>.Of(new Obj());
            Assert.Throws<ArgumentNullException>("caseSuccess", () => ok.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => ok.Match(val => val, null));

            var nok = Result<Obj, string>.FromError("error");
            Assert.Throws<ArgumentNullException>("caseSuccess", () => nok.Match(null, _ => new Obj()));
            Assert.Throws<ArgumentNullException>("caseError", () => nok.Match(val => val, null));
        }
    }

    public static partial class ResultFacts {
        private static readonly EqualityComparer s_Comparer = new EqualityComparer();

        private sealed class EqualityComparer : IEqualityComparer {
            public new bool Equals(object left, object right) => left?.Equals(right) ?? right == null;

            public int GetHashCode(object obj) => obj?.GetHashCode() ?? 0;
        }
    }
}
