// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    // Tests for Outcome.
    public static partial class OutcomeFacts {
        internal sealed class factAttribute : FactAttribute_ {
            public factAttribute(string message) : base(nameof(Outcome), message) { }
        }

        #region Ok

        [fact("")]
        public static void Ok_IsSuccess() => Assert.True(Outcome.Ok.IsSuccess);

        #endregion

        #region FromError()

        [fact("")]
        public static void FromError_Guards() {
            Assert.Throws<ArgumentNullException>("error", () => Outcome.FromError(null));
            Assert.Throws<ArgumentException>("error", () => Outcome.FromError(String.Empty));
        }

        [fact("")]
        public static void FromError_ReturnsError() {
            var result = Outcome.FromError("error");
            Assert.True(result.IsError);
        }

        #endregion

        #region Bind()

        [fact("")]
        public static void Bind_Guards() {
            Assert.Throws<ArgumentNullException>("binder", () => Outcome.Ok.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => Outcome.FromError("error").Bind<string>(null));
        }

        #endregion
    }
}
