// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System;

    using Narvalo.Finance.Rounding;

    // Factory methods: FromXXX() methods produce normalized instances (except FromMoney()), OfXXX() do not.
    public static class MoneyFactory
    {
        public static Money<TCurrency> OfMajor<TCurrency>(decimal major)
            where TCurrency : Currency<TCurrency>
            => new Money<TCurrency>(major, false);

        public static Money<TCurrency> OfMinor<TCurrency>(decimal minor)
            where TCurrency : Currency<TCurrency>
        {
            var major = Money<TCurrency>.UnderlyingUnit.ConvertToMajor(minor);
            return new Money<TCurrency>(major, false);
        }

        public static Money<TCurrency> FromMajor<TCurrency>(decimal major)
            where TCurrency : Currency<TCurrency>
            => new Money<TCurrency>(major, true);

        public static Money<TCurrency> FromMinor<TCurrency>(decimal minor)
            where TCurrency : Currency<TCurrency>
        {
            var major = Money<TCurrency>.UnderlyingUnit.ConvertToMajor(minor);
            return new Money<TCurrency>(major, true);
        }

        public static Money<TCurrency> FromMajor<TCurrency>(decimal amount, MidpointRounding mode)
            where TCurrency : Currency<TCurrency>
        {
            var unit = Money<TCurrency>.UnderlyingUnit;
            var major = unit.HasFixedDecimalPlaces
                 ? Math.Round(amount, unit.DecimalPlaces, mode)
                 : amount;
            return new Money<TCurrency>(major, true);
        }

        public static Money<TCurrency> FromMajor<TCurrency>(decimal amount, IRoundingAdjuster adjuster)
            where TCurrency : Currency<TCurrency>
        {
            Require.NotNull(adjuster, nameof(adjuster));

            var unit = Money<TCurrency>.UnderlyingUnit;
            decimal major = unit.HasFixedDecimalPlaces
                ? adjuster.Round(amount, unit.DecimalPlaces)
                : amount;
            return new Money<TCurrency>(major, true);
        }

        public static Money<TCurrency> FromMinor<TCurrency>(decimal minor, MidpointRounding mode)
            where TCurrency : Currency<TCurrency>
        {
            var major = Money<TCurrency>.UnderlyingUnit.ConvertToMajor(minor);
            return FromMajor<TCurrency>(major, mode);
        }

        public static Money<TCurrency> FromMinor<TCurrency>(decimal minor, IRoundingAdjuster adjuster)
            where TCurrency : Currency<TCurrency>
        {
            var major = Money<TCurrency>.UnderlyingUnit.ConvertToMajor(minor);
            return FromMajor<TCurrency>(major, adjuster);
        }

        public static Money<TCurrency> FromMoney<TCurrency>(Money money)
            where TCurrency : Currency<TCurrency>
        {
            if (!(money.Currency.Code == Money<TCurrency>.UnderlyingUnit.Code))
            {
                throw new InvalidCastException();
            }

            return new Money<TCurrency>(money.Amount, money.IsNormalized);
        }
    }
}
