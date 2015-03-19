// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Security
{
    using System;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif

    public partial interface IBidirectionalCryptoEngine
    {
        string Encrypt(string val);

        string Decrypt(string val);
    }

#if CONTRACTS_FULL

    [ContractClass(typeof(ContractForIBidirectionalCryptoEngine))]
    public partial interface IBidirectionalCryptoEngine { }

    [ContractClassFor(typeof(IBidirectionalCryptoEngine))]
    internal abstract class ContractForIBidirectionalCryptoEngine : IBidirectionalCryptoEngine
    {
        string IBidirectionalCryptoEngine.Encrypt(string val)
        {
            Contract.Requires<ArgumentNullException>(val != null);
            return default(String);
        }

        string IBidirectionalCryptoEngine.Decrypt(string val)
        {
            Contract.Requires<ArgumentNullException>(val != null);
            return default(String);
        }
    }

#endif
}
