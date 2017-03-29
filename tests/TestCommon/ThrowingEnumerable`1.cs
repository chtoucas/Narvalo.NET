// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Class to help for deferred execution tests: it throw an exception
    /// if GetEnumerator is called.
    /// </summary>
    public sealed class ThrowingEnumerable<T> : IEnumerable<T> {
        public IEnumerator<T> GetEnumerator() => throw new InvalidOperationException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
