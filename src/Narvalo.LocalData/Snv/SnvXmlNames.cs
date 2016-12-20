// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.LocalData.Snv
{
    using System.Xml.Linq;

    internal static class SnvXmlNames
    {
        private static readonly XName s_AlphabeticCode = XName.Get("Ccy");
        private static readonly XName s_CountryName = XName.Get("CtryNm");
        private static readonly XName s_EnglishName = XName.Get("CcyNm");
        private static readonly XName s_IsFund = XName.Get("IsFund");
        private static readonly XName s_Item = XName.Get("CcyNtry");
        private static readonly XName s_LegacyItem = XName.Get("HstrcCcyNtry");
        private static readonly XName s_LegacyList = XName.Get("HstrcCcyTbl");
        private static readonly XName s_List = XName.Get("CcyTbl");
        private static readonly XName s_MinorUnits = XName.Get("CcyMnrUnts");
        private static readonly XName s_NumericCode = XName.Get("CcyNbr");
        private static readonly XName s_PubDate = XName.Get("Pblshd");
        private static readonly XName s_WithdrawnDate = XName.Get("WthdrwlDt");

        public static XName AlphabeticCode { get { Warrant.NotNull<XName>(); return s_AlphabeticCode; } }
        public static XName CountryName { get { Warrant.NotNull<XName>(); return s_CountryName; } }
        public static XName EnglishName { get { Warrant.NotNull<XName>(); return s_EnglishName; } }
        public static XName Item { get { Warrant.NotNull<XName>(); return s_Item; } }
        public static XName IsFund { get { Warrant.NotNull<XName>(); return s_IsFund; } }
        public static XName LegacyItem { get { Warrant.NotNull<XName>(); return s_LegacyItem; } }
        public static XName LegacyList { get { Warrant.NotNull<XName>(); return s_LegacyList; } }
        public static XName List { get { Warrant.NotNull<XName>(); return s_List; } }
        public static XName MinorUnits { get { Warrant.NotNull<XName>(); return s_MinorUnits; } }
        public static XName NumericCode { get { Warrant.NotNull<XName>(); return s_NumericCode; } }
        public static XName PubDate { get { Warrant.NotNull<XName>(); return s_PubDate; } }
        public static XName WithdrawnDate { get { Warrant.NotNull<XName>(); return s_WithdrawnDate; } }
    }
}
