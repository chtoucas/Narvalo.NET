// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using StyleCop;
    using StyleCop.CSharp;

    internal static class AnalyzerUtility
    {
        public static bool IsGeneratedOrDesignerFile(CsDocument document)
        {
            Param.AssertNotNull(document, "document");

            string fileName = document.SourceCode.Name;

            return fileName.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase)
               || fileName.EndsWith(".Designer.cs", StringComparison.OrdinalIgnoreCase);
        }

        public static string TrimGenericInfoFromElementName(string elementName)
        {
            Param.AssertNotNull(elementName, "elementName");

            int indexOfBracket = elementName.IndexOf('<');

            return indexOfBracket != -1
                ? elementName.Substring(0, indexOfBracket)
                : elementName;
        }

        public static string TrimGenericInfoFromFileName(string elementName)
        {
            Param.AssertNotNull(elementName, "elementName");

            int indexOfBracket = elementName.IndexOf('`');

            return indexOfBracket != -1
                ? elementName.Substring(0, indexOfBracket)
                : elementName;
        }
    }
}
