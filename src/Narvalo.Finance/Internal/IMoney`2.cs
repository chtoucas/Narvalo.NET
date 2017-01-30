// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;

    using Narvalo.Finance.Rounding;

    // Used internally to ensure that Money and Money<T> look alike.
    internal interface IMoney<T, out TCurrency>
        : IAmount<T, decimal>, IEquatable<T>, IComparable<T>, IComparable, IFormattable
    {
        TCurrency Currency { get; }

        bool IsNormalized { get; }
        bool IsRoundable { get; }
        bool IsRounded { get; }

        T Normalize(MidpointRounding mode);
        T Normalize(IRoundingAdjuster adjuster);

        decimal ToMinor();
        long? ToLongMinor();

        T Plus(uint amount);
        T Plus(ulong amount);
        T Plus(int amount);
        T Plus(long amount);

        T Minus(uint amount);
        T Minus(ulong amount);
        T Minus(int amount);
        T Minus(long amount);

        T MultiplyBy(uint multiplier);
        T MultiplyBy(ulong multiplier);
        T MultiplyBy(int multiplier);
        T MultiplyBy(long multiplier);

        T IncrementMajor();
        T IncrementMinor();

        T DecrementMajor();
        T DecrementMinor();
    }
}
