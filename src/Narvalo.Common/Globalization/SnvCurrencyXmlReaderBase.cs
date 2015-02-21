// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    public abstract class SnvCurrencyXmlReaderBase
    {
        private const string MINOR_UNITS_NOT_AVAILABLE = "N.A.";

        protected SnvCurrencyXmlReaderBase() { }

        protected internal string ReadCurrencyName(XElement element)
        {
            return element.Value.Replace("\"", "\"\"");
        }

        protected internal string ReadRegionName(XElement element)
        {
            return element.Value.Replace("’", "'").Replace("\"", "\"\"").Replace("\n", String.Empty);
        }

        protected internal bool HasMinorUnits(string value)
        {
            return value != MINOR_UNITS_NOT_AVAILABLE;
        }

        protected internal XElement ReadContent(string source)
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

            XElement retval;
            using (var reader = XmlReader.Create(source, settings)) {
                retval = XElement.Load(reader, LoadOptions.None);
            }

            return retval;
        }

        protected internal short ReadValueAsShort(XElement element)
        {
            return Int16.Parse(element.Value, CultureInfo.InvariantCulture);
        }

        [Conditional("DEBUG")]
        [CLSCompliant(false)]
        protected internal void __ValidateCode(string code)
        {
            Debug.Assert(code.Length == 3, "The alphabetic code MUST be composed of exactly 3 characters.");
        }

        [Conditional("DEBUG")]
        [CLSCompliant(false)]
        protected internal void __ValidateIsFund(string value)
        {
            Debug.Assert(value == "true", "When present, the 'IsFund' attribute value is expected to be 'true'.");
        }

        [Conditional("DEBUG")]
        [CLSCompliant(false)]
        protected internal void __ValidateNumericCode(short numericCode)
        {
            Debug.Assert(numericCode > 0, "The numeric code MUST be strictly greater than 0.");
            Debug.Assert(numericCode < 1000, "The numeric code MUST be strictly less than 1000.");
        }
    }
}
