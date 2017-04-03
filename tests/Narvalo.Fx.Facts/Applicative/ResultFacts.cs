// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Collections;
    using System.Linq;

    using Xunit;

    using static global::My;

    using Assert = Narvalo.AssertExtended;

    // Tests for Result<T, TError>.
    public static partial class ResultFacts {
        [t("Deconstruct() if OK.")]
        public static void Deconstruct1() {
            var exp = new Obj();
            var ok = Result<Obj, string>.Of(exp);
            var (succeed, value, err) = ok;
            Assert.True(succeed);
            Assert.Same(exp, value);
            Assert.Null(err);
        }

        [t("Deconstruct() if NOK.")]
        public static void Deconstruct2() {
            var exp = new Obj();
            var nok = Result<string, Obj>.FromError(exp);
            var (succeed, value, err) = nok;
            Assert.False(succeed);
            Assert.Null(value);
            Assert.Same(exp, err);
        }

        [t("ThrowIfError() does not throw if OK.")]
        public static void ThrowIfError1() {
            var ok = Result<Obj, Exception>.Of(new Obj());
            Assert.DoesNotThrow(() => ok.ThrowIfError());
        }

        [t("ThrowIfError() throws if NOK.")]
        public static void ThrowIfError2() {
            var error = "My message";
            var nok = Result<Obj, SimpleException>.FromError(new SimpleException(error));
            var ex = Record.Exception(() => nok.ThrowIfError());

            Assert.NotNull(ex);
            Assert.IsType<SimpleException>(ex);
            Assert.Equal(error, ex.Message);
        }

        [t("GetHashCode() guards.")]
        public static void GetHashCode0() {
            var nok = (IStructuralEquatable)Result<Obj, string>.FromError("error");
            Assert.Throws<ArgumentNullException>("comparer", () => nok.GetHashCode(null));

            var ok = (IStructuralEquatable)Result<Obj, string>.Of(new Obj());
            Assert.Throws<ArgumentNullException>("comparer", () => ok.GetHashCode(null));
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
            var nok = (IStructuralEquatable)Result<Obj, string>.FromError("error");
            Assert.Equal(nok.GetHashCode(s_Comparer), nok.GetHashCode(s_Comparer));

            var ok1 = (IStructuralEquatable)Result<Obj, string>.Of(new Obj());
            Assert.Equal(ok1.GetHashCode(s_Comparer), ok1.GetHashCode(s_Comparer));

            var ok2 = (IStructuralEquatable)Result<int, string>.Of(1);
            Assert.Equal(ok2.GetHashCode(s_Comparer), ok2.GetHashCode(s_Comparer));
        }

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode3() {
            var nok1 = Result<Obj, string>.FromError("error");
            var nok2 = Result<Obj, string>.FromError("error");
            Assert.Equal(nok1.GetHashCode(), nok2.GetHashCode());

            var ok1 = Result<Tuple<string>, string>.Of(Tuple.Create("1"));
            var ok2 = Result<Tuple<string>, string>.Of(Tuple.Create("1"));
            Assert.Equal(ok1.GetHashCode(), ok2.GetHashCode());
        }

        [t("GetHashCode(comparer) returns the same result for equal instances.")]
        public static void GetHashCode4() {
            var nok1 = (IStructuralEquatable)Result<Obj, string>.FromError("error");
            var nok2 = (IStructuralEquatable)Result<Obj, string>.FromError("error");
            Assert.Equal(nok1.GetHashCode(s_Comparer), nok2.GetHashCode(s_Comparer));

            var ok1 = (IStructuralEquatable)Result<Tuple<string>, string>.Of(Tuple.Create("1"));
            var ok2 = (IStructuralEquatable)Result<Tuple<string>, string>.Of(Tuple.Create("1"));
            Assert.Equal(ok1.GetHashCode(s_Comparer), ok2.GetHashCode(s_Comparer));
        }

        [t("GetHashCode() returns different results for non-equal instances.")]
        public static void GetHashCode5() {
            var nok = Result<Obj, string>.FromError("error");
            var ok = Result<Obj, string>.Of(new Obj());
            Assert.NotEqual(nok.GetHashCode(), ok.GetHashCode());

            var nok1 = Result<Obj, string>.FromError("error1");
            var nok2 = Result<Obj, string>.FromError("error2");
            Assert.NotEqual(nok1.GetHashCode(), nok2.GetHashCode());

            var ok1 = Result<Obj, string>.Of(new Obj());
            var ok2 = Result<Obj, string>.Of(new Obj());
            Assert.NotEqual(ok1.GetHashCode(), ok2.GetHashCode());
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

    // Tests for the monadic methods.
    public static partial class ResultFacts {
        [t("Flatten() returns NOK if NOK.")]
        public static void Flatten1() {
            var nok = Result<Result<Obj, string>, string>.FromError("error");
            var result = nok.Flatten();
            Assert.True(result.IsError);
        }

        [t("Flatten() returns OK if OK.")]
        public static void Flatten2() {
            var ok = Result<Result<Obj, string>, string>.Of(Result<Obj, string>.Of(new Obj()));
            var result = ok.Flatten();
            Assert.True(result.IsSuccess);
        }

        [t("Flatten() returns NOK if NOK.")]
        public static void FlattenError1() {
            var nok = Result<Obj, Result<Obj, string>>.FromError(Result<Obj, string>.FromError("error"));
            var result = Result.FlattenError(nok);
            Assert.True(result.IsError);
        }

        [t("Flatten() returns OK if OK.")]
        public static void FlattenError2() {
            var ok1 = Result<Obj, Result<Obj, string>>.FromError(Result<Obj, string>.Of(new Obj()));
            var result1 = Result.FlattenError(ok1);
            Assert.True(result1.IsSuccess);

            var ok2 = Result<Obj, Result<Obj, string>>.Of(new Obj());
            var result2 = Result.FlattenError(ok2);
            Assert.True(result2.IsSuccess);
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
