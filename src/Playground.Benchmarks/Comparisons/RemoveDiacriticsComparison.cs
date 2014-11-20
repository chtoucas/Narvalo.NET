namespace Playground.Benchmarks.Comparisons
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using Narvalo.Benchmarking;

    [BenchmarkComparison(10000, DisplayName = "Suppression des diacritiques.")]
    public class RemoveDiacriticsComparison
    {
        // \p{Mn} or \p{Non_Spacing_Mark}:
        //   a character intended to be combined with another 
        //   character without taking up extra space 
        //   (e.g. accents, umlauts, etc.). 
        static readonly Regex _NonSpacingMarkRegex = new Regex(@"\p{Mn}", RegexOptions.Compiled);

        ////IEnumerable<string> GenerateTestData()
        ////{
        ////    int[] lengths = new int[] { 100, 1000, };

        ////    return from l in lengths
        ////           select StringGenerator.RandomUnicodeString(l);
        ////}

        [BenchmarkComparative(DisplayName = "Expression rationnelle.")]
        public string Regex(string value)
        {
            var formD = value.Normalize(NormalizationForm.FormD);

            if (formD != null) {
                return _NonSpacingMarkRegex.Replace(formD, String.Empty);
            }
            else {
                return String.Empty;
            }
        }

        [BenchmarkComparative(DisplayName = "Caractères traités pas à pas.")]
        public string ForLoop(string value)
        {
            var formD = value.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();

            for (int i = 0; i < formD.Length; i++) {
                var c = formD[i];

                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
