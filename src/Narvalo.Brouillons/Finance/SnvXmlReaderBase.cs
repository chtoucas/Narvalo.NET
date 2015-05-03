// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    public abstract class SnvXmlReaderBase
    {
        private const string MINOR_UNITS_NOT_AVAILABLE = "N.A.";

        private readonly string _source;

        protected SnvXmlReaderBase(string source)
        {
            Require.NotNullOrEmpty(source, "source");

            _source = source;
        }

        public string Source { get { return _source; } }

        protected string ProcessAlphabeticCode(string code)
        {
            Debug.Assert(code.Length == 3, "The alphabetic code MUST be composed of exactly 3 characters.");

            return code;
        }

        protected string ProcessCurrencyName(string name)
        {
            return name.Replace("\"", "\"\"");
        }

        protected bool ProcessIsFund(string value)
        {
            // NB: A blank value is interpreted to be the same as no attribute.
            // Only applies to the legacy XML source.
            if (String.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            Debug.Assert(value == "true", "When present, the 'IsFund' attribute value is expected to be equal to 'true'.");

            return value == "true";
        }

        protected short? ProcessMinorUnits(string value)
        {
            if (value == MINOR_UNITS_NOT_AVAILABLE)
            {
                return null;
            }
            else
            {
                short? result = ParseTo.Int16(value);
                Debug.Assert(result.HasValue);

                return result.Value;
            }
        }

        protected short ProcessNumericCode(string value)
        {
            short? result = ParseTo.Int16(value);
            Debug.Assert(result.HasValue);

            short retval = result.Value;
            Debug.Assert(retval > 0, "The numeric code MUST be strictly greater than 0.");
            Debug.Assert(retval < 1000, "The numeric code MUST be strictly less than 1000.");

            return retval;
        }

        protected DateTime ProcessPublicationDate(string value)
        {
            DateTime? result = ParseTo.DateTime(value);
            Debug.Assert(result.HasValue);

            return result.Value;
        }

        protected string ProcessRegionName(string name)
        {
            return name.Replace('’', '\'')
                .Replace("\"", "\"\"")
                .Replace("\n", String.Empty);
        }

        protected XElement ReadContent()
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
            using (var reader = XmlReader.Create(Source, settings))
            {
                retval = XElement.Load(reader, LoadOptions.None);
            }

            return retval;
        }

        protected Func<FinanceException> ExceptionThunk(string message)
        {
            return () => new FinanceException(message);
        }
    }
}
