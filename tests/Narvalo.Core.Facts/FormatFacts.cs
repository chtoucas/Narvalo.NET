// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System;

    using Xunit;

    using Assert = Narvalo.AssertExtended;

    public static class FormatFacts {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Format), description) { }
        }

        [t("Current() guards.")]
        public static void Current0() {
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1));
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1, 2));
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1, 2, 3));
        }

        [t("Current() accepts null args.")]
        public static void Current1() {
            Action act = () => {
                Format.Current("{0}", default(string));
                Format.Current("{0} {1}", default(string), default(string));
                Format.Current("{0} {1} {2}", default(string), default(string), default(string));
            };

            Assert.DoesNotThrow(act);
        }

        [t("Current() throws FormatException when the format is not valid.")]
        public static void Current2() {
            Assert.Throws<FormatException>(() => Format.Current("{0} {1}", 1));
            Assert.Throws<FormatException>(() => Format.Current("{0} {1} {2}", 1, 2));
            Assert.Throws<FormatException>(() => Format.Current("{0} {1} {2} {3}", 1, 2, 3));
        }
    }
}
