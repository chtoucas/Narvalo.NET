// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Text;

    using static System.Diagnostics.Contracts.Contract;

    // TODO: Créer la version Hexavigesimal.
    public static class BinaryEncoder
    {
        private static string s_ZBase32Alphabet = "ybndrfg8ejkmcpqxot1uwisza345h769";

        // Original alphabet for the base32 encoding: "abcdefghijklmnopqrstuvwxyz234567";
        private static readonly char[] s_Base32Alphabet = {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '2', '3', '4', '5', '6', '7', '='
        };

        #region Z-Base32

        private const int BASE32_BITS_IN = 8;
        private const int BASE32_BITS_OUT = 5;

        // !!! Temporary !!! TO BE REMOVED, only for study, borrowed from Microsoft
        public static string ToBase32String(byte[] value)
        {
            Require.NotNull(value, nameof(value));
            Ensures(Result<string>() != null);

            int inlength = value.Length;
            int totalbits = checked(inlength * BASE32_BITS_IN);
            int outlength = totalbits / BASE32_BITS_OUT;

            // No need to do checked{} since outlen = totalbits / 5 where numBits is int.
            if (totalbits % 5 != 0)
                ++outlength;

            var sb = new StringBuilder(outlength);

            for (int i = 0; i < outlength; ++i)
            {
                // Starting bit offset from start of byte array.
                // No need to use checked{} here since iChar cannot be bigger than numChars which cannot be
                //  bigger than (max int / 5)
                int start = 5 * i;

                // Index into encoding table.
                int index = 0;

                for (int j = start; j < start + 5 && j < totalbits; ++j)
                {
                    int iByte = j / 8;
                    int iBitWithinByte = j % 8;

                    if ((value[iByte] & (1 << iBitWithinByte)) != 0)
                        index += (1 << (j - start));
                }

                sb.Append(s_Base32Alphabet[index]);
            }

            return sb.ToString();
        }

        // TODO: I borrowed this somewhere, need a reference.
        public static string ToZBase32String(byte[] value)
        {
            Require.NotNull(value, nameof(value));
            Ensures(Result<string>() != null);

            int length = value.Length;
            var sb = new StringBuilder((length + 7) * 8 / 5);

            int i = 0;
            int index = 0;
            int digit;

            while (i < length)
            {
                //Assume(i < value.Length);
                //currByte = value[i] + 256;
                int currByte = value[i] >= 0 ? value[i] : (value[i] + 256); // unsign
                //byte currValue = value[i];
                //currByte = currValue;
                //if (currValue >= 0)
                //{
                //    currByte += 256;
                //}

                // Is the current digit going to span a byte boundary?
                if (index > 3)
                {
                    int nextByte;

                    if (i + 1 < length)
                    {
                        //Assume(i + 1 < value.Length);
                        //nextByte = value[i + 1] + 256;
                        nextByte = value[i + 1] >= 0 ? value[i + 1] : (value[i + 1] + 256);
                        //byte nextValue = value[i + 1];
                        //nextByte = nextValue;
                        //if (nextByte >= 0)
                        //{
                        //    nextByte += 256;
                        //}
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
                    if (index == 0) { i++; }
                }

                Check.True(i >= 0);
                Assume(digit >= 0);
                Assume(digit < s_ZBase32Alphabet.Length);

                sb.Append(s_ZBase32Alphabet[digit]);
            }

            return sb.ToString();
        }

        public static byte[] FromZBase32String(string value)
        {
            Require.NotNull(value, nameof(value));
            Ensures(Result<byte[]>() != null);

            unchecked
            {
                int length = value.Length;
                int resultLength = length * 5 / 8;
                byte[] bytes = new byte[resultLength];

                int index = 0;
                int offset = 0;

                for (int i = 0; i < length; i++)
                {
                    int digit = s_ZBase32Alphabet.IndexOf(value[i]);

                    if (index <= 3)
                    {
                        index = (index + 5) % 8;
                        if (index == 0)
                        {
                            bytes[offset] |= (byte)digit;
                            offset++;
                            if (offset >= resultLength)
                            {
                                break;
                            }
                        }
                        else
                        {
                            bytes[offset] |= (byte)(digit << (8 - index));
                        }
                    }
                    else
                    {
                        index = (index + 5) % 8;
                        bytes[offset] |= (byte)(digit >> index);
                        offset++;
                        if (offset >= resultLength)
                        {
                            break;
                        }

                        bytes[offset] |= (byte)(digit << (8 - index));
                    }
                }

                return bytes;
            }
        }

        #endregion
    }
}
