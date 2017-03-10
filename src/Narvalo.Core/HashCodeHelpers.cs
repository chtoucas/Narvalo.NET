// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    public static class HashCodeHelpers
    {
        // 31 is chosen because the multiplication can be replaced by:
        // > 31 * h = (h << 5) - h
        // for detailed explanations, see "Effective Java" by Joshua Bloch.
        // In https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Tuple.cs, we have:
        // > (((h1 << 5) + h1) ^ h2)
        // REVIEW: It seems that in the near future .NET will have a built-in utility for that.
        // There is already
        // https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Numerics/Hashing/HashHelpers.cs
        // which is used to compute a randomized hash code for ValueTuple's.
        private const int INIT = 17;
        private const int MULT = 31;

        public static int Combine(int hash1, int hash2)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + hash1;
                hash = MULT * hash + hash2;
                return hash;
            }
        }

        public static int Combine(int hash1, int hash2, int hash3)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + hash1;
                hash = MULT * hash + hash2;
                hash = MULT * hash + hash3;
                return hash;
            }
        }
    }
}
