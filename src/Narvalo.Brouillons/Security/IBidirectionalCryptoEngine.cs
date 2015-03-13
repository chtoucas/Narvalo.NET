// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Security
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ContractForIBidirectionalCryptoEngine))]
    public interface IBidirectionalCryptoEngine
    {
        string Encrypt(string val);

        string Decrypt(string val);
    }

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
}
