// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    // Used internally to ensure that all money types behave like a numeric type.
    internal interface IAmount<T, TAmount>
    {
        TAmount Amount { get; }

        bool IsZero { get; }
        bool IsNegative { get; }
        bool IsNegativeOrZero { get; }
        bool IsPositive { get; }
        bool IsPositiveOrZero { get; }

        int Sign { get; }

        sbyte ToSByte();
        ushort ToUInt16();
        uint ToUInt32();
        ulong ToUInt64();
        byte ToByte();
        short ToInt16();
        int ToInt32();
        long ToInt64();
        decimal ToDecimal();

        T Plus(T other);
        T Plus(TAmount amount);

        T Minus(T other);
        T Minus(TAmount amount);

        T MultiplyBy(TAmount multiplier);

        T DivideBy(TAmount divisor);

        T Mod(TAmount divisor);

        T Negate();

        T Plus();
    }
}
