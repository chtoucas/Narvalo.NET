// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Text;

    // TODO: Créer la version Hexavigesimal.
    [ContractVerification(false)] // TODO: Enable ContractVerification for BinaryEncoder.
    public static class BinaryEncoder
    {
        // Alternative alphabet: "abcdefghijklmnopqrstuvwxyz234567";
        private static string s_ZBase32Alphabet = "ybndrfg8ejkmcpqxot1uwisza345h769";

        #region Hexadecimal

        // Cf. http://stackoverflow.com/questions/623104/c-sharp-byte-to-hex-string/623184
        public static string ToHexString(byte[] value)
        {
            Require.NotNull<byte[]>(value, "value");
            Contract.Ensures(Contract.Result<string>() != null);

            return BitConverter.ToString(value).Replace("-", String.Empty);
        }

        public static byte[] FromHexString(string value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<byte[]>() != null);

            // FIXME: FormatException quand value.Length est impair ?
            byte[] retval = new byte[value.Length / 2];

            for (int i = 0; i < retval.Length; i++)
            {
                retval[i] = Convert.ToByte(value.Substring(2 * i, 2), 16);
            }

            return retval;
        }

        #endregion

        #region Z-Base32

        public static string ToZBase32String(byte[] value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<string>() != null);

            int length = value.Length;
            var sb = new StringBuilder((length + 7) * 8 / 5);

            int i = 0;
            int index = 0;
            int digit = 0;
            int currByte;
            int nextByte;

            while (i < length)
            {
                currByte = (value[i] >= 0) ? value[i] : (value[i] + 256); // unsign

                // Is the current digit going to span a byte boundary?
                if (index > 3)
                {
                    if ((i + 1) < length)
                    {
                        nextByte = (value[i + 1] >= 0) ? value[i + 1] : (value[i + 1] + 256);
                    }
                    else
                    {
                        nextByte = 0;
                    }

                    digit = currByte & (0xFF >> index);
                    index = (index + 5) % 8;
                    digit <<= index;
                    digit |= nextByte >> (8 - index);
                    i++;
                }
                else
                {
                    digit = (currByte >> (8 - (index + 5))) & 0x1F;
                    index = (index + 5) % 8;
                    if (index == 0)
                    {
                        i++;
                    }
                }

                sb.Append(s_ZBase32Alphabet[digit]);
            }

            return sb.ToString();
        }

        public static byte[] FromZBase32String(string value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<byte[]>() != null);

            unchecked
            {
                int length = value.Length;
                int resultLength = length * 5 / 8;
                byte[] retval = new byte[resultLength];

                int index = 0;
                int digit = 0;
                int offset = 0;

                for (int i = 0; i < length; i++)
                {
                    digit = s_ZBase32Alphabet.IndexOf(value[i]);

                    if (index <= 3)
                    {
                        index = (index + 5) % 8;
                        if (index == 0)
                        {
                            retval[offset] |= (byte)digit;
                            offset++;
                            if (offset >= resultLength)
                            {
                                break;
                            }
                        }
                        else
                        {
                            retval[offset] |= (byte)(digit << (8 - index));
                        }
                    }
                    else
                    {
                        index = (index + 5) % 8;
                        retval[offset] |= (byte)(digit >> index);
                        offset++;
                        if (offset >= resultLength)
                        {
                            break;
                        }

                        retval[offset] |= (byte)(digit << (8 - index));
                    }
                }

                return retval;
            }
        }

        #endregion
    }
}
