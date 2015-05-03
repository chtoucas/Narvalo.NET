// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    using Narvalo.Xml;

    public sealed class SnvLegacyXmlReader : SnvXmlReaderBase
    {
        private DateTime? _publicationDate;

        public SnvLegacyXmlReader(string source) : base(source) { }

        public DateTime PublicationDate
        {
            get
            {
                if (!_publicationDate.HasValue)
                {
                    throw new FinanceException("XXX");
                }

                return _publicationDate.Value;
            }
        }

        public IEnumerable<CurrencyInfo> Read()
        {
            XElement root = ReadContent();

            _publicationDate = root
                .AttributeOrThrow("Pblshd", ExceptionThunk("XXX"))
                .Value(ProcessPublicationDate);

            List<XElement> currencyElements = root
                .ElementOrThrow("HstrcCcyTbl", ExceptionThunk("XXX"))
                .Elements("HstrcCcyNtry")
                .ToList();

            if (currencyElements.Count == 0)
            {
                throw new FinanceException("XXX");
            }

            foreach (var currencyElement in currencyElements)
            {
                // English Name
                // NB: Keep the "englishNameElement" around, we will need it later on.
                XElement englishNameElement = currencyElement
                    .ElementOrThrow("CcyNm", ExceptionThunk("XXX"));
                string englishName = englishNameElement.Value(ProcessCurrencyName);

                // Alphabetic Code
                string code = currencyElement
                    .ElementOrThrow("Ccy", ExceptionThunk("XXX"))
                    .Value(ProcessAlphabeticCode);

                // Numeric Code
                short numericCode = currencyElement
                    .ElementOrNone("CcyNbr")
                    .MapValue(ProcessNumericCode)
                    .ValueOrElse(0);
                if (numericCode == 0)
                {
                    Debug.WriteLine("Found a currency without a numeric code: " + englishName);
                }

                // Fund?
                bool isFund = englishNameElement
                    .AttributeOrNone("IsFund")
                    .MapValue(ProcessIsFund)
                    .ValueOrElse(false);

                // Country English Name
                string englishRegionName = currencyElement
                    .ElementOrThrow("CtryNm", ExceptionThunk("XXX"))
                    .Value(ProcessRegionName);

                yield return new CurrencyInfo(code, numericCode) {
                    EnglishName = englishName,
                    //EnglishRegionName = englishRegionName,
                    IsFund = isFund,
                    //Superseded = true,
                };
            }
        }
    }
}
