// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    public sealed class XmlValidator
    {
        public IList<XmlValidationError> ValidateDtd(XmlReader reader, XmlReaderSettings settings)
        {
            var errors = new List<XmlValidationError>();

            reader.Settings.DtdProcessing = DtdProcessing.Parse;
            reader.Settings.ValidationType = ValidationType.DTD;
            reader.Settings.ValidationEventHandler += (sender, e) => errors.Add(new XmlValidationError(e));

            while (reader.Read()) ;

            return errors;
        }

        public IList<XmlValidationError> ValidateSchema(XmlReader reader, XmlReaderSettings settings)
        {
            var errors = new List<XmlValidationError>();

            reader.Settings.ValidationType = ValidationType.Schema;
            reader.Settings.ValidationEventHandler += (sender, e) => errors.Add(new XmlValidationError(e));

            while (reader.Read()) ;

            return errors;
        }

        public bool FastValidate(XmlReader reader)
        {
            while (reader.Read()) ;

            return true;
        }

        public bool SlowValidate(XmlReader reader)
        {
            try {
                while (reader.Read()) ;
            }
            catch (XmlSchemaValidationException) {
                throw;
            }

            return true;
        }
    }
}
