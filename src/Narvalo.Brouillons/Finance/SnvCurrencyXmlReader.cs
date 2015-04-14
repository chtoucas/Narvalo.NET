// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    using Narvalo.Xml;
    using Narvalo.Finance.Legacy;

    public sealed class SnvCurrencyXmlReader : SnvCurrencyXmlReaderBase
    {
        private DateTime? _publicationDate;

        public SnvCurrencyXmlReader(string source) : base(source) { }

        public DateTime PublicationDate
        {
            get
            {
                if (!_publicationDate.HasValue)
                {
                    throw new CurrencyException("XXX");
                }

                return _publicationDate.Value;
            }
        }

        public IEnumerable<CurrencyInfo> Read()
        {
            var root = ReadContent();

            _publicationDate = root
                .AttributeOrThrow("Pblshd", ExceptionThunk("XXX"))
                .Value(ProcessPublicationDate);

            var currencyElements = root
                .ElementOrThrow("CcyTbl", ExceptionThunk("XXX"))
                .Elements("CcyNtry")
                .ToList();

            if (currencyElements.Count == 0)
            {
                throw new CurrencyException("XXX");
            }

            foreach (var currencyElement in currencyElements)
            {
                // English Name
                // NB: Keep the "englishNameElement" around, we will need it later on.
                XElement englishNameElement = currencyElement
                    .ElementOrThrow("CcyNm", ExceptionThunk("XXX"));
                string englishName = englishNameElement.Value(ProcessCurrencyName);

                // Alphabetic Code
                XElement codeElement = currencyElement.Element("Ccy");
                if (codeElement == null)
                {
                    Debug.WriteLine("Found a country without universal currency: " + englishName);

                    continue;
                }

                var code = codeElement.Value(ProcessAlphabeticCode);

                // Numeric Code
                short numericCode = currencyElement
                    .ElementOrThrow("CcyNbr", ExceptionThunk("XXX"))
                    .Value(ProcessNumericCode);

                // Fund?
                bool isFund = englishNameElement
                    .AttributeOrNone("IsFund")
                    .MapValue(ProcessIsFund)
                    .ValueOrElse(false);

                // Country English Name
                string englishRegionName = currencyElement
                    .ElementOrThrow("CtryNm", ExceptionThunk("XXX"))
                    .Value(ProcessRegionName);

                // Minor Units
                short? minorUnits = currencyElement
                    .ElementOrThrow("CcyMnrUnts", ExceptionThunk("XXX"))
                    .Value(ProcessMinorUnits);

                yield return new CurrencyInfo(code, numericCode) {
                    EnglishName = englishName,
                    //EnglishRegionName = englishRegionName,
                    IsFund = isFund,
                    MinorUnits = minorUnits,
                };
            }
        }
    }
}
