// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    /// <summary>
    /// Defines the types of currencies that can be used to create a currency using the
    /// <see cref="Currency.Of(string, CurrencyTypes)"/> and
    /// <see cref="CurrencyFactory.TryCreate(string, CurrencyTypes)"/> methods.
    /// </summary>
    /// <remarks>This enumeration has a <see cref="FlagsAttribute"/> attribute that allows
    /// a bitwise combination of its member values.</remarks>
    [Flags]
    public enum CurrencyTypes
    {
        /// <summary>
        /// All circulating ISO currencies.
        /// </summary>
        Active = 1 << 0,

        /// <summary>
        /// ISO currencies that have been superseded or simply deleted.
        /// </summary>
        Withdrawn = 1 << 1,

        /// <summary>
        /// User-defined currencies (not part of ISO 4217).
        /// <para>These currencies are registered via the
        /// <see cref="Currency.RegisterCurrency(string, short?)"/> and
        /// <see cref="Currency.RegisterCurrencies(System.Collections.Generic.Dictionary{string, short?})"/>
        /// methods.</para>
        /// </summary>
        Custom = 1 << 2,

        /// <summary>
        /// All circulating currencies whether they are ISO or not.
        /// <para><see cref="Current"/> is a composite field that includes the <see cref="Active"/>
        /// and <see cref="Custom"/> fields.</para>
        /// </summary>
        Current = Active | Custom,

        /// <summary>
        /// All ISO currencies.
        /// <para><see cref="Current"/> is a composite field that includes the <see cref="Active"/>
        /// and <see cref="Withdrawn"/> fields.</para>
        /// </summary>
        ISO = Active | Withdrawn,

        /// <summary>
        /// All supported currencies.
        /// <para><see cref="Current"/> is a composite field that includes the <see cref="Active"/>,
        /// <see cref="Withdrawn"/> and <see cref="Custom"/> fields.</para>
        /// </summary>
        Any = Active | Withdrawn | Custom
    }

    public static class CurrencyTypesExtensions
    {
        public static bool Contains(this CurrencyTypes @this, CurrencyTypes values) => (@this & values) != 0;

        public static CurrencyTypes Except(this CurrencyTypes @this, CurrencyTypes values) => (@this & ~values);

        public static CurrencyTypes Toggle(this CurrencyTypes @this, CurrencyTypes values) => @this ^ values;
    }
}
