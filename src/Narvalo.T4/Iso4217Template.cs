// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.T4
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using Microsoft.VisualStudio.TextTemplating;

    /// <summary>
    /// Provides a base class for ISO-4217 templates.
    /// </summary>
    public abstract class Iso4217Template : VSTemplate
    {
        /// <summary>
        /// Set of features to support on the <see cref="XmlReader"/>.
        /// </summary>
        private static readonly XmlReaderSettings s_Settings = new XmlReaderSettings {
            CheckCharacters = false,
            CloseInput = true,
            DtdProcessing = DtdProcessing.Ignore,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            ValidationFlags = XmlSchemaValidationFlags.None,
            ValidationType = ValidationType.None,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Iso4217Template"/> class.
        /// </summary>
        protected Iso4217Template() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Iso4217Template"/> class.
        /// </summary>
        /// <param name="parent">The parent text transformation.</param>
        protected Iso4217Template(TextTransformation parent) : base(parent) { }

        /// <summary>
        /// Gets or sets a value indicating whether debugging is enabled.
        /// </summary>
        /// <value><see langword="true"/> if debugging is enabled; otherwise <see langword="false"/>.</value>
        public bool Debug { get; set; }

        protected List<Currency> ParseCurrent(string path)
        {
            var source = VSHost.ResolvePath(path);

            var retval = new List<Currency>();

            using (var reader = XmlReader.Create(source, s_Settings))
            {
                var root = XElement.Load(reader, LoadOptions.None);
                var list = root.Element("CcyTbl").Elements("CcyNtry");

                foreach (var item in list)
                {
                    // Currency Alphabetic Code.
                    var codeElement = item.Element("Ccy");
                    if (codeElement == null)
                    {
                        if (Debug)
                        {
                            Warning("Found a country without universal currency: " + item.Element("CtryNm").Value);
                        }

                        continue;
                    }

                    var code = codeElement.Value;

                    // Currency Numeric Code.
                    // NB: Int16.Parse should never fail.
                    var numericCode = Int16.Parse(item.Element("CcyNbr").Value);

                    // Currency English Name.
                    var englishNameElement = item.Element("CcyNm");
                    var englishName = englishNameElement.Value
                        .Replace("\"", "\"\"");

                    // Fund Currency.
                    bool isFund = false;
                    var isFundAttr = englishNameElement.Attribute("IsFund");
                    if (isFundAttr != null)
                    {
                        isFund = isFundAttr.Value == "true";
                    }

                    // Country English Name.
                    var englishRegionName = item.Element("CtryNm").Value
                        .Replace("’", "'")
                        .Replace("\"", "\"\"")
                        .Replace("\n", String.Empty);

                    // Minor Units.
                    var minorUnitsValue = item.Element("CcyMnrUnts").Value;
                    short? minorUnits = null;
                    if (minorUnitsValue != "N.A.")
                    {
                        // NB: ParseTo should never fail.
                        minorUnits = Int16.Parse(minorUnitsValue);
                    }

                    retval.Add(new Currency {
                        Code = code,
                        EnglishName = englishName,
                        EnglishRegionName = englishRegionName,
                        IsFund = isFund,
                        MinorUnits = minorUnits,
                        NumericCode = numericCode,
                    });
                }
            }

            return retval;
        }

        protected List<Currency> ParseLegacy(string path)
        {
            var source = VSHost.ResolvePath(path);

            var retval = new List<Currency>();

            using (var reader = XmlReader.Create(source, s_Settings))
            {
                var root = XElement.Load(reader, LoadOptions.None);
                var list = root.Element("HstrcCcyTbl").Elements("HstrcCcyNtry");

                foreach (var item in list)
                {
                    // Currency Alphabetic Code
                    var code = item.Element("Ccy").Value;

                    // Currency Numeric Code
                    // NB: ParseTo should never fail.
                    var numericCodeElement = item.Element("CcyNbr");
                    short numericCode;
                    if (numericCodeElement == null)
                    {
                        if (Debug)
                        {
                            Warning("Found a legacy currency without a numeric code: " + item.Element("CtryNm").Value);
                        }

                        numericCode = (short)0;
                    }
                    else
                    {
                        numericCode = Int16.Parse(numericCodeElement.Value);
                    }

                    // Currency English Name
                    var englishNameElement = item.Element("CcyNm");
                    var englishName = englishNameElement.Value
                        .Replace("\"", "\"\"");

                    // Fund Currency
                    bool isFund = false;
                    var isFundAttr = englishNameElement.Attribute("IsFund");
                    if (isFundAttr != null)
                    {
                        // NB: There are whitespace-only values, there are interpreted to be the same as no attibrute.
                        isFund = isFundAttr.Value == "true";
                    }

                    // Country English Name
                    var englishRegionName = item.Element("CtryNm").Value
                        .Replace("’", "'")
                        .Replace("\"", "\"\"")
                        .Replace("\n", String.Empty);

                    retval.Add(new Currency {
                        Code = code,
                        EnglishName = englishName,
                        EnglishRegionName = englishRegionName,
                        IsFund = isFund,
                        IsLegacy = true,
                        NumericCode = numericCode,
                    });
                }
            }

            return retval;
        }

        protected class Currency
        {
            public string Code { get; set; }

            public string EnglishName { get; set; }

            public string EnglishRegionName { get; set; }

            public bool IsFund { get; set; }

            public bool IsLegacy { get; set; }

            public short? MinorUnits { get; set; }

            public short NumericCode { get; set; }
        }
    }
}
