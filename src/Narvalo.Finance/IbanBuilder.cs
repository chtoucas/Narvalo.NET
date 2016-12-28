// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    public sealed class IbanBuilder
    {
        public string Bban { get; set; }

        public string CountryCode { get; set; }

        public Iban Build()
        {
            // TODO: Compute check digits then return Iban.Create().
            throw new NotImplementedException();
        }
    }
}
