// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Numerics;

    public enum RoundingMode
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

    public static class RoundingModeExtensions
    {
        public static MidpointRounding ToMidpointRounding(this RoundingMode @this)
        {
            if (@this == RoundingMode.ToEven)
            {
                return MidpointRounding.ToEven;
            }
            else if (@this == RoundingMode.AwayFromZero)
            {
                return MidpointRounding.AwayFromZero;
            }
            else
            {
                throw new ArgumentException("XXX", nameof(@this));
            }
        }

        public static NumberRounding ToNumberRounding(this RoundingMode @this)
        {
            if (@this == RoundingMode.ToEven)
            {
                return NumberRounding.ToEven;
            }
            else if (@this == RoundingMode.AwayFromZero)
            {
                return NumberRounding.HalfAwayFromZero;
            }
            else
            {
                throw new ArgumentException("XXX", nameof(@this));
            }
        }
    }
}
