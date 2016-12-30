// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using Narvalo.Finance.Utilities;

    public sealed class IbanBuilder
    {
        public string Bban { get; set; }

        public string CountryCode { get; set; }

        public Iban Build()
        {
            var checkDigits = IbanCheckDigits.Compute(CountryCode, Bban, IntPtr.Size == 8);

            return Iban.Create(CountryCode, checkDigits, Bban, IbanValidationLevels.None);
        }
    }
}
