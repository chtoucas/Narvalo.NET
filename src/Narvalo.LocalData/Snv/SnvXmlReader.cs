// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.LocalData.Snv
{
    using System.IO;
    using System.Xml;

    internal static class SnvXmlReader
    {
        private static readonly XmlReaderSettings s_Settings = new XmlReaderSettings
        {
            CheckCharacters = false,
            CloseInput = true,
            DtdProcessing = DtdProcessing.Ignore,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            //ValidationFlags = XmlSchemaValidationFlags.None,
            //ValidationType = ValidationType.None,
        };

        public static XmlReader Of(string input)
        {
            Demand.NotNullOrEmpty(input);
            Warrant.NotNull<XmlReader>();

            return XmlReader.Create(input, s_Settings);
        }

        public static XmlReader Of(Stream input)
        {
            Demand.NotNull(input);
            Warrant.NotNull<XmlReader>();

            return XmlReader.Create(input, s_Settings);
        }

        public static XmlReader Of(TextReader input)
        {
            Demand.NotNull(input);
            Warrant.NotNull<XmlReader>();

            return XmlReader.Create(input, s_Settings);
        }

        public static XmlReader Of(XmlReader input)
        {
            Demand.NotNull(input);
            Warrant.NotNull<XmlReader>();

            return XmlReader.Create(input, s_Settings);
        }
    }
}
