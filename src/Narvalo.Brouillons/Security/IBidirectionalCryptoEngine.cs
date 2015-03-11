// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Security
{
    using System;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif

#if CONTRACTS_FULL
    [ContractClass(typeof(ContractForIBidirectionalCryptoEngine))]
#endif
    public interface IBidirectionalCryptoEngine
    {
        string Encrypt(string val);

        string Decrypt(string val);
    }

#if CONTRACTS_FULL

    [ContractClassFor(typeof(IBidirectionalCryptoEngine))]
    internal abstract class ContractForIBidirectionalCryptoEngine : IBidirectionalCryptoEngine
    {
        public string IBidirectionalCryptoEngine.Encrypt(string val)
        {
            Contract.Requires<ArgumentNullException>(val != null);
            return default(String);
        }

        public string IBidirectionalCryptoEngine.Decrypt(string val)
        {
            Contract.Requires<ArgumentNullException>(val != null);
            return default(String);
        }
    }

#endif
}
