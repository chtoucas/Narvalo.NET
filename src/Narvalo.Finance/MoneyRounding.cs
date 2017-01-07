// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Numerics;

    public enum MoneyRounding
    {
        /// <summary>
        /// The number should be kept as it.
        /// </summary>
        None,

        /// <summary>
        /// The number is already rounded.
        /// </summary>
        Unnecessary,

        /// <summary>
        /// When a number is halfway between two others, it is rounded toward the nearest even number.
        /// </summary>
        ToEven,

        /// <summary>
        /// When a number is halfway between two others, it is rounded toward the nearest
        /// number that is away from zero.
        /// </summary>
        AwayFromZero,

        /// <summary>
        /// Default IEEE 754 rounding mode.
        /// </summary>
        Default = ToEven,
    }

    public static class MoneyRoundingExtensions
    {
        public static decimal Round(this MoneyRounding @this, decimal amount, Currency currency)
        {
            if (@this == MoneyRounding.ToEven)
            {
                return DecimalRounding.RoundToEven(amount, currency.DecimalPlaces);
            }
            else if (@this == MoneyRounding.AwayFromZero)
            {
                return DecimalRounding.RoundHalfAwayFromZero(amount, currency.DecimalPlaces);
            }
            else
            {
                return amount;
            }
        }

        public static MidpointRounding ToMidpointRounding(this MoneyRounding @this)
        {
            if (@this == MoneyRounding.ToEven)
            {
                return MidpointRounding.ToEven;
            }
            else if (@this == MoneyRounding.AwayFromZero)
            {
                return MidpointRounding.AwayFromZero;
            }
            else
            {
                throw new ArgumentException("XXX", nameof(@this));
            }
        }

        public static RoundingMode ToRoundingMode(this MoneyRounding @this)
        {
            if (@this == MoneyRounding.ToEven)
            {
                return RoundingMode.ToEven;
            }
            else if (@this == MoneyRounding.AwayFromZero)
            {
                return RoundingMode.HalfAwayFromZero;
            }
            else
            {
                throw new ArgumentException("XXX", nameof(@this));
            }
        }
    }
}
