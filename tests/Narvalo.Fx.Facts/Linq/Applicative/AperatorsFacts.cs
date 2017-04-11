// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq.Applicative {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Tests;

    public partial class AperatorsFacts : EnumerableTests {
        internal sealed class tAttribute : TestCaseAttribute {
            public tAttribute(string description) : base(nameof(Aperators), description) { }
        }

        internal sealed class TAttribute : TestTheoryAttribute {
            public TAttribute(string description) : base(nameof(Aperators), description) { }
        }

        private static IEnumerable<T> EmptySource<T>() {
            yield break;
        }

        /// <summary>
        /// Class to help for deferred execution tests: it throw an exception
        /// if GetEnumerator is called.
        /// </summary>
        private sealed class ThrowingEnumerable<T> : IEnumerable<T> {
            public IEnumerator<T> GetEnumerator() => throw new InvalidOperationException();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
