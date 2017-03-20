// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    /// <summary>
    /// Provides helper methods to combine hash codes.
    /// </summary>
    //
    //
    // The multiplier (31) is chosen because the multiplication can be replaced
    // by the compiler by:
    // > 31 * h = (h << 5) - h
    // For detailed explanations, see "Effective Java" by Joshua Bloch.
    // Note that the .NET team is planning for a built-in utility for that.
    //
    // Current alternatives that I know of are:
    // - https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Tuple.cs
    //   > (((h1 << 5) + h1) ^ h2)
    // - https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Numerics/Hashing/HashHelpers.cs
    //   which is used to compute a randomized hash code for ValueTuple's.
    // - https://github.com/ASP-NET-MVC/aspnetwebstack/blob/master/src/Common/HashCodeCombiner.cs
    public static class HashCodeHelpers
    {
        /// <summary>
        /// Initial hash value.
        /// </summary>
        private const int INIT = 17;

        /// <summary>
        /// Multiplier for each value.
        /// </summary>
        private const int MULT = 31;

        /// <summary>
        /// Combines two hash codes.
        /// </summary>
        /// <param name="hash1">The first hash code.</param>
        /// <param name="hash2">The second hash code.</param>
        /// <returns>A combined hash code of the <paramref name="hash1"/>
        /// and <paramref name="hash2"/>.</returns>
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

        /// <summary>
        /// Combines three hash codes.
        /// </summary>
        /// <param name="hash1">The first hash code.</param>
        /// <param name="hash2">The second hash code.</param>
        /// <param name="hash3">The third hash code.</param>
        /// <returns>A combined hash code of <paramref name="hash1"/>,
        /// <paramref name="hash2"/> and <paramref name="hash3"/>.</returns>
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
