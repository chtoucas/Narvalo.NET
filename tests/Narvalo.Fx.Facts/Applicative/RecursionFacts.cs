// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative {
    using System;

    using Xunit;

    public static class RecursionFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Recursion), description) { }
        }

        [t("Fix() guards.")]
        public static void Fix0() {
            Assert.Throws<ArgumentNullException>("generator", () => Recursion.Fix(default(Func<Func<int, int>, Func<int, int>>)));
        }
    }
}
