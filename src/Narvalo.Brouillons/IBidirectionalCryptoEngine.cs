// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
#if CONTRACTS_FULL // [Intentionally] Using directive.
    using System.Diagnostics.Contracts;
#endif

    public partial interface IBidirectionalCryptoEngine
    {
        string Encrypt(string value);

        string Decrypt(string value);
    }

#if CONTRACTS_FULL // [Ignore] Contract Class and Object Invariants.

    [ContractClass(typeof(ContractForIBidirectionalCryptoEngine))]
    public partial interface IBidirectionalCryptoEngine { }

    [ContractClassFor(typeof(IBidirectionalCryptoEngine))]
    internal abstract class ContractForIBidirectionalCryptoEngine : IBidirectionalCryptoEngine
    {
        string IBidirectionalCryptoEngine.Encrypt(string value)
        {
            Contract.Requires(value != null);

            return default(String);
        }

        string IBidirectionalCryptoEngine.Decrypt(string value)
        {
            Contract.Requires(value != null);

            return default(String);
        }
    }

#endif
}
