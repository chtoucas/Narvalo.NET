// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    public sealed class SnvCurrencyXmlReader : SnvCurrencyXmlReaderBase
    {
        private readonly string _source;

        public SnvCurrencyXmlReader(string source)
        {
            Require.NotNullOrEmpty(source, "source");

            _source = source;
        }

        public DateTime? PublicationDate { get; private set; }

        public IEnumerable<CurrencyInfo> Read()
        {
            var root = ReadContent(_source);

            PublicationDate = ParseTo.DateTime(root.Attribute("Pblshd").Value).Value;

            var list = root.Element("CcyTbl").Elements("CcyNtry");

            foreach (var item in list) {
                // Currency Alphabetic Code
                var codeElement = item.Element("Ccy");
                if (codeElement == null) {
                    Debug.WriteLine("Found a country without universal currency: " + item.Element("CtryNm").Value);

                    continue;
                }

                var code = codeElement.Value;
                __ValidateCode(code);

                // Currency Numeric Code
                var numericCode = ReadValueAsShort(item.Element("CcyNbr"));
                __ValidateNumericCode(numericCode);

                // Currency English Name
                var englishNameElement = item.Element("CcyNm");
                var englishName = ReadCurrencyName(englishNameElement);

                // Fund
                bool isFund = false;
                var isFundAttr = englishNameElement.Attribute("IsFund");
                if (isFundAttr != null) {
                    __ValidateIsFund(isFundAttr.Value);

                    isFund = isFundAttr.Value == "true";
                }

                // Country English Name
                var englishRegionName = ReadRegionName(item.Element("CtryNm"));

                // Minor Units
                var minorUnitsValue = item.Element("CcyMnrUnts").Value;
                short? minorUnits = null;
                if (HasMinorUnits(minorUnitsValue)) {
                    minorUnits = Int16.Parse(minorUnitsValue, CultureInfo.InvariantCulture);
                }

                yield return new CurrencyInfo(code, numericCode) {
                    EnglishName = englishName,
                    EnglishRegionName = englishRegionName,
                    IsFund = isFund,
                    MinorUnits = minorUnits,
                };
            }
        }
    }
}
