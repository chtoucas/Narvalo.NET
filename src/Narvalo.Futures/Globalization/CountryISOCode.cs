// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // ISO 3166-1
    public partial class CountryISOCode
    {
        private string _threeLetterCode;
        private string _twoLetterCode;
        private string _numericCode;

        public CountryISOCode(string twoLetterCode, string numericCode)
        {
            Require.NotNullOrEmpty(twoLetterCode, nameof(twoLetterCode));
            Require.NotNullOrEmpty(numericCode, nameof(numericCode));

            _twoLetterCode = twoLetterCode;
            _numericCode = numericCode;
        }

        // Alpha-2 code defined in ISO 3166-1 alpha-2.
        public string TwoLetterCode
        {
            get { Warrant.NotNull<string>(); return _twoLetterCode; }
        }

        // Alpha-3 code defined in ISO 3166-2 alpha-3.
        public string ThreeLetterCode
        {
            get
            {
                Warrant.NotNull<string>();
                if (_threeLetterCode == null)
                {
                    _threeLetterCode = FindThreeLetterCode(TwoLetterCode);
                }

                return _threeLetterCode;
            }

            internal set { _threeLetterCode = value; }
        }

        // Numeric code defined in ISO 3166-1 numeric.
        public string NumericCode
        {
            get { Warrant.NotNull<string>(); return _numericCode; }
        }

        public bool IsUserAssigned
            => TwoLetterCode == "AA"
            || TwoLetterCode == "ZZ"
            || TwoLetterCode[0] == 'X'
            || (TwoLetterCode[0] == 'Q' && TwoLetterCode[1] >= 'M');

        public static CountryISOCode Of(string twoLetterCode)
        {
            Expect.NotNull(twoLetterCode);

            string numericCode;
            if (!Alpha2ToNumeric.TryGetValue(twoLetterCode, out numericCode))
            {
                throw new Exception(Format.Current("Unknown country code {0}.", twoLetterCode));
            }

            return new CountryISOCode(twoLetterCode, numericCode);
        }
    }

    public partial class CountryISOCode
    {
        private static volatile Dictionary<string, string> s_Alpha2ToAlpha3;
        private static volatile Dictionary<string, string> s_Alpha2ToNumeric;

        private static readonly Lazy<ILookup<string, string>> s_NumericToAlpha2
            = new Lazy<ILookup<string, string>>(InitializeNumericToAlpha2);

        private static readonly Lazy<ILookup<string, string>> s_Alpha3ToAlpha2
            = new Lazy<ILookup<string, string>>(InitializeAlpha3ToAlpha2);

        private static Dictionary<string, string> Alpha2ToAlpha3
        {
            get
            {
                if (s_Alpha2ToAlpha3 == null)
                {
                    var dict = new Dictionary<string, string>
                    {
                    };

                    s_Alpha2ToAlpha3 = dict;
                }

                return s_Alpha2ToAlpha3;
            }
        }

        private static Dictionary<string, string> Alpha2ToNumeric
        {
            get
            {
                if (s_Alpha2ToNumeric == null)
                {
                    var dict = new Dictionary<string, string> {
        #region ISO data in english alphabetic order.
                {"AF", "004"},
                {"AX", "248"},
                {"AL", "008"},
                {"DZ", "012"},
                {"AS", "016"},
                {"AD", "020"},
                {"AO", "024"},
                {"AI", "660"},
                {"AQ", "010"},
                {"AG", "028"},
                {"AR", "032"},
                {"AM", "051"},
                {"AW", "533"},
                {"AU", "036"},
                {"AT", "040"},
                {"AZ", "031"},
                {"BS", "044"},
                {"BH", "048"},
                {"BD", "050"},
                {"BB", "052"},
                {"BY", "112"},
                {"BE", "056"},
                {"BZ", "084"},
                {"BJ", "204"},
                {"BM", "060"},
                {"BT", "064"},
                {"BO", "068"},
                {"BQ", "535"},
                {"BA", "070"},
                {"BW", "072"},
                {"BV", "074"},
                {"BR", "076"},
                {"IO", "086"},
                {"BN", "096"},
                {"BG", "100"},
                {"BF", "854"},
                {"BI", "108"},
                {"KH", "116"},
                {"CM", "120"},
                {"CA", "124"},
                {"CV", "132"},
                {"KY", "136"},
                {"CF", "140"},
                {"TD", "148"},
                {"CL", "152"},
                {"CN", "156"},
                {"CX", "162"},
                {"CC", "166"},
                {"CO", "170"},
                {"KM", "174"},
                {"CG", "178"},
                {"CD", "180"},
                {"CK", "184"},
                {"CR", "188"},
                {"CI", "384"},
                {"HR", "191"},
                {"CU", "192"},
                {"CW", "531"},
                {"CY", "196"},
                {"CZ", "203"},
                {"DK", "208"},
                {"DJ", "262"},
                {"DM", "212"},
                {"DO", "214"},
                {"EC", "218"},
                {"EG", "818"},
                {"SV", "222"},
                {"GQ", "226"},
                {"ER", "232"},
                {"EE", "233"},
                {"ET", "231"},
                {"FK", "238"},
                {"FO", "234"},
                {"FJ", "242"},
                {"FI", "246"},
                {"FR", "250"},
                {"GF", "254"},
                {"PF", "258"},
                {"TF", "260"},
                {"GA", "266"},
                {"GM", "270"},
                {"GE", "268"},
                {"DE", "276"},
                {"GH", "288"},
                {"GI", "292"},
                {"GR", "300"},
                {"GL", "304"},
                {"GD", "308"},
                {"GP", "312"},
                {"GU", "316"},
                {"GT", "320"},
                {"GG", "831"},
                {"GN", "324"},
                {"GW", "624"},
                {"GY", "328"},
                {"HT", "332"},
                {"HM", "334"},
                {"VA", "336"},
                {"HN", "340"},
                {"HK", "344"},
                {"HU", "348"},
                {"IS", "352"},
                {"IN", "356"},
                {"ID", "360"},
                {"IR", "364"},
                {"IQ", "368"},
                {"IE", "372"},
                {"IM", "833"},
                {"IL", "376"},
                {"IT", "380"},
                {"JM", "388"},
                {"JP", "392"},
                {"JE", "832"},
                {"JO", "400"},
                {"KZ", "398"},
                {"KE", "404"},
                {"KI", "296"},
                {"KP", "408"},
                {"KR", "410"},
                {"KW", "414"},
                {"KG", "417"},
                {"LA", "418"},
                {"LV", "428"},
                {"LB", "422"},
                {"LS", "426"},
                {"LR", "430"},
                {"LY", "434"},
                {"LI", "438"},
                {"LT", "440"},
                {"LU", "442"},
                {"MO", "446"},
                {"MK", "807"},
                {"MG", "450"},
                {"MW", "454"},
                {"MY", "458"},
                {"MV", "462"},
                {"ML", "466"},
                {"MT", "470"},
                {"MH", "584"},
                {"MQ", "474"},
                {"MR", "478"},
                {"MU", "480"},
                {"YT", "175"},
                {"MX", "484"},
                {"FM", "583"},
                {"MD", "498"},
                {"MC", "492"},
                {"MN", "496"},
                {"ME", "499"},
                {"MS", "500"},
                {"MA", "504"},
                {"MZ", "508"},
                {"MM", "104"},
                {"NA", "516"},
                {"NR", "520"},
                {"NP", "524"},
                {"NL", "528"},
                {"NC", "540"},
                {"NZ", "554"},
                {"NI", "558"},
                {"NE", "562"},
                {"NG", "566"},
                {"NU", "570"},
                {"NF", "574"},
                {"MP", "580"},
                {"NO", "578"},
                {"OM", "512"},
                {"PK", "586"},
                {"PW", "585"},
                {"PS", "275"},
                {"PA", "591"},
                {"PG", "598"},
                {"PY", "600"},
                {"PE", "604"},
                {"PH", "608"},
                {"PN", "612"},
                {"PL", "616"},
                {"PT", "620"},
                {"PR", "630"},
                {"QA", "634"},
                {"RE", "638"},
                {"RO", "642"},
                {"RU", "643"},
                {"RW", "646"},
                {"BL", "652"},
                {"SH", "654"},
                {"KN", "659"},
                {"LC", "662"},
                {"MF", "663"},
                {"PM", "666"},
                {"VC", "670"},
                {"WS", "882"},
                {"SM", "674"},
                {"ST", "678"},
                {"SA", "682"},
                {"SN", "686"},
                {"RS", "688"},
                {"SC", "690"},
                {"SL", "694"},
                {"SG", "702"},
                {"SX", "534"},
                {"SK", "703"},
                {"SI", "705"},
                {"SB", "090"},
                {"SO", "706"},
                {"ZA", "710"},
                {"GS", "239"},
                {"SS", "728"},
                {"ES", "724"},
                {"LK", "144"},
                {"SD", "729"},
                {"SR", "740"},
                {"SJ", "744"},
                {"SZ", "748"},
                {"SE", "752"},
                {"CH", "756"},
                {"SY", "760"},
                {"TW", "158"},
                {"TJ", "762"},
                {"TZ", "834"},
                {"TH", "764"},
                {"TL", "626"},
                {"TG", "768"},
                {"TK", "772"},
                {"TO", "776"},
                {"TT", "780"},
                {"TN", "788"},
                {"TR", "792"},
                {"TM", "795"},
                {"TC", "796"},
                {"TV", "798"},
                {"UG", "800"},
                {"UA", "804"},
                {"AE", "784"},
                {"GB", "826"},
                {"US", "840"},
                {"UM", "581"},
                {"UY", "858"},
                {"UZ", "860"},
                {"VU", "548"},
                {"VE", "862"},
                {"VN", "704"},
                {"VG", "092"},
                {"VI", "850"},
                {"WF", "876"},
                {"EH", "732"},
                {"YE", "887"},
                {"ZM", "894"},
                {"ZW", "716"}
                #endregion
                    };

                    s_Alpha2ToNumeric = dict;
                }

                return s_Alpha2ToNumeric;
            }
        }

        public static string FindThreeLetterCode(string twoLetterCode)
        {
            Expect.NotNull(twoLetterCode);
            Warrant.NotNull<string>();

            return Alpha2ToAlpha3[twoLetterCode];
        }

        public static string FindNumericCode(string twoLetterCode)
        {
            Expect.NotNull(twoLetterCode);
            Warrant.NotNull<string>();

            return Alpha2ToNumeric[twoLetterCode];
        }

        public static string FindTwoLetterCode(string numericCode)
        {
            // Numeric codes that have been withdrawn from ISO 3166.
            // See the "Withdrawn codes" section at http://en.wikipedia.org/wiki/ISO_3166-1_numeric.
            //switch (numericCode)
            //{
            //    case 230: // Ethiopia (before Eritrea split away in 1993).
            //    case 532: // Netherlands Antilles (before Aruba split away in 1986).
            //    case 590: // Panama (before adding Panama Canal Zone in 1979).
            //    case 886: // Yemen Arab Republic (i.e., North Yemen).
            //        return null;
            //}

            var lookup = s_NumericToAlpha2.Value;
            var seq = lookup[numericCode].ToList();

            if (seq.Count == 0)
            {
                throw new KeyNotFoundException("ISO Code not found: " + numericCode);
            }
            else if (seq.Count > 1)
            {
                throw new KeyNotFoundException("More than one ISO Code found: " + numericCode);
            }
            else
            {
                return seq[0];
            }
        }

        // FIXME: Use .ToDictionary(_ => _.Value, _ => _.Key);
        // since key and values are unique.
        private static ILookup<string, string> InitializeNumericToAlpha2()
            => Alpha2ToNumeric.ToLookup(_ => _.Value, _ => _.Key);

        private static ILookup<string, string> InitializeAlpha3ToAlpha2()
            => s_Alpha2ToAlpha3.ToLookup(_ => _.Value, _ => _.Key);
    }
}
