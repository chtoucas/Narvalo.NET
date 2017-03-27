// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

    using static global::My;

    // Tests for Fallible.
    public static partial class FallibleFacts {
        [t("Ok is OK.")]
        public static void Ok1() {
            Assert.True(Fallible.Ok.IsSuccess);
            Assert.False(Fallible.Ok.IsError);
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

        [t("GetHashCode() returns the same result when called repeatedly.")]
        public static void GetHashCode1() {
            var nok = Fallible.FromError(Error);
            Assert.Equal(nok.GetHashCode(), nok.GetHashCode());

            var ok = Fallible.Ok;
            Assert.Equal(ok.GetHashCode(), ok.GetHashCode());
        }

        [t("GetHashCode() returns the same result for equal instances.")]
        public static void GetHashCode2() {
            var nok1 = Fallible.FromError(Error);
            var nok2 = Fallible.FromError(Error);

            Assert.NotSame(nok1, nok2);
            Assert.Equal(nok1, nok2);
            Assert.Equal(nok1.GetHashCode(), nok2.GetHashCode());
        }

        [t("ToString() result contains a string representation of the value if OK, of the error if NOK.")]
        public static void ToString1() {
            var ok = Fallible.Ok;
            Assert.Equal("Success", ok.ToString());

            var nok = Fallible.FromError(Error);
            Assert.Contains(Error.ToString(), nok.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public static partial class FallibleFacts {
        [t("Bind() guards.")]
        public static void Bind0() {
            var ok = Fallible.Ok;
            Assert.Throws<ArgumentNullException>("binder", () => ok.Bind<string>(null));

            var nok = Fallible.FromError(Error);
            Assert.Throws<ArgumentNullException>("binder", () => nok.Bind<string>(null));
        }
    }

    public static partial class FallibleFacts {
        private static readonly Lazy<ExceptionDispatchInfo> s_Error
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);

        internal static ExceptionDispatchInfo Error => s_Error.Value;

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
