// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
#if DEBUG

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using Narvalo.Globalization;

    // NB: SNV = "Association Suisse de Normalisation"
    // Only used internally for testing the XML sources. No need to bother too much with this.
    internal sealed class SnvCurrencyProvider : CurrencyProviderBase
    {
        const string NoMinorUnits_ = "N.A.";

        readonly string _source;
        readonly string _legacySource;

        HashSet<string> _codeSet;

        public SnvCurrencyProvider(string source, string legacySource)
        {
            Require.NotNull(source, "source");
            Require.NotNull(legacySource, "legacySource");

            _source = source;
            _legacySource = legacySource;
        }

        public override HashSet<string> CurrencyCodes
        {
            get
            {
                if (_codeSet == null) {
                    var q = (from item in GetCurrencies(CurrencyTypes.AllCurrencies)
                             select item.Code).Distinct();

                    _codeSet = new HashSet<string>(q);
                }

                return _codeSet;
            }
        }

        public override IEnumerable<CurrencyInfo> GetCurrencies(CurrencyTypes types)
        {
            var settings = new XmlReaderSettings {
                CheckCharacters = false,
                CloseInput = true,
                DtdProcessing = DtdProcessing.Ignore,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true,
                ValidationFlags = XmlSchemaValidationFlags.None,
                ValidationType = ValidationType.None,
            };

            IEnumerable<CurrencyInfo> current;
            if (types.HasFlag(CurrencyTypes.CurrentCurrencies)) {
                using (var reader = XmlReader.Create(_source, settings)) {
                    var root = XElement.Load(reader, LoadOptions.None);
                    current = Parse_(root);
                }
            }
            else {
                current = Enumerable.Empty<CurrencyInfo>();
            }

            IEnumerable<CurrencyInfo> legacy;
            if (types.HasFlag(CurrencyTypes.LegacyCurrencies)) {
                using (var reader = XmlReader.Create(_legacySource, settings)) {
                    var root = XElement.Load(reader, LoadOptions.None);
                    legacy = ParseLegacy_(root);
                }
            }
            else {
                legacy = Enumerable.Empty<CurrencyInfo>();
            }

            return current.Concat(legacy);
        }

        static IEnumerable<CurrencyInfo> Parse_(XElement root)
        {
            //var pubDate = root.Attribute("Pblshd").Value;

            var list = root.Element("CcyTbl").Elements("CcyNtry");

            foreach (var item in list) {
                // Currency Alphabetic Code
                var codeElement = item.Element("Ccy");
                if (codeElement == null) {
                    Debug.WriteLine("Found a country without universal currency: " + item.Element("CtryNm").Value);

                    continue;
                }
                var code = codeElement.Value;

                Debug.Assert(code.Length == 3, "The alphabetic code MUST be composed of exactly 3 characters.");

                // Currency Numeric Code
                // NB: Int16.Parse should never fail.
                var numericCode = Int16.Parse(item.Element("CcyNbr").Value);

                Debug.Assert(numericCode > 0, "The numeric code MUST be strictly greater than 0.");
                Debug.Assert(numericCode < 1000, "The numeric code MUST be strictly less than 1000.");

                // Currency English Name
                var englishNameElement = item.Element("CcyNm");
                var englishName = englishNameElement.Value.Replace("\"", "\"\"");

                // Fund Currency
                bool isFund = false;
                var isFundAttr = englishNameElement.Attribute("IsFund");
                if (isFundAttr != null) {
                    Debug.Assert(isFundAttr.Value == "true", "When present, the 'IsFund' attribute value is expected to be 'true'.");

                    isFund = isFundAttr.Value == "true";
                }

                // Country English Name
                var englishRegionName = item.Element("CtryNm").Value
                    .Replace("’", "'")
                    .Replace("\"", "\"\"")
                    .Replace("\n", "");

                // Minor Units
                var minorUnitsValue = item.Element("CcyMnrUnts").Value;
                short? minorUnits = null;
                if (minorUnitsValue != NoMinorUnits_) {
                    // NB: Int16.Parse should never fail.
                    minorUnits = Int16.Parse(minorUnitsValue);
                }

                yield return CreateCurrency(code, numericCode, minorUnits, englishName, englishRegionName, isFund);
            }
        }

        static IEnumerable<CurrencyInfo> ParseLegacy_(XElement root)
        {
            var list = root.Element("HstrcCcyTbl").Elements("HstrcCcyNtry");

            foreach (var item in list) {
                // Currency Alphabetic Code
                var code = item.Element("Ccy").Value;

                Debug.Assert(code.Length == 3, "The alphabetic code MUST be composed of exactly 3 characters.");

                // Currency Numeric Code
                // NB: Int16.Parse should never fail.
                var numericCodeElement = item.Element("CcyNbr");
                short numericCode;
                if (numericCodeElement == null) {
                    Debug.WriteLine("Found a legacy currency without a numeric code: " + item.Element("CtryNm").Value);

                    numericCode = 0;
                }
                else {
                    numericCode = Int16.Parse(numericCodeElement.Value);
                }

                Debug.Assert(numericCode > 0, "The numeric code MUST be strictly greater than 0.");
                Debug.Assert(numericCode < 1000, "The numeric code MUST be strictly less than 1000.");

                // Currency English Name
                var englishNameElement = item.Element("CcyNm");
                var englishName = englishNameElement.Value.Replace("\"", "\"\"");

                // Fund Currency
                bool isFund = false;
                var isFundAttr = englishNameElement.Attribute("IsFund");
                if (isFundAttr != null) {
                    Debug.Assert(isFundAttr.Value == "true" || isFundAttr.Value == " ", "When present, the 'IsFund' attribute value is expected to be 'true'.");

                    // NB: Empty value is interpreted to be the same as no attibrute.
                    isFund = isFundAttr.Value == "true";
                }

                // Country English Name
                var englishRegionName = item.Element("CtryNm").Value
                    .Replace("’", "'")
                    .Replace("\"", "\"\"")
                    .Replace("\n", "");

                yield return CreateLegacyCurrency(code, numericCode, englishName, englishRegionName, isFund);
            }
        }
    }

#endif
}
