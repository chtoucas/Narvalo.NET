namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Text;

    public static class RandomString
    {
        const string Alphabet_ = "abcdefghijklmnopqrstuvwyxz";

        // Cf. http://stackoverflow.com/questions/976646/is-this-a-good-way-to-generate-a-string-of-random-characters
        public static string GenerateString(int size, Random generator)
        {
            Require.NotNull(generator, "generator");
            Contract.Ensures(Contract.Result<string>() != null);

            var chars = new char[size];

            for (int i = 0; i < size; i++) {
                chars[i] = Alphabet_[generator.Next(Alphabet_.Length)];
            }

            return new String(chars);
        }

        // Cf. http://www.bonf.net/2009/01/14/generating-random-unicode-strings-in-c/
        public static string GenerateUnicodeString(int size, Random generator)
        {
            Require.GreaterThanOrEqualTo(size, 0, "size");
            Require.LessThanOrEqualTo(size, Int32.MaxValue / 2, "size");
            Require.NotNull(generator, "generator");
            Contract.Ensures(Contract.Result<string>() != null);

            checked {
                int length = 2 * size;

                var byteArray = new byte[length];

                for (int i = 0; i < length; i += 2) {
                    int chr = generator.Next(0xD7FF);
                    byteArray[i + 1] = (byte)((chr & 0xFF00) >> 8);
                    byteArray[i] = (byte)(chr & 0xFF);
                }

                return Encoding.Unicode.GetString(byteArray);
            }
        }
    }
}
