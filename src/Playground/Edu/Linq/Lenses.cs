// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Linq
{
    using System.Collections.Generic;

    using Narvalo.Fx;

    // Linq from scratch.
    public static class Lenses
    {
        #region Generation Operators

        public static IEnumerable<int> Range(int start, int count)
        {
            return Sequence.Create(i => i + 1, 0, i => start + i, i => i < count);
        }

        public static IEnumerable<T> Repeat<T>(T value)
        {
            return Sequence.Create(i => i + 1, 0, i => value);
        }

        public static IEnumerable<T> Repeat<T>(T value, int count)
        {
            return Sequence.Create(i => i + 1, 0, i => value, i => i < count);
        }

        public static IEnumerable<T> Empty<T>()
        {
            return Sequence.Create(i => i, 0, i => default(T), i => false);
        }

        #endregion
    }
}
