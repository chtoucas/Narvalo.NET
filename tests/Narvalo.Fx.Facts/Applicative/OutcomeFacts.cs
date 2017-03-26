// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    // Tests for Outcome.
    public static partial class OutcomeFacts {
        #region Ok

        [t("Ok is OK.")]
        public static void Ok1() {
            Assert.True(Outcome.Ok.IsSuccess);
            Assert.False(Outcome.Ok.IsError);
        }

        #endregion

        #region FromError()

        [t("FromError() guards.")]
        public static void FromError0() {
            Assert.Throws<ArgumentNullException>("error", () => Outcome.FromError(null));
            Assert.Throws<ArgumentException>("error", () => Outcome.FromError(String.Empty));
        }

        [t("FromError() returns NOK.")]
        public static void FromError1() {
            var result = Outcome.FromError("error");
            Assert.True(result.IsError);
        }

        #endregion

        #region Bind()

        [t("Bind() guards.")]
        public static void Bind0() {
            Assert.Throws<ArgumentNullException>("binder", () => Outcome.Ok.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => Outcome.FromError("error").Bind<string>(null));
        }

        #endregion
    }
}
