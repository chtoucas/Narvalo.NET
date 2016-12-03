// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using static System.Diagnostics.Contracts.Contract;

    public static class BinaryEncoder
    {
        #region Hexadecimal

        // Cf. http://stackoverflow.com/questions/623104/c-sharp-byte-to-hex-string/623184
        public static string ToHexString(byte[] value)
        {
            Require.NotNull(value, nameof(value));
            Ensures(Result<string>() != null);

            return BitConverter.ToString(value).Replace("-", String.Empty);
        }

        public static byte[] FromHexString(string value)
        {
            Require.NotNull(value, nameof(value));
            Ensures(Result<byte[]>() != null);

            if (value.Length == 0 || value.Length % 2 != 0)
            {
                return new byte[0];
            }

            int length = value.Length / 2;
            byte[] bytes = new byte[length];

            for (int i = 0; i < length; i++)
            {
                bytes[i] = Convert.ToByte(value.Substring(2 * i, 2), 16);
            }

            return bytes;
        }

        #endregion
    }
}
