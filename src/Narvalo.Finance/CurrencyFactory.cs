// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;

    public abstract class CurrencyFactory
    {
        protected CurrencyFactory() { }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the specified alphabetic code.
        /// </summary>
        /// <remarks>
        /// Contrary to the <see cref="Currency.Of"/> method, this method always return a fresh object.
        /// </remarks>
        /// <param name="code">The three letter ISO-4217 code of the currency.</param>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the
        /// specified code.</exception>
        /// <returns>The currency for the specified code.</returns>
        public Currency GetCurrency(string code)
        {
            ContractFor.CurrencyCode(code);
            Contract.Ensures(Contract.Result<Currency>() != null);

            if (!IsValid(code))
            {
                throw new CurrencyNotFoundException("Unknown currency: " + code + ".");
            }

            return new Currency(code);
        }

        protected abstract bool IsValid(string code);
    }
}
