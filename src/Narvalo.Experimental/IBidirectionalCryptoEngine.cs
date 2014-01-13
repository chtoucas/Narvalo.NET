namespace Narvalo
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
        #region IBidirectionalCryptoEngine

        public string Encrypt(string val)
        {
            Contract.Requires<ArgumentNullException>(val != null);
            return default(String);
        }

        public string Decrypt(string val)
        {
            Contract.Requires<ArgumentNullException>(val != null);
            return default(String);
        }

        #endregion
    }
}
