// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    public partial interface IBidirectionalCryptoEngine
    {
        string Encrypt(string value);

        string Decrypt(string value);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IBidirectionalCryptoEngineContract))]
    public partial interface IBidirectionalCryptoEngine { }

    [ContractClassFor(typeof(IBidirectionalCryptoEngine))]
    internal abstract class IBidirectionalCryptoEngineContract : IBidirectionalCryptoEngine
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
}

#endif
