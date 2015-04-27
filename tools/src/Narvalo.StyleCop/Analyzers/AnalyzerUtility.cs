// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Analyzers
{
    using global::StyleCop;

    internal static class AnalyzerUtility
    {
        public static string TrimGenericInfoFromElementName(string elementName)
        {
            Param.AssertNotNull(elementName, "elementName");

            // Remove the generic part in the name.
            var indexOfBracket = elementName.IndexOf('<');
            return indexOfBracket != -1
                ? elementName.Substring(0, indexOfBracket)
                : elementName;

        }

        public static string TrimGenericInfoFromFilename(string elementName)
        {
            Param.AssertNotNull(elementName, "elementName");

            // Remove the generic part in the name.
            var indexOfBracket = elementName.IndexOf('`');
            return indexOfBracket != -1
                ? elementName.Substring(0, indexOfBracket)
                : elementName;

        }
    }
}
