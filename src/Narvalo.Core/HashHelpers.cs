// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    public static class HashHelpers
    {
        // 31 is chosen because the multiplication can be replaced by:
        // > 31 * h = (h << 5) - h
        // for detailed explanations, see "Effective Java" by Joshua Bloch.
        // In https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Tuple.cs, we have:
        // > (((h1 << 5) + h1) ^ h2)
        // See also: https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Numerics/Hashing/HashHelpers.cs
        private const int INIT = 17;
        private const int MULT = 31;

        public static int Combine(int h1, int h2)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + h1;
                hash = MULT * hash + h2;
                return hash;
            }
        }

        public static int Combine(int h1, int h2, int h3)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + h1;
                hash = MULT * hash + h2;
                hash = MULT * hash + h3;
                return hash;
            }
        }

        public static int Combine(int h1, int h2, int h3, int h4)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + h1;
                hash = MULT * hash + h2;
                hash = MULT * hash + h3;
                hash = MULT * hash + h4;
                return hash;
            }
        }

        public static int Combine(int h1, int h2, int h3, int h4, int h5)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + h1;
                hash = MULT * hash + h2;
                hash = MULT * hash + h3;
                hash = MULT * hash + h4;
                hash = MULT * hash + h5;
                return hash;
            }
        }

        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + h1;
                hash = MULT * hash + h2;
                hash = MULT * hash + h3;
                hash = MULT * hash + h4;
                hash = MULT * hash + h5;
                hash = MULT * hash + h6;
                return hash;
            }
        }

        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + h1;
                hash = MULT * hash + h2;
                hash = MULT * hash + h3;
                hash = MULT * hash + h4;
                hash = MULT * hash + h5;
                hash = MULT * hash + h6;
                hash = MULT * hash + h7;
                return hash;
            }
        }

        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
        {
            unchecked
            {
                int hash = INIT;
                hash = MULT * hash + h1;
                hash = MULT * hash + h2;
                hash = MULT * hash + h3;
                hash = MULT * hash + h4;
                hash = MULT * hash + h5;
                hash = MULT * hash + h6;
                hash = MULT * hash + h7;
                hash = MULT * hash + h8;
                return hash;
            }
        }
    }
}
