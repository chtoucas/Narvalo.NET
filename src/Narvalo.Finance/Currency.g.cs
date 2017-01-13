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
    using System.Threading;

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

        private static readonly uint[] s_PowersOfTen = new uint[] {
            1,
            10,
            100,
            1000,
            10000,
        };

        // This list is automatically generated using data obtained from the SNV website.
        private static readonly Dictionary<string, short?> s_Codes = new Dictionary<string, short?>(179) {
            { "AED", 2},
            { "AFN", 2},
            { "ALL", 2},
            { "AMD", 2},
            { "ANG", 2},
            { "AOA", 2},
            { "ARS", 2},
            { "AUD", 2},
            { "AWG", 2},
            { "AZN", 2},
            { "BAM", 2},
            { "BBD", 2},
            { "BDT", 2},
            { "BGN", 2},
            { "BHD", 3},
            { "BIF", 0},
            { "BMD", 2},
            { "BND", 2},
            { "BOB", 2},
            { "BOV", 2},
            { "BRL", 2},
            { "BSD", 2},
            { "BTN", 2},
            { "BWP", 2},
            { "BYN", 2},
            { "BYR", 0},
            { "BZD", 2},
            { "CAD", 2},
            { "CDF", 2},
            { "CHE", 2},
            { "CHF", 2},
            { "CHW", 2},
            { "CLF", 4},
            { "CLP", 0},
            { "CNY", 2},
            { "COP", 2},
            { "COU", 2},
            { "CRC", 2},
            { "CUC", 2},
            { "CUP", 2},
            { "CVE", 2},
            { "CZK", 2},
            { "DJF", 0},
            { "DKK", 2},
            { "DOP", 2},
            { "DZD", 2},
            { "EGP", 2},
            { "ERN", 2},
            { "ETB", 2},
            { "EUR", 2},
            { "FJD", 2},
            { "FKP", 2},
            { "GBP", 2},
            { "GEL", 2},
            { "GHS", 2},
            { "GIP", 2},
            { "GMD", 2},
            { "GNF", 0},
            { "GTQ", 2},
            { "GYD", 2},
            { "HKD", 2},
            { "HNL", 2},
            { "HRK", 2},
            { "HTG", 2},
            { "HUF", 2},
            { "IDR", 2},
            { "ILS", 2},
            { "INR", 2},
            { "IQD", 3},
            { "IRR", 2},
            { "ISK", 0},
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
            { "LAK", 2},
            { "LBP", 2},
            { "LKR", 2},
            { "LRD", 2},
            { "LSL", 2},
            { "LYD", 3},
            { "MAD", 2},
            { "MDL", 2},
            { "MGA", 2},
            { "MKD", 2},
            { "MMK", 2},
            { "MNT", 2},
            { "MOP", 2},
            { "MRO", 2},
            { "MUR", 2},
            { "MVR", 2},
            { "MWK", 2},
            { "MXN", 2},
            { "MXV", 2},
            { "MYR", 2},
            { "MZN", 2},
            { "NAD", 2},
            { "NGN", 2},
            { "NIO", 2},
            { "NOK", 2},
            { "NPR", 2},
            { "NZD", 2},
            { "OMR", 3},
            { "PAB", 2},
            { "PEN", 2},
            { "PGK", 2},
            { "PHP", 2},
            { "PKR", 2},
            { "PLN", 2},
            { "PYG", 0},
            { "QAR", 2},
            { "RON", 2},
            { "RSD", 2},
            { "RUB", 2},
            { "RWF", 0},
            { "SAR", 2},
            { "SBD", 2},
            { "SCR", 2},
            { "SDG", 2},
            { "SEK", 2},
            { "SGD", 2},
            { "SHP", 2},
            { "SLL", 2},
            { "SOS", 2},
            { "SRD", 2},
            { "SSP", 2},
            { "STD", 2},
            { "SVC", 2},
            { "SYP", 2},
            { "SZL", 2},
            { "THB", 2},
            { "TJS", 2},
            { "TMT", 2},
            { "TND", 3},
            { "TOP", 2},
            { "TRY", 2},
            { "TTD", 2},
            { "TWD", 2},
            { "TZS", 2},
            { "UAH", 2},
            { "UGX", 0},
            { "USD", 2},
            { "USN", 2},
            { "UYI", 0},
            { "UYU", 2},
            { "UZS", 2},
            { "VEF", 2},
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
            { "XOF", 0},
            { "XPD", null},
            { "XPF", 0},
            { "XPT", null},
            { "XSU", null},
            { "XTS", null},
            { "XUA", null},
            { "XXX", null},
            { "YER", 2},
            { "ZAR", 2},
            { "ZMW", 2},
            { "ZWL", 2},
        };

        /// <summary>
        /// Gets the set of legacy currencies.
        /// </summary>
        [ContractVerification(false)]
        internal static HashSet<string> WithdrawnCodes
        {
            get
            {
                Warrant.NotNull<HashSet<string>>();

                if (s_WithdrawnCodes == null)
                {
                    var set = new HashSet<string>() {
                        "ADP",
                        "AFA",
                        "ALK",
                        "ANG",
                        "AOK",
                        "AON",
                        "AOR",
                        "ARA",
                        "ARP",
                        "ARY",
                        "ATS",
                        "AYM",
                        "AZM",
                        "BAD",
                        "BEC",
                        "BEF",
                        "BEL",
                        "BGJ",
                        "BGK",
                        "BGL",
                        "BOP",
                        "BRB",
                        "BRC",
                        "BRE",
                        "BRN",
                        "BRR",
                        "BUK",
                        "BYB",
                        "CHC",
                        "CNX",
                        "CSD",
                        "CSJ",
                        "CSK",
                        "CYP",
                        "DDM",
                        "DEM",
                        "ECS",
                        "ECV",
                        "EEK",
                        "EQE",
                        "ESA",
                        "ESB",
                        "ESP",
                        "ESP",
                        "EUR",
                        "FIM",
                        "FIM",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "FRF",
                        "GEK",
                        "GHC",
                        "GHP",
                        "GNE",
                        "GNS",
                        "GQE",
                        "GRD",
                        "GWE",
                        "GWP",
                        "HRD",
                        "HRK",
                        "IDR",
                        "IEP",
                        "ILP",
                        "ILR",
                        "ISJ",
                        "ITL",
                        "ITL",
                        "ITL",
                        "LAJ",
                        "LSM",
                        "LTL",
                        "LTT",
                        "LUC",
                        "LUF",
                        "LUL",
                        "LVL",
                        "LVR",
                        "MAF",
                        "MGF",
                        "MLF",
                        "MTL",
                        "MTP",
                        "MVQ",
                        "MWK",
                        "MXP",
                        "MZE",
                        "MZM",
                        "NIC",
                        "NLG",
                        "PEH",
                        "PEI",
                        "PEN",
                        "PES",
                        "PLZ",
                        "PTE",
                        "RHD",
                        "ROK",
                        "ROL",
                        "RON",
                        "RUR",
                        "RUR",
                        "RUR",
                        "RUR",
                        "RUR",
                        "RUR",
                        "RUR",
                        "RUR",
                        "RUR",
                        "RUR",
                        "RUR",
                        "SDD",
                        "SDG",
                        "SDP",
                        "SIT",
                        "SKK",
                        "SRG",
                        "SUR",
                        "TJR",
                        "TMM",
                        "TPE",
                        "TRL",
                        "TRY",
                        "UAK",
                        "UGS",
                        "UGW",
                        "USS",
                        "UYN",
                        "UYP",
                        "VEB",
                        "VEF",
                        "VEF",
                        "VNC",
                        "XEU",
                        "XFO",
                        "XFU",
                        "XRE",
                        "YDD",
                        "YUD",
                        "YUM",
                        "YUN",
                        "ZAL",
                        "ZAL",
                        "ZMK",
                        "ZRN",
                        "ZRZ",
                        "ZWC",
                        "ZWD",
                        "ZWD",
                        "ZWN",
                        "ZWR",
                    };

                    // If the field is still null, write to it.
                    Interlocked.CompareExchange(ref s_WithdrawnCodes, set, null);
                }

                return s_WithdrawnCodes;
            }
        }
    }
}