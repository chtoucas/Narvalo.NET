// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System;

    using Narvalo.Finance.Rounding;

    // Used internally to ensure that Money and Money<T> are in sync.
    internal interface IMoney<T> : IEquatable<T>, IComparable<T>, IComparable, IFormattable
    {
        decimal Amount { get; }

        bool IsNormalized { get; }
        bool IsRoundable { get; }

        bool IsZero { get; }
        bool IsNegative { get; }
        bool IsNegativeOrZero { get; }
        bool IsPositive { get; }
        bool IsPositiveOrZero { get; }

        int Sign { get; }

        T Normalize(MidpointRounding mode);
        T Normalize(IRoundingAdjuster adjuster);

        //decimal ToMinor();
        //long? ToLongMinor();

        //T Plus(T other);
        //T Plus(uint amount);
        //T Plus(ulong amount);
        //T Plus(int amount);
        //T Plus(long amount);
        //T Plus(decimal amount);

        //T Minus(T other);
        //T Minus(uint amount);
        //T Minus(ulong amount);
        //T Minus(int amount);
        //T Minus(long amount);
        //T Minus(decimal amount);

        //T MultiplyBy(uint multiplier);
        //T MultiplyBy(ulong multiplier);
        //T MultiplyBy(int multiplier);
        //T MultiplyBy(long multiplier);

        //T DivideBy(decimal divisor);

        T Mod(decimal divisor);

        T IncrementMajor();
        T IncrementMinor();

        T DecrementMajor();
        T DecrementMinor();

        T Negate();

        T Plus();
    }
}
