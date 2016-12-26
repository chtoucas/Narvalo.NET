// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Currencies
{
    using System;

    public static class CurrencyUnit
    {
        public static bool IsUnit<TCurrency>() where TCurrency : Currency
        {
            return typeof(TCurrency).Namespace == typeof(CurrencyUnit).Namespace;
        }

        public static bool IsUnit<TCurrency>(this TCurrency @this) where TCurrency : Currency
        {
            Require.NotNull(@this, nameof(@this));

            return @this is CurrencyUnit && @this.GetType().Namespace == typeof(CurrencyUnit).Namespace;
        }
    }

    public class CurrencyUnit<TCurrency>
    {
        internal CurrencyUnit(string code) { }

        public string Code { get; set; }
    }

    public class ZZZ : CurrencyUnit<ZZZ>
    {
        private ZZZ() : base("ZZZ") { }

        public static ZZZ Unit { get { Warrant.NotNull<Currency>(); return Uniq.Instance; } }

        public static explicit operator ZZZ(Currency value) => FromCurrency(value);

        public static explicit operator Currency(ZZZ value) => value.ToCurrency();

        public Currency ToCurrency() => Currency.Of(Code);

        public static ZZZ FromCurrency(Currency value)
        {
            if (value.Code != "ZZZ") { throw new NotSupportedException(); }

            return Unit;
        }

        private class Uniq
        {
            static Uniq() { }
            internal static readonly ZZZ Instance = new ZZZ();
        }
    }

    internal static class CurrencyFactory
    {
        public static void GetUnit(string code)
        {

        }

        public static void CreateInstance<TCurrency>() where TCurrency : Currency
        {
            Currency currency = Currency.Of(typeof(TCurrency).Name);
        }
    }
}
