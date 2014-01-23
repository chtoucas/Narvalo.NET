namespace Narvalo
{
    using System;
    using System.Text;

    public static class RandomString
    {
        const string Alphabet_ = "abcdefghijklmnopqrstuvwyxz";

        [Alien(AlienSource.Informal,
            Link = "http://stackoverflow.com/questions/976646/is-this-a-good-way-to-generate-a-string-of-random-characters")]
        public static string GenerateString(int size, Random rng)
        {
            var chars = new char[size];

            for (int i = 0; i < size; i++) {
                chars[i] = Alphabet_[rng.Next(Alphabet_.Length)];
            }

            return new String(chars);
        }

        [Alien(AlienSource.Informal,
            Link = "http://www.bonf.net/2009/01/14/generating-random-unicode-strings-in-c/")]
        public static string GenerateUnicodeString(int size, Random rng)
        {
            Require.GreaterThanOrEqualTo(size, 0, "size");
            Require.LessThanOrEqualTo(size, Int32.MaxValue / 2, "size");

            checked {
                int length = 2 * size;

                var byteArray = new byte[length];

                for (int i = 0; i < length; i += 2) {
                    int chr = rng.Next(0xD7FF);
                    byteArray[i + 1] = (byte)((chr & 0xFF00) >> 8);
                    byteArray[i] = (byte)(chr & 0xFF);
                }

                return Encoding.Unicode.GetString(byteArray);
            }
        }
    }
}
