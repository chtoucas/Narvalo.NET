namespace Narvalo
{
    using System;
    using System.Text;

    // TODO: Hexavigesimal
    public static class BinaryEncoder
    {
        //static string Base32Alphabet =  "abcdefghijklmnopqrstuvwxyz234567";
        static string ZBase32Alphabet = "ybndrfg8ejkmcpqxot1uwisza345h769";

        #region Hexadecimal

        // Cf. http://stackoverflow.com/questions/623104/c-sharp-byte-to-hex-string/623184
        public static string ToHexString(byte[] value)
        {
            Requires.NotNull<byte[]>(value, "value");

            return BitConverter.ToString(value).Replace("-", String.Empty);
        }

        public static byte[] FromHexString(string value)
        {
            Requires.NotNull(value, "value");

            // FIXME: FormatException quand value.Length est impair ?
            byte[] result = new byte[value.Length / 2];
            for (int i = 0; i < result.Length; i++) {
                result[i] = Convert.ToByte(value.Substring(2 * i, 2), 16);
            }
            return result;
        }

        #endregion

        #region Z-Base32

        public static string ToZBase32String(byte[] value)
        {
            Requires.NotNull<byte[]>(value, "value");

            int length = value.Length;
            var sb = new StringBuilder((length + 7) * 8 / 5);

            int i = 0;
            int index = 0;
            int digit = 0;
            int currByte;
            int nextByte;

            while (i < length) {
                currByte = (value[i] >= 0) ? value[i] : (value[i] + 256); // unsign

                // Is the current digit going to span a byte boundary?
                if (index > 3) {
                    if ((i + 1) < length) {
                        nextByte = (value[i + 1] >= 0) ? value[i + 1] : (value[i + 1] + 256);
                    }
                    else {
                        nextByte = 0;
                    }

                    digit = currByte & (0xFF >> index);
                    index = (index + 5) % 8;
                    digit <<= index;
                    digit |= nextByte >> (8 - index);
                    i++;
                }
                else {
                    digit = (currByte >> (8 - (index + 5))) & 0x1F;
                    index = (index + 5) % 8;
                    if (index == 0) {
                        i++;
                    }
                }
                sb.Append(ZBase32Alphabet[digit]);
            }

            return sb.ToString();
        }

        public static byte[] FromZBase32String(string value)
        {
            Requires.NotNull(value, "value");

            unchecked {
                int length = value.Length;
                int resultLength = length * 5 / 8;
                byte[] result = new byte[resultLength];

                int index = 0;
                int digit = 0;
                int offset = 0;

                for (int i = 0; i < length; i++) {
                    digit = ZBase32Alphabet.IndexOf(value[i]);

                    if (index <= 3) {
                        index = (index + 5) % 8;
                        if (index == 0) {
                            result[offset] |= (byte)digit;
                            offset++;
                            if (offset >= resultLength) {
                                break;
                            }
                        }
                        else {
                            result[offset] |= (byte)(digit << (8 - index));
                        }
                    }
                    else {
                        index = (index + 5) % 8;
                        result[offset] |= (byte)(digit >> index);
                        offset++;
                        if (offset >= resultLength) {
                            break;
                        }
                        result[offset] |= (byte)(digit << (8 - index));
                    }
                }

                return result;
            }
        }

        #endregion
    }
}
