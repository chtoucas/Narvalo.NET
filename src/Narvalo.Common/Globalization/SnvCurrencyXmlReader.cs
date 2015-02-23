// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    using Narvalo.Xml;

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
                .Select(ProcessPublicationDate);

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
                string englishName = englishNameElement.Select(ProcessCurrencyName);

                // Alphabetic Code
                XElement codeElement = currencyElement.Element("Ccy");
                if (codeElement == null)
                {
                    Debug.WriteLine("Found a country without universal currency: " + englishName);

                    continue;
                }

                var code = codeElement.Select(ProcessAlphabeticCode);

                // Numeric Code
                short numericCode = currencyElement
                    .ElementOrThrow("CcyNbr", ExceptionThunk("XXX"))
                    .Select(ProcessNumericCode);

                // Fund?
                bool isFund = englishNameElement
                    .AttributeOrNone("IsFund")
                    .Select(_ => _.Select(ProcessIsFund))
                    .ValueOrElse(false);

                // Country English Name
                string englishRegionName = currencyElement
                    .ElementOrThrow("CtryNm", ExceptionThunk("XXX"))
                    .Select(ProcessRegionName);

                // Minor Units
                short? minorUnits = currencyElement
                    .ElementOrThrow("CcyMnrUnts", ExceptionThunk("XXX"))
                    .Select(ProcessMinorUnits);

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
