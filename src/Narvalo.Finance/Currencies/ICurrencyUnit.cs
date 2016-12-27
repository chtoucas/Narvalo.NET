// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Currencies
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    //public static class CurrencyUnit
    //{
    //    public static bool IsUnit<TCurrency>() where TCurrency : Currency
    //    {
    //        return typeof(TCurrency).Namespace == typeof(CurrencyUnit).Namespace;
    //    }

    //    public static bool IsUnit<TCurrency>(this TCurrency @this) where TCurrency : Currency
    //    {
    //        Require.NotNull(@this, nameof(@this));

    //        return @this is CurrencyUnit && @this.GetType().Namespace == typeof(CurrencyUnit).Namespace;
    //    }

    //    public static string GetCode<TCurrency>() where TCurrency : CurrencyUnit<TCurrency>
    //        => typeof(TCurrency).Name;

    //    public static string GetCode<TCurrency>(this CurrencyUnit<TCurrency> @this)
    //        where TCurrency : CurrencyUnit<TCurrency>
    //        => typeof(TCurrency).Name;
    //}

    public abstract class CurrencyUnit
    {
        internal CurrencyUnit() { }

        public abstract string Code { get; }

        public override string ToString() => Code;
    }

    public class CurrencyUnit<TCurrency> : CurrencyUnit where TCurrency : CurrencyUnit<TCurrency>
    {
        protected CurrencyUnit() { }

        internal static string CoderIntern => typeof(TCurrency).Name;

        public Currency ToCurrency() => Currency.Of(Code);

        public override string Code => CoderIntern;
    }

    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
    public sealed class ZZZ : CurrencyUnit<ZZZ>
    {
        private ZZZ() { }

        public static ZZZ Unit => Uniq.Instance;

        public static explicit operator ZZZ(Currency value) => FromCurrency(value);
        public static explicit operator Currency(ZZZ value) => value?.ToCurrency();

        //public Currency ToCurrency() => Currency.Of(Code);

        public static ZZZ FromCurrency(Currency value)
        {
            if (value == null) { return null; }
            if (value.Code != CoderIntern) { throw new InvalidCastException(); }

            return Unit;
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
        private class Uniq
        {
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static Uniq() { }
            internal static readonly ZZZ Instance = new ZZZ();
        }
    }

    internal static class CurrencyUnitActivator<TCurrency> where TCurrency : CurrencyUnit<TCurrency>
    {
        public static TCurrency CreateInstance()
        {
            //var currency = Currency.Of(typeof(TCurrency).Name);

            throw new NotImplementedException();
        }
    }
}
