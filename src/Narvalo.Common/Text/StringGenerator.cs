namespace Narvalo.Text
{
    using System;
    using System.Text;

    public static class StringGenerator
    {
        public static string RandomString(int size)
        {
            var rnd = new Random();
            var sb = new StringBuilder();

            for (int i = 0; i < size; i++) {
                char chr = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rnd.NextDouble() + 65)));
                sb.Append(chr);
            }

            return sb.ToString();
        }

        public static string RandomUnicodeString(int size)
        {
            Requires.GreaterThanOrEqualTo(size, 0, "size");
            Requires.LessThanOrEqualTo(size, Int32.MaxValue / 2, "size");

            checked {
                int length = 2 * size;

                var random = new Random();
                var byteArray = new byte[length];

                for (int i = 0; i < length; i += 2) {
                    int chr = random.Next(0xD7FF);
                    byteArray[i + 1] = (byte)((chr & 0xFF00) >> 8);
                    byteArray[i] = (byte)(chr & 0xFF);
                }

                return Encoding.Unicode.GetString(byteArray);
            }
        }
    }
}
