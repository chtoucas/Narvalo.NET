// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Sequences
{
    using System;

    public static class Factorial
    {
        public static Func<int, int> Recurse()
            => YCombinator.Fix<int, int>(iter => i => i == 0 ? 1 : i * iter(i - 1));
    }
}
