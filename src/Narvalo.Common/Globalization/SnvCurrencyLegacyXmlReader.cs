// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public sealed class SnvCurrencyLegacyXmlReader : SnvCurrencyXmlReaderBase
    {
        readonly string _source;

        public SnvCurrencyLegacyXmlReader(string source)
        {
            Require.NotNullOrEmpty(source, "source");

            _source = source;
        }

        public DateTime? PublicationDate { get; private set; }

        public IEnumerable<CurrencyInfo> Read()
        {
            var root = ReadContent(_source);

            PublicationDate = ParseTo.DateTime(root.Attribute("Pblshd").Value).Value;

            var list = root.Element("HstrcCcyTbl").Elements("HstrcCcyNtry");

            foreach (var item in list) {
                // Currency Alphabetic Code
                var code = item.Element("Ccy").Value;
                __ValidateCode(code);

                // Currency Numeric Code
                var numericCodeElement = item.Element("CcyNbr");
                short numericCode;
                if (numericCodeElement == null) {
                    Debug.WriteLine("Found a currency without a numeric code: " + item.Element("CtryNm").Value);

                    numericCode = 0;
                }
                else {
                    numericCode = ReadValueAsShort(numericCodeElement);
                    __ValidateNumericCode(numericCode);
                }

                // Currency English Name
                var englishNameElement = item.Element("CcyNm");
                var englishName = ReadCurrencyName(englishNameElement);

                // Fund
                bool isFund = false;
                var isFundAttr = englishNameElement.Attribute("IsFund");
                if (isFundAttr != null) {
                    // NB: A blank value is interpreted to be the same as no attribute.
                    // Only applies to the legacy XML source.
                    __ValidateIsFund(isFundAttr.Value.Trim());

                    isFund = isFundAttr.Value == "true";
                }

                // Country English Name
                var englishRegionName = ReadRegionName(item.Element("CtryNm"));

                yield return new CurrencyInfo(code, numericCode) {
                    EnglishName = englishName,
                    EnglishRegionName = englishRegionName,
                    IsFund = isFund,
                    Superseded = true,
                };
            }
        }
    }
}
