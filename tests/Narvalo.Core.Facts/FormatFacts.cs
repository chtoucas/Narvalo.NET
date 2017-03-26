// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System;

    using Xunit;

    public static class FormatFacts {
        internal sealed class factAttribute : FactAttribute_ {
            public factAttribute(string message) : base(nameof(Format), message) { }
        }

        [fact("Current() guards.")]
        public static void Current_guards() {
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1));
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1, 2));
            Assert.Throws<ArgumentNullException>("format", () => Format.Current(null, 1, 2, 3));
        }

        [fact("Current() accepts null args.")]
        public static void Current_does_not_throw() {
            Format.Current("{0}", default(string));
            Format.Current("{0} {1}", default(string), default(string));
            Format.Current("{0} {1} {2}", default(string), default(string), default(string));
        }

        [fact("Current() throws FormatException when the format is not valid.")]
        public static void Current_throws() {
            Assert.Throws<FormatException>(() => Format.Current("{0} {1}", 1));
            Assert.Throws<FormatException>(() => Format.Current("{0} {1} {2}", 1, 2));
            Assert.Throws<FormatException>(() => Format.Current("{0} {1} {2} {3}", 1, 2, 3));
        }
    }
}
