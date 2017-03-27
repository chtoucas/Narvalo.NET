// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    using static global::My;

    // Tests for Outcome.
    public static partial class OutcomeFacts {
        [t("Ok is OK.")]
        public static void Ok1() {
            Assert.True(Outcome.Ok.IsSuccess);
            Assert.False(Outcome.Ok.IsError);
        }

        [t("FromError() guards.")]
        public static void FromError0() {
            Assert.Throws<ArgumentNullException>("error", () => Outcome.FromError(null));
            Assert.Throws<ArgumentException>("error", () => Outcome.FromError(String.Empty));
        }

        [t("FromError() returns NOK.")]
        public static void FromError1() {
            var nok = Outcome.FromError("error");
            Assert.True(nok.IsError);
        }

        [t("== and != when both sides are NOK.")]
        public static void Equality1() {
            var nok1 = Outcome.FromError("error");
            var nok2 = Outcome.FromError("error");
            Assert.True(nok1 == nok2);
            Assert.False(nok1 != nok2);

            var nok3 = Outcome.FromError("error1");
            var nok4 = Outcome.FromError("error2");
            Assert.False(nok3 == nok4);
            Assert.True(nok3 != nok4);
        }

        [t("!= when one of the sides is OK.")]
        public static void Equality3() {
            var ok = Outcome.Ok;
            var nok = Outcome.FromError("error");
            Assert.True(nok != ok);
        }

        [t("Equals() is reflexive.")]
        public static void Equals1() {
            var ok = Outcome.Ok;
            Assert.True(ok.Equals(ok));

            var nok = Outcome.FromError("error");
            Assert.True(nok.Equals(nok));
        }

        [t("Equals(null) returns false.")]
        public static void Equals2() {
            var ok = Outcome.Ok;
            Assert.False(ok.Equals(null));

            var nok = Outcome.FromError("error");
            Assert.False(nok.Equals(null));
        }

        [t("Equals(obj) returns false if obj is not Outcome.")]
        public static void Equals3() {
            var ok = Outcome.Ok;
            Assert.False(ok.Equals(new Obj()));

            var nok = Outcome.FromError("error");
            Assert.False(nok.Equals(new Obj()));
        }

        [t("Equals() returns true only if instances wrap the same error.")]
        public static void Equals4() {
            var ok = Outcome.Ok;
            var nok = Outcome.FromError("error");
            var nok1 = Outcome.FromError("error1");
            var nok2 = Outcome.FromError("error1");

            Assert.False(ok.Equals(nok));
            Assert.False(nok1.Equals(nok));
            Assert.True(nok1.Equals(nok2));
        }

        [t("GetHashCode() returns the same result when called repeatedly.")]
        public static void GetHashCode1() {
            var nok = Outcome.FromError("error");
            Assert.Equal(nok.GetHashCode(), nok.GetHashCode());

            var ok = Outcome.Ok;
            Assert.Equal(ok.GetHashCode(), ok.GetHashCode());
        }

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode2() {
            var nok1 = Outcome.FromError("error");
            var nok2 = Outcome.FromError("error");
            Assert.Equal(nok1.GetHashCode(), nok2.GetHashCode());
        }

        [t("GetHashCode() returns different results for non-equal instances.")]
        public static void GetHashCode3() {
            var nok1 = Outcome.FromError("error1");
            var nok2 = Outcome.FromError("error2");
            Assert.NotEqual(nok1.GetHashCode(), nok2.GetHashCode());

            var ok = Outcome.Ok;
            Assert.NotEqual(nok1.GetHashCode(), ok.GetHashCode());
        }

        [t("ToString() result contains a string representation of the value if OK, or is 'Success' if NOK.")]
        public static void ToString1() {
            var ok = Outcome.Ok;
            Assert.Equal("Success", ok.ToString());

            var error = "My error";
            var nok = Outcome.FromError(error);
            Assert.Contains(error, nok.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public static partial class OutcomeFacts {
        [t("Match() guards.")]
        public static void Match0() {
            var ok = Outcome.Ok;
            Assert.Throws<ArgumentNullException>("caseSuccess", () => ok.Match(null, default(Func<string, Obj>)));
            Assert.Throws<ArgumentNullException>("caseError", () => ok.Match(() => default(Obj), null));

            var nok = Outcome.FromError("error");
            Assert.Throws<ArgumentNullException>("caseSuccess", () => nok.Match(null, default(Func<string, Obj>)));
            Assert.Throws<ArgumentNullException>("caseError", () => nok.Match(() => default(Obj), null));
        }

        [t("Match() calls 'caseSuccess' if OK.")]
        public static void Match1() {
            var ok = Outcome.Ok;
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("caseSuccess");
            Func<Obj> caseSuccess = () => { wasCalled = true; return exp; };
            Func<string, Obj> caseError = err => { notCalled = false; return new Obj(err); };

            var result = ok.Match(caseSuccess, caseError);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Match() calls 'caseError' if NOK.")]
        public static void Match2() {
            var nok = Outcome.FromError("error");
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("caseError");
            Func<Obj> caseSuccess = () => { notCalled = false; return new Obj("caseSuccess"); };
            Func<string, Obj> caseError = err => { wasCalled = true; return exp; };

            var result = nok.Match(caseSuccess, caseError);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Do() guards.")]
        public static void Do0() {
            var ok = Outcome.Ok;
            Assert.Throws<ArgumentNullException>("onSuccess", () => ok.Do(null, _ => { }));
            Assert.Throws<ArgumentNullException>("onError", () => ok.Do(() => { }, null));

            var nok = Outcome.FromError("error");
            Assert.Throws<ArgumentNullException>("onSuccess", () => nok.Do(null, _ => { }));
            Assert.Throws<ArgumentNullException>("onError", () => nok.Do(() => { }, null));
        }

        [t("Do() calls 'onSuccess' if OK.")]
        public static void Do1() {
            var ok = Outcome.Ok;
            var wasCalled = false;
            var notCalled = true;
            Action onSuccess = () => wasCalled = true;
            Action<string> onError = err => notCalled = false;

            ok.Do(onSuccess, onError);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("Do() calls 'onError' if NOK.")]
        public static void Do2() {
            var nok = Outcome.FromError("error");
            var wasCalled = false;
            var notCalled = true;
            Action onSuccess = () => notCalled = false;
            Action<string> onError = _ => wasCalled = true;

            nok.Do(onSuccess, onError);

            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("OnSuccess() guards.")]
        public static void OnSuccess0() {
            var ok = Outcome.Ok;
            Assert.Throws<ArgumentNullException>("action", () => ok.OnSuccess(null));

            var nok = Outcome.FromError("error");
            Assert.Throws<ArgumentNullException>("action", () => nok.OnSuccess(null));
        }

        [t("OnSuccess() calls 'action' if OK.")]
        public static void OnSuccess1() {
            var ok = Outcome.Ok;
            var wasCalled = false;
            Action act = () => wasCalled = true;

            ok.OnSuccess(act);
            Assert.True(wasCalled);
        }

        [t("OnSuccess() does not call 'action' if NOK.")]
        public static void OnSuccess2() {
            var nok = Outcome.FromError("error");
            var notCalled = true;
            Action act = () => notCalled = false;

            nok.OnSuccess(act);
            Assert.True(notCalled);
        }

        [t("OnError() guards.")]
        public static void OnError0() {
            var ok = Outcome.Ok;
            Assert.Throws<ArgumentNullException>("action", () => ok.OnError(null));

            var nok = Outcome.FromError("error");
            Assert.Throws<ArgumentNullException>("action", () => nok.OnError(null));
        }

        [t("OnError() calls 'action' if NOK.")]
        public static void OnError1() {
            var nok = Outcome.FromError("error");
            var wasCalled = false;
            Action<string> act = _ => wasCalled = true;

            nok.OnError(act);
            Assert.True(wasCalled);
        }

        [t("OnError() does not call 'action' if OK.")]
        public static void OnError2() {
            var ok = Outcome.Ok;
            var notCalled = true;
            Action<string> act = _ => notCalled = false;

            ok.OnError(act);
            Assert.True(notCalled);
        }
    }

    // Tests for the monadic methods.
    public static partial class OutcomeFacts {
        [t("Bind() guards.")]
        public static void Bind0() {
            var ok = Outcome.Ok;
            Assert.Throws<ArgumentNullException>("binder", () => ok.Bind<string>(null));

            var nok = Outcome.FromError("error");
            Assert.Throws<ArgumentNullException>("binder", () => nok.Bind<string>(null));
        }

        [t("Bind() returns NOK if NOK.")]
        public static void Bind1() {
            var nok = Outcome.FromError("error");
            Func<Outcome<string>> binder = () => Outcome.Of("value");

            var result = nok.Bind(binder);
            Assert.True(result.IsError);
        }

        [t("Bind() returns OK if OK.")]
        public static void Bind2() {
            var ok = Outcome.Ok;
            Func<Outcome<string>> binder = () => Outcome.Of("value");

            var result = ok.Bind(binder);
            Assert.True(result.IsSuccess);
        }

        [t("Map() returns OK if OK.")]
        public static void Map1() {
            var ok = Outcome.Ok;
            Func<int> selector = () => 1;

            var result = ok.Map(selector);
            Assert.True(result.IsSuccess);
        }

        [t("Map() returns NOK if NOK.")]
        public static void Map2() {
            var nok = Outcome.FromError("error");
            Func<int> selector = () => 1;

            var result = nok.Map(selector);
            Assert.True(result.IsError);
        }

        [t("ReplaceBy() returns OK if OK.")]
        public static void ReplaceBy1() {
            var ok = Outcome.Ok;
            var exp = Outcome.Of(1);

            var result = ok.ReplaceBy(exp);
            Assert.True(result.IsSuccess);
        }

        [t("ReplaceBy() returns NOK if NOK.")]
        public static void ReplaceBy2() {
            var nok = Outcome.FromError("error");
            var exp = Outcome.Of(1);

            var result = nok.ReplaceBy(exp);
            Assert.True(result.IsError);
        }

        [t("ContinueWith(other) returns 'other' if OK.")]
        public static void ContinueWith1() {
            var ok = Outcome.Ok;
            var exp = Outcome.Of(1);

            var result = ok.ContinueWith(exp);
            Assert.Equal(exp, result);
        }

        [t("ContinueWith() returns NOK if NOK.")]
        public static void ContinueWith2() {
            var nok = Outcome.FromError("error");
            var other = Outcome.Of(1);

            var result = nok.ContinueWith(other);
            Assert.True(result.IsError);
        }
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class OutcomeFacts {
        [t("Bind() transports error if NOK.")]
        public static void Bind3() {
            var exp = "error";
            var err = Outcome.FromError(exp);
            Func<Outcome<string>> binder = () => Outcome.Of("value");

            var result = err.Bind(binder);
            Assert.Equal(exp, result.Error);
        }

        [t("Bind() applies binder if OK.")]
        public static void Bind4() {
            var exp = "value";
            var ok = Outcome.Ok;
            Func<Outcome<string>> binder = () => Outcome.Of(exp);

            var result = ok.Bind(binder);
            Assert.Equal(exp, result.Value);
        }

        [t("Map() applies selector if some.")]
        public static void Map3() {
            var ok = Outcome.Ok;
            Func<int> selector = () => 1;

            var result = ok.Map(selector);
            Assert.Equal(1, result.Value);
        }
    }

#endif
}
