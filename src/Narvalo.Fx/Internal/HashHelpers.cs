// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Narvalo.Fx.Internal
{
    using System;

    // Adapted from https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Numerics/Hashing/HashHelpers.cs
    // and https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/ValueTuple.cs
    // NB: There is another version in
    // https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Tuple.cs
    internal static class HashHelpers
    {
        public static readonly int RandomSeed = new Random().Next(Int32.MinValue, Int32.MaxValue);

        public static int Combine(int h1, int h2) => CombineCore(CombineCore(RandomSeed, h1), h2);

        public static int CombineCore(int h1, int h2)
        {
            // RyuJIT optimizes this to use the ROL instruction
            // Related GitHub pull request: dotnet/coreclr#1830
            uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
            return ((int)rol5 + h1) ^ h2;
        }
    }
}