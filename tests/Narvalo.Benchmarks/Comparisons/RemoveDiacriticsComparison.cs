// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Comparisons
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Narvalo.BenchmarkCommon;

    [BenchmarkComparison(100000, DisplayName = "Suppression des diacritiques.")]
    public static class RemoveDiacriticsComparison
    {
        // \p{Mn} or \p{Non_Spacing_Mark}:
        //   a character intended to be combined with another 
        //   character without taking up extra space 
        //   (e.g. accents, umlauts, etc.). 
        private static readonly Regex s_NonSpacingMarkRegex = new Regex(@"\p{Mn}", RegexOptions.Compiled);

        // Cf. http://www.bonf.net/2009/01/14/generating-random-unicode-strings-in-c/
        public static string GenerateUnicodeString(int size, Random generator)
        {
            Require.GreaterThanOrEqualTo(size, 0, "size");
            Require.LessThanOrEqualTo(size, Int32.MaxValue / 2, "size");
            Require.NotNull(generator, "generator");

            checked
            {
                int length = 2 * size;

                var byteArray = new byte[length];

                for (int i = 0; i < length; i += 2)
                {
                    int chr = generator.Next(0xD7FF);
                    byteArray[i] = (byte)(chr & 0xFF);
                    byteArray[i + 1] = (byte)((chr & 0xFF00) >> 8);
                }

                return Encoding.Unicode.GetString(byteArray);
            }
        }

        public static IEnumerable<string> GenerateTestData()
        {
            int[] lengths = new int[] { 10, 100 };

            var rnd = new Random();

            return from l in lengths select GenerateUnicodeString(l, rnd);
        }

        [BenchmarkComparative(DisplayName = "Expression rationnelle.")]
        public static void Regex(string value)
        {
            RegexCore(value);
        }

        [BenchmarkComparative(DisplayName = "Caractères traités pas à pas.")]
        public static void ForLoop(string value)
        {
            ForLoopCore(value);
        }

        private static string RegexCore(string value)
        {
            string formD = value.Normalize(NormalizationForm.FormD);

            if (formD != null)
            {
                return s_NonSpacingMarkRegex.Replace(formD, String.Empty);
            }
            else
            {
                return String.Empty;
            }
        }

        private static string ForLoopCore(string value)
        {
            string formD = value.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();

            for (int i = 0; i < formD.Length; i++)
            {
                char c = formD[i];

                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
