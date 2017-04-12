// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

    using static global::My;

    using Assert = Narvalo.AssertExtended;

    // Tests for Fallible.
    public static partial class FallibleFacts {
        [t("Success is OK.")]
        public static void Success1() {
            Assert.True(Fallible.Success.IsSuccess);
            Assert.False(Fallible.Success.IsError);
        }

        [t("FromError() guards.")]
        public static void FromError0()
            => Assert.Throws<ArgumentNullException>("error", () => Fallible.FromError(null));

        [t("FromError() returns NOK.")]
        public static void FromError1() {
            var nok = Fallible.FromError(Error);
            Assert.True(nok.IsError);
            Assert.False(nok.IsSuccess);
        }

        [t("Deconstruct() if OK.")]
        public static void Deconstruct1() {
            var ok = Fallible.Success;
            var (succeed, err) = ok;
            Assert.True(succeed);
            Assert.Null(err);
        }

        [t("Deconstruct() if NOK.")]
        public static void Deconstruct2() {
            var nok = Fallible.FromError(Error);
            var (succeed, err) = nok;
            Assert.False(succeed);
            Assert.Same(Error, err);
        }

        [t("ThrowIfError() does not throw if OK.")]
        public static void ThrowIfError1() {
            var ok = Fallible.Success;
            Assert.DoesNotThrow(() => ok.ThrowIfError());
        }

        [t("ThrowIfError() throws if NOK.")]
        public static void ThrowIfError2() {
            var nok = Fallible.FromError(Error);
            var ex = Record.Exception(() => nok.ThrowIfError());

            Assert.NotNull(ex);
            Assert.IsType<SimpleException>(ex);
            Assert.Equal(ErrorMessage, ex.Message);
        }

        [t("ToExceptionInfo() throws if OK.")]
        public static void ToExceptionInfo1() {
            var ok = Fallible.Success;
            Assert.Throws<InvalidCastException>(() => ok.ToExceptionInfo());
        }

        [t("ToExceptionInfo() throws if NOK.")]
        public static void ToExceptionInfo2() {
            var nok = Fallible.FromError(Error);
            var result = nok.ToExceptionInfo();
            Assert.Equal(Error, result);
        }

        [t("Casting (to ExceptionDispatchInfo) throws if OK.")]
        public static void cast1() {
            var ok = Fallible.Success;
            Assert.Throws<InvalidCastException>(() => (ExceptionDispatchInfo)ok);
        }

        [t("Casting (to ExceptionDispatchInfo) returns the error if NOK.")]
        public static void cast2() {
            var nok = Fallible.FromError(Error);
            var result = (ExceptionDispatchInfo)nok;
            Assert.Equal(Error, result);
        }

        [t("== and != when both sides are NOK.")]
        public static void Equality1() {
            var nok1 = Fallible.FromError(Error);
            var nok2 = Fallible.FromError(Error);
            Assert.True(nok1 == nok2);
            Assert.False(nok1 != nok2);

            var nok3 = Fallible.FromError(Error);
            var nok4 = Fallible.FromError(Error1);
            Assert.False(nok3 == nok4);
            Assert.True(nok3 != nok4);
        }

        [t("!= when one of the sides is OK.")]
        public static void Equality3() {
            var ok = Fallible.Success;
            var nok = Fallible.FromError(Error);
            Assert.True(nok != ok);
        }

        [t("Equals() is reflexive.")]
        public static void Equals1() {
            var ok = Fallible.Success;
            Assert.True(ok.Equals(ok));

            var nok = Fallible.FromError(Error);
            Assert.True(nok.Equals(nok));
        }

        [t("Equals(null) returns false.")]
        public static void Equals2() {
            var ok = Fallible.Success;
            Assert.False(ok.Equals(null));

            var nok = Fallible.FromError(Error);
            Assert.False(nok.Equals(null));
        }

        [t("Equals(obj) returns false if obj is not Outcome.")]
        public static void Equals3() {
            var ok = Fallible.Success;
            Assert.False(ok.Equals(new Obj()));

            var nok = Fallible.FromError(Error);
            Assert.False(nok.Equals(new Obj()));
        }

        [t("Equals() returns true only if instances wrap the same error.")]
        public static void Equals4() {
            var ok = Fallible.Success;
            var nok = Fallible.FromError(Error);
            var nok1 = Fallible.FromError(Error1);
            var nok2 = Fallible.FromError(Error1);

            Assert.False(ok.Equals(nok));
            Assert.False(nok1.Equals(nok));
            Assert.True(nok1.Equals(nok2));
        }

        [t("GetHashCode() returns the same result when called repeatedly.")]
        public static void GetHashCode1() {
            var nok = Fallible.FromError(Error);
            Assert.Equal(nok.GetHashCode(), nok.GetHashCode());

            var ok = Fallible.Success;
            Assert.Equal(ok.GetHashCode(), ok.GetHashCode());
        }

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode2() {
            var nok1 = Fallible.FromError(Error);
            var nok2 = Fallible.FromError(Error);
            Assert.Equal(nok1.GetHashCode(), nok2.GetHashCode());
        }

        [t("GetHashCode() returns different results for non-equal instances.")]
        public static void GetHashCode3() {
            var nok1 = Fallible.FromError(Error);
            var nok2 = Fallible.FromError(Error1);
            Assert.NotEqual(nok1.GetHashCode(), nok2.GetHashCode());

            var ok = Fallible.Success;
            Assert.NotEqual(nok1.GetHashCode(), ok.GetHashCode());
        }

        [t("ToString() result contains a string representation of the value if OK, of the error if NOK.")]
        public static void ToString1() {
            var ok = Fallible.Success;
            Assert.Equal("Success", ok.ToString());

            var nok = Fallible.FromError(Error);
            Assert.Contains(Error.ToString(), nok.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public static partial class FallibleFacts {
        [t("TryWith() guards.")]
        public static void TryWith0() {
            Assert.Throws<ArgumentNullException>("action", () => Fallible.TryWith(null));
            Assert.Throws<ArgumentNullException>("func", () => Fallible.TryWith<string>(null));
        }

        [t("TryWith() returns OK when action does not throw.")]
        public static void TryWith1() {
            Action act = () => { };
            var result1 = Fallible.TryWith(act);
            Assert.True(result1.IsSuccess);

            Func<string> func = () => String.Empty;
            var result2 = Fallible.TryWith(func);
            Assert.True(result2.IsSuccess);
        }

        [t("TryWith() returns NOK when action throws.")]
        public static void TryWith2() {
            Action act = () => throw new SimpleException();
            var result1 = Fallible.TryWith(act);
            Assert.True(result1.IsError);

            Func<string> func = () => throw new SimpleException();
            var result2 = Fallible.TryWith(func);
            Assert.True(result2.IsError);
        }

        [t("TryFinally() guards.")]
        public static void TryFinally0() {
            Assert.Throws<ArgumentNullException>("action", () => Fallible.TryFinally(null, () => { }));
            Assert.Throws<ArgumentNullException>("finallyAction", () => Fallible.TryFinally(() => { }, null));

            Assert.Throws<ArgumentNullException>("func", () => Fallible.TryFinally<string>(null, () => { }));
            Assert.Throws<ArgumentNullException>("finallyAction", () => Fallible.TryFinally<string>(() => default(String), null));
        }

        [t("TryFinally() returns OK and calls 'finallyAction' when action does not throw.")]
        public static void TryFinally1() {
            Action act = () => { };
            var wasCalled1 = false;
            Action finallyAct1 = () => wasCalled1 = true;
            var result1 = Fallible.TryFinally(act, finallyAct1);

            Assert.True(result1.IsSuccess);
            Assert.True(wasCalled1);

            Func<string> func = () => String.Empty;
            var wasCalled2 = false;
            Action finallyAct2 = () => wasCalled2 = true;
            var result2 = Fallible.TryFinally(func, finallyAct2);

            Assert.True(result2.IsSuccess);
            Assert.True(wasCalled2);
        }

        [t("TryFinally() returns NOK and calls 'finallyAction' when action throws.")]
        public static void TryFinally2() {
            Action act = () => throw new SimpleException();
            var wasCalled1 = false;
            Action finallyAct1 = () => wasCalled1 = true;
            var result1 = Fallible.TryFinally(act, finallyAct1);

            Assert.True(result1.IsError);
            Assert.True(wasCalled1);

            Func<string> func = () => throw new SimpleException();
            var wasCalled2 = false;
            Action finallyAct2 = () => wasCalled2 = true;
            var result2 = Fallible.TryFinally(func, finallyAct2);

            Assert.True(result2.IsError);
            Assert.True(wasCalled2);
        }
    }

    public static partial class FallibleFacts {
        [t("Match() guards.")]
        public static void Match0() {
            var ok = Fallible.Success;
            Assert.Throws<ArgumentNullException>("caseSuccess", () => ok.Match(null, default(Func<ExceptionDispatchInfo, Obj>)));
            Assert.Throws<ArgumentNullException>("caseError", () => ok.Match(() => default(Obj), null));

            var nok = Fallible.FromError(Error);
            Assert.Throws<ArgumentNullException>("caseSuccess", () => nok.Match(null, default(Func<ExceptionDispatchInfo, Obj>)));
            Assert.Throws<ArgumentNullException>("caseError", () => nok.Match(() => default(Obj), null));
        }

        [t("Match() calls 'caseSuccess' if OK.")]
        public static void Match1() {
            var ok = Fallible.Success;
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("caseSuccess");
            Func<Obj> caseSuccess = () => { wasCalled = true; return exp; };
            Func<ExceptionDispatchInfo, Obj> caseError = err => { notCalled = false; return new Obj(err.ToString()); };
            var result = ok.Match(caseSuccess, caseError);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Match() calls 'caseError' if NOK.")]
        public static void Match2() {
            var nok = Fallible.FromError(Error);
            var wasCalled = false;
            var notCalled = true;
            var exp = new Obj("caseError");
            Func<Obj> caseSuccess = () => { notCalled = false; return new Obj("caseSuccess"); };
            Func<ExceptionDispatchInfo, Obj> caseError = err => { wasCalled = true; return exp; };
            var result = nok.Match(caseSuccess, caseError);

            Assert.True(notCalled);
            Assert.True(wasCalled);
            Assert.Same(exp, result);
        }

        [t("Do() guards.")]
        public static void Do0() {
            var ok = Fallible.Success;
            Assert.Throws<ArgumentNullException>("onSuccess", () => ok.Do(null, _ => { }));
            Assert.Throws<ArgumentNullException>("onError", () => ok.Do(() => { }, null));

            var nok = Fallible.FromError(Error);
            Assert.Throws<ArgumentNullException>("onSuccess", () => nok.Do(null, _ => { }));
            Assert.Throws<ArgumentNullException>("onError", () => nok.Do(() => { }, null));
        }

        [t("Do() calls 'onSuccess' if OK.")]
        public static void Do1() {
            var ok = Fallible.Success;
            var wasCalled = false;
            var notCalled = true;
            Action onSuccess = () => wasCalled = true;
            Action<ExceptionDispatchInfo> onError = err => notCalled = false;

            ok.Do(onSuccess, onError);
            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("Do() calls 'onError' if NOK.")]
        public static void Do2() {
            var nok = Fallible.FromError(Error);
            var wasCalled = false;
            var notCalled = true;
            Action onSuccess = () => notCalled = false;
            Action<ExceptionDispatchInfo> onError = _ => wasCalled = true;

            nok.Do(onSuccess, onError);
            Assert.True(notCalled);
            Assert.True(wasCalled);
        }

        [t("OnSuccess() guards.")]
        public static void OnSuccess0() {
            var ok = Fallible.Success;
            Assert.Throws<ArgumentNullException>("action", () => ok.OnSuccess(null));

            var nok = Fallible.FromError(Error);
            Assert.Throws<ArgumentNullException>("action", () => nok.OnSuccess(null));
        }

        [t("OnSuccess() calls 'action' if OK.")]
        public static void OnSuccess1() {
            var ok = Fallible.Success;
            var wasCalled = false;
            Action act = () => wasCalled = true;

            ok.OnSuccess(act);
            Assert.True(wasCalled);
        }

        [t("OnSuccess() does not call 'action' if NOK.")]
        public static void OnSuccess2() {
            var nok = Fallible.FromError(Error);
            var notCalled = true;
            Action act = () => notCalled = false;

            nok.OnSuccess(act);
            Assert.True(notCalled);
        }

        [t("OnError() guards.")]
        public static void OnError0() {
            var ok = Fallible.Success;
            Assert.Throws<ArgumentNullException>("action", () => ok.OnError(null));

            var nok = Fallible.FromError(Error);
            Assert.Throws<ArgumentNullException>("action", () => nok.OnError(null));
        }

        [t("OnError() calls 'action' if NOK.")]
        public static void OnError1() {
            var nok = Fallible.FromError(Error);
            var wasCalled = false;
            Action<ExceptionDispatchInfo> act = _ => wasCalled = true;

            nok.OnError(act);
            Assert.True(wasCalled);
        }

        [t("OnError() does not call 'action' if OK.")]
        public static void OnError2() {
            var ok = Fallible.Success;
            var notCalled = true;
            Action<ExceptionDispatchInfo> act = _ => notCalled = false;

            ok.OnError(act);
            Assert.True(notCalled);
        }
    }

    public static partial class FallibleFacts {
        [t("Bind() guards.")]
        public static void Bind0() {
            var ok = Fallible.Success;
            Assert.Throws<ArgumentNullException>("binder", () => ok.Bind<string>(null));

            var nok = Fallible.FromError(Error);
            Assert.Throws<ArgumentNullException>("binder", () => nok.Bind<string>(null));
        }

        [t("Bind() returns NOK if NOK.")]
        public static void Bind1() {
            var nok = Fallible.FromError(Error);
            Func<Fallible<string>> binder = () => Fallible.Of("value");

            var result = nok.Bind(binder);
            Assert.True(result.IsError);
        }

        [t("Bind() returns OK if OK.")]
        public static void Bind2() {
            var ok = Fallible.Success;
            Func<Fallible<string>> binder = () => Fallible.Of("value");

            var result = ok.Bind(binder);
            Assert.True(result.IsSuccess);
        }

        [t("Select() returns OK if OK.")]
        public static void Select1() {
            var ok = Fallible.Success;
            Func<int> selector = () => 1;

            var result = ok.Select(selector);
            Assert.True(result.IsSuccess);
        }

        [t("Select() returns NOK if NOK.")]
        public static void Select2() {
            var nok = Fallible.FromError(Error);
            Func<int> selector = () => 1;

            var result = nok.Select(selector);
            Assert.True(result.IsError);
        }

        [t("ReplaceBy() returns OK if OK.")]
        public static void ReplaceBy1() {
            var ok = Fallible.Success;
            var exp = Fallible.Of(1);

            var result = ok.ReplaceBy(exp);
            Assert.True(result.IsSuccess);
        }

        [t("ReplaceBy() returns NOK if NOK.")]
        public static void ReplaceBy2() {
            var nok = Fallible.FromError(Error);
            var exp = Fallible.Of(1);

            var result = nok.ReplaceBy(exp);
            Assert.True(result.IsError);
        }

        [t("ContinueWith(other) returns 'other' if OK.")]
        public static void ContinueWith1() {
            var ok = Fallible.Success;
            var exp = Fallible.Of(1);

            var result = ok.ContinueWith(exp);
            Assert.Equal(exp, result);
        }

        [t("ContinueWith() returns NOK if NOK.")]
        public static void ContinueWith2() {
            var nok = Fallible.FromError(Error);
            var other = Fallible.Of(1);

            var result = nok.ContinueWith(other);
            Assert.True(result.IsError);
        }
    }

#if !NO_INTERNALS_VISIBLE_TO

    public static partial class FallibleFacts {
        [t("Bind() transports error if NOK.")]
        public static void Bind3() {
            var nok = Fallible.FromError(Error);
            Func<Fallible<string>> binder = () => Fallible.Of("value");

            var result = nok.Bind(binder);
            Assert.Same(Error, result.Error);
        }

        [t("Bind() applies binder if OK.")]
        public static void Bind4() {
            var exp = new Obj("value");
            var ok = Fallible.Success;
            Func<Fallible<Obj>> binder = () => Fallible.Of(exp);

            var result = ok.Bind(binder);
            Assert.Same(exp, result.Value);
        }

        [t("Select() applies selector if some.")]
        public static void Select3() {
            var ok = Fallible.Success;
            Func<int> selector = () => 1;

            var result = ok.Select(selector);
            Assert.Equal(1, result.Value);
        }
    }

#endif

    public static partial class FallibleFacts {
        private static readonly Lazy<ExceptionDispatchInfo> s_Error
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);
        private static readonly Lazy<ExceptionDispatchInfo> s_Error1
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);

        private static ExceptionDispatchInfo Error => s_Error.Value;
        private static ExceptionDispatchInfo Error1 => s_Error1.Value;

        private static string ErrorMessage => "My error";

        private static ExceptionDispatchInfo CreateExceptionDispatchInfo() {
            try {
                throw new SimpleException(ErrorMessage);
            } catch (Exception ex) {
                return ExceptionDispatchInfo.Capture(ex);
            }
        }
    }
}
