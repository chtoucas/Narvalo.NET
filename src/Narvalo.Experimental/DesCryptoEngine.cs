namespace Narvalo
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    //[ContractVerification(true)]
    public class DesCryptoEngine : IBidirectionalCryptoEngine
    {
        private readonly static TripleDESCryptoServiceProvider Provider
            = new TripleDESCryptoServiceProvider();
        private readonly static UTF8Encoding Encoding = new UTF8Encoding();

        private readonly byte[] _key;
        private readonly byte[] _iv;

        public DesCryptoEngine(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;
        }

        //[SuppressMessage("Microsoft.Usage", "CA2202:Ne pas supprimer d'objets plusieurs fois",
        //    Justification = "Multiple calls to Dispose is certainly implemented by a framework class.")]
        public string Encrypt(string val)
        {
            if (val == String.Empty) {
                return String.Empty;
            }

            byte[] input = Encoding.GetBytes(val);
            byte[] result;
            MemoryStream mStream = null;

            var encryptor = Provider.CreateEncryptor(_key, _iv);

            try {
                mStream = new MemoryStream();

                using (CryptoStream cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write)) {
                    // Write the byte array to the crypto stream and flush it.
                    cStream.Write(input, 0, input.Length);
                    cStream.FlushFinalBlock();

                    // Get an array of bytes from the MemoryStream that holds the encrypted data.
                    result = mStream.ToArray();
                }
            }
            finally {
                if (mStream != null) {
                    mStream.Close();
                    mStream = null;
                }
            }

            return Convert.ToBase64String(result);
        }

        //[SuppressMessage("Microsoft.Usage", "CA2202:Ne pas supprimer d'objets plusieurs fois",
        //    Justification = "Multiple calls to Dispose is certainly implemented by a framework class.")]
        public string Decrypt(string val)
        {
            if (val == String.Empty) {
                return String.Empty;
            }

            byte[] input = Convert.FromBase64String(val);
            byte[] result;
            MemoryStream mStream = null;

            var decryptor = Provider.CreateDecryptor(_key, _iv);

            try {
                mStream = new MemoryStream(input);

                using (CryptoStream cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read)) {
                    // Create buffer to hold the decrypted data.
                    result = new byte[input.Length];

                    // Read the decrypted data out of the crypto stream
                    // and place it into the temporary buffer.
                    cStream.Read(result, 0, result.Length);
                }
            }
            finally {
                if (mStream != null) {
                    mStream.Close();
                    mStream = null;
                }
            }

            // See http://www.codeproject.com/KB/security/encryption_decryption.aspx
            // Trim the '\0' bytes.
            int i = 0;
            for (i = 0; i < result.Length; i++) {
                if (result[i] == 0) {
                    break;
                }
            }

            return Encoding.GetString(result, 0, i);
        }
    }
}
