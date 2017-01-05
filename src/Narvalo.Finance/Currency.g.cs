﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.42000
// Microsoft.VisualStudio.TextTemplating: 14.0
// </auto-generated>
//------------------------------------------------------------------------------

namespace Narvalo.Finance
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public partial struct Currency
    {
        private static readonly decimal[] s_Epsilons = new decimal[]
        {
            1m,
            0.1m,
            0.01m,
            0.001m,
            0.0001m,
        };

        private static decimal[] s_Powers10 = new decimal[] {
            1m,
            10m,
            100m,
            1000m,
            10000m,
        };

        /// <summary>
        /// The list of available currency codes.
        /// </summary>
        [ContractVerification(false)]
        private static Dictionary<string, short?> Codes
        {
            get
            {
                if (s_Codes == null)
                {
                    var dict = new Dictionary<string, short?>() {
                        { "ADP", null},
                        { "AED", 2},
                        { "AFA", null},
                        { "AFN", 2},
                        { "ALK", null},
                        { "ALL", 2},
                        { "AMD", 2},
                        { "ANG", 2},
                        { "AOA", 2},
                        { "AOK", null},
                        { "AON", null},
                        { "AOR", null},
                        { "ARA", null},
                        { "ARP", null},
                        { "ARS", 2},
                        { "ARY", null},
                        { "ATS", null},
                        { "AUD", 2},
                        { "AWG", 2},
                        { "AYM", null},
                        { "AZM", null},
                        { "AZN", 2},
                        { "BAD", null},
                        { "BAM", 2},
                        { "BBD", 2},
                        { "BDT", 2},
                        { "BEC", null},
                        { "BEF", null},
                        { "BEL", null},
                        { "BGJ", null},
                        { "BGK", null},
                        { "BGL", null},
                        { "BGN", 2},
                        { "BHD", 3},
                        { "BIF", 0},
                        { "BMD", 2},
                        { "BND", 2},
                        { "BOB", 2},
                        { "BOP", null},
                        { "BOV", 2},
                        { "BRB", null},
                        { "BRC", null},
                        { "BRE", null},
                        { "BRL", 2},
                        { "BRN", null},
                        { "BRR", null},
                        { "BSD", 2},
                        { "BTN", 2},
                        { "BUK", null},
                        { "BWP", 2},
                        { "BYB", null},
                        { "BYN", 2},
                        { "BYR", 0},
                        { "BZD", 2},
                        { "CAD", 2},
                        { "CDF", 2},
                        { "CHC", null},
                        { "CHE", 2},
                        { "CHF", 2},
                        { "CHW", 2},
                        { "CLF", 4},
                        { "CLP", 0},
                        { "CNX", null},
                        { "CNY", 2},
                        { "COP", 2},
                        { "COU", 2},
                        { "CRC", 2},
                        { "CSD", null},
                        { "CSJ", null},
                        { "CSK", null},
                        { "CUC", 2},
                        { "CUP", 2},
                        { "CVE", 2},
                        { "CYP", null},
                        { "CZK", 2},
                        { "DDM", null},
                        { "DEM", null},
                        { "DJF", 0},
                        { "DKK", 2},
                        { "DOP", 2},
                        { "DZD", 2},
                        { "ECS", null},
                        { "ECV", null},
                        { "EEK", null},
                        { "EGP", 2},
                        { "EQE", null},
                        { "ERN", 2},
                        { "ESA", null},
                        { "ESB", null},
                        { "ESP", null},
                        { "ETB", 2},
                        { "EUR", 2},
                        { "FIM", null},
                        { "FJD", 2},
                        { "FKP", 2},
                        { "FRF", null},
                        { "GBP", 2},
                        { "GEK", null},
                        { "GEL", 2},
                        { "GHC", null},
                        { "GHP", null},
                        { "GHS", 2},
                        { "GIP", 2},
                        { "GMD", 2},
                        { "GNE", null},
                        { "GNF", 0},
                        { "GNS", null},
                        { "GQE", null},
                        { "GRD", null},
                        { "GTQ", 2},
                        { "GWE", null},
                        { "GWP", null},
                        { "GYD", 2},
                        { "HKD", 2},
                        { "HNL", 2},
                        { "HRD", null},
                        { "HRK", 2},
                        { "HTG", 2},
                        { "HUF", 2},
                        { "IDR", 2},
                        { "IEP", null},
                        { "ILP", null},
                        { "ILR", null},
                        { "ILS", 2},
                        { "INR", 2},
                        { "IQD", 3},
                        { "IRR", 2},
                        { "ISJ", null},
                        { "ISK", 0},
                        { "ITL", null},
                        { "JMD", 2},
                        { "JOD", 3},
                        { "JPY", 0},
                        { "KES", 2},
                        { "KGS", 2},
                        { "KHR", 2},
                        { "KMF", 0},
                        { "KPW", 2},
                        { "KRW", 0},
                        { "KWD", 3},
                        { "KYD", 2},
                        { "KZT", 2},
                        { "LAJ", null},
                        { "LAK", 2},
                        { "LBP", 2},
                        { "LKR", 2},
                        { "LRD", 2},
                        { "LSL", 2},
                        { "LSM", null},
                        { "LTL", null},
                        { "LTT", null},
                        { "LUC", null},
                        { "LUF", null},
                        { "LUL", null},
                        { "LVL", null},
                        { "LVR", null},
                        { "LYD", 3},
                        { "MAD", 2},
                        { "MAF", null},
                        { "MDL", 2},
                        { "MGA", 2},
                        { "MGF", null},
                        { "MKD", 2},
                        { "MLF", null},
                        { "MMK", 2},
                        { "MNT", 2},
                        { "MOP", 2},
                        { "MRO", 2},
                        { "MTL", null},
                        { "MTP", null},
                        { "MUR", 2},
                        { "MVQ", null},
                        { "MVR", 2},
                        { "MWK", 2},
                        { "MXN", 2},
                        { "MXP", null},
                        { "MXV", 2},
                        { "MYR", 2},
                        { "MZE", null},
                        { "MZM", null},
                        { "MZN", 2},
                        { "NAD", 2},
                        { "NGN", 2},
                        { "NIC", null},
                        { "NIO", 2},
                        { "NLG", null},
                        { "NOK", 2},
                        { "NPR", 2},
                        { "NZD", 2},
                        { "OMR", 3},
                        { "PAB", 2},
                        { "PEH", null},
                        { "PEI", null},
                        { "PEN", 2},
                        { "PES", null},
                        { "PGK", 2},
                        { "PHP", 2},
                        { "PKR", 2},
                        { "PLN", 2},
                        { "PLZ", null},
                        { "PTE", null},
                        { "PYG", 0},
                        { "QAR", 2},
                        { "RHD", null},
                        { "ROK", null},
                        { "ROL", null},
                        { "RON", 2},
                        { "RSD", 2},
                        { "RUB", 2},
                        { "RUR", null},
                        { "RWF", 0},
                        { "SAR", 2},
                        { "SBD", 2},
                        { "SCR", 2},
                        { "SDD", null},
                        { "SDG", 2},
                        { "SDP", null},
                        { "SEK", 2},
                        { "SGD", 2},
                        { "SHP", 2},
                        { "SIT", null},
                        { "SKK", null},
                        { "SLL", 2},
                        { "SOS", 2},
                        { "SRD", 2},
                        { "SRG", null},
                        { "SSP", 2},
                        { "STD", 2},
                        { "SUR", null},
                        { "SVC", 2},
                        { "SYP", 2},
                        { "SZL", 2},
                        { "THB", 2},
                        { "TJR", null},
                        { "TJS", 2},
                        { "TMM", null},
                        { "TMT", 2},
                        { "TND", 3},
                        { "TOP", 2},
                        { "TPE", null},
                        { "TRL", null},
                        { "TRY", 2},
                        { "TTD", 2},
                        { "TWD", 2},
                        { "TZS", 2},
                        { "UAH", 2},
                        { "UAK", null},
                        { "UGS", null},
                        { "UGW", null},
                        { "UGX", 0},
                        { "USD", 2},
                        { "USN", 2},
                        { "USS", null},
                        { "UYI", 0},
                        { "UYN", null},
                        { "UYP", null},
                        { "UYU", 2},
                        { "UZS", 2},
                        { "VEB", null},
                        { "VEF", 2},
                        { "VNC", null},
                        { "VND", 0},
                        { "VUV", 0},
                        { "WST", 2},
                        { "XAF", 0},
                        { "XAG", null},
                        { "XAU", null},
                        { "XBA", null},
                        { "XBB", null},
                        { "XBC", null},
                        { "XBD", null},
                        { "XCD", 2},
                        { "XDR", null},
                        { "XEU", null},
                        { "XFO", null},
                        { "XFU", null},
                        { "XOF", 0},
                        { "XPD", null},
                        { "XPF", 0},
                        { "XPT", null},
                        { "XRE", null},
                        { "XSU", null},
                        { "XTS", null},
                        { "XUA", null},
                        { "XXX", null},
                        { "YDD", null},
                        { "YER", 2},
                        { "YUD", null},
                        { "YUM", null},
                        { "YUN", null},
                        { "ZAL", null},
                        { "ZAR", 2},
                        { "ZMK", null},
                        { "ZMW", 2},
                        { "ZRN", null},
                        { "ZRZ", null},
                        { "ZWC", null},
                        { "ZWD", null},
                        { "ZWL", 2},
                        { "ZWN", null},
                        { "ZWR", null},
                    };

                    s_Codes = dict;
                }

                return s_Codes;
            }
        }
    }
}