// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;
    using System.Runtime.ExceptionServices;

    using Xunit;

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
            var result = Fallible.FromError(Error);

            Assert.True(result.IsError);
            Assert.False(result.IsSuccess);
        }

        [t("Bind() guards.")]
        public static void Bind0() {
            Assert.Throws<ArgumentNullException>("binder", () => Fallible.Ok.Bind<string>(null));
            Assert.Throws<ArgumentNullException>("binder", () => Fallible.FromError(Error).Bind<string>(null));
        }
    }

    public static partial class FallibleFacts {
        private static readonly Lazy<ExceptionDispatchInfo> s_Error
            = new Lazy<ExceptionDispatchInfo>(CreateExceptionDispatchInfo);

        internal static ExceptionDispatchInfo Error => s_Error.Value;

        private static ExceptionDispatchInfo CreateExceptionDispatchInfo() {
            try {
                throw new Exception("My message");
            } catch (Exception ex) {
                return ExceptionDispatchInfo.Capture(ex);
            }
        }
    }
}
