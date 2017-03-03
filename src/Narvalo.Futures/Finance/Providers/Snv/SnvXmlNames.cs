// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Providers.Snv
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

        public static XName AlphabeticCode => s_AlphabeticCode;
        public static XName CountryName => s_CountryName;
        public static XName EnglishName => s_EnglishName;
        public static XName Item => s_Item;
        public static XName IsFund => s_IsFund;
        public static XName LegacyItem => s_LegacyItem;
        public static XName LegacyList => s_LegacyList;
        public static XName List => s_List;
        public static XName MinorUnits => s_MinorUnits;
        public static XName NumericCode => s_NumericCode;
        public static XName PubDate => s_PubDate;
        public static XName WithdrawnDate => s_WithdrawnDate;
    }
}
