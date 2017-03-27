// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

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

            Assert.NotSame(nok1, nok2);
            Assert.Equal(nok1, nok2);
            Assert.Equal(nok1.GetHashCode(), nok2.GetHashCode());
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
        [t("Bind() guards.")]
        public static void Bind0() {
            var ok = Outcome.Ok;
            Assert.Throws<ArgumentNullException>("binder", () => ok.Bind<string>(null));

            var nok = Outcome.FromError("error");
            Assert.Throws<ArgumentNullException>("binder", () => nok.Bind<string>(null));
        }
    }
}
