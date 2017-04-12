// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;

// Local additions to EnumerableTests.

namespace System.Linq.Tests {
    public abstract partial class EnumerableTests {
        protected static IEnumerable<T> EmptySource<T>() {
            yield break;
        }

        /// <summary>
        /// Class to help for deferred execution tests: it throw an exception
        /// if GetEnumerator is called.
        /// </summary>
        protected sealed class ThrowingEnumerable<T> : IEnumerable<T> {
            public IEnumerator<T> GetEnumerator() => throw new InvalidOperationException();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
