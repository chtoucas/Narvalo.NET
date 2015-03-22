// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System.Diagnostics;

    public class Locale
    {
        /// <summary>
        /// Obtains the fallback currency symbol for a given currency code.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Since the currency symbol actually depends on the culture and local habits, this method 
        /// can only return a closest match. The result is then non-authoritative.
        /// </para>
        /// <para>
        /// If none is found, returns the Unicode currency sign.
        /// </para>
        /// </remarks>
        /// <param name="code">The alphabetic code of the currency.</param>
        /// <returns>The fallback currency symbol for a given currency code.</returns>
        public string GetFallbackSymbol(string code)
        {
            // The list is compiled from various sources:
            // - http://unicode.org/charts/PDF/U20A0.pdf (authoritative)
            // - http://www.xe.com/symbols.php
            // - http://en.wikipedia.org/wiki/Currency_symbol
            switch (code) {
                // Symbols made up from four characters.
                case "RSD": return "\x0414\x0438\x043d\x002e";    // Дин.

                // Symbols made up from three characters.
                case "BZD": return "\x0042\x005a\x0024";    // BZ$
                case "PAB": return "\x0042\x002f\x002e";    // B/.
                case "CHF": return "\x0043\x0048\x0046";    // CHF
                case "ALL": return "\x004c\x0065\x006b";    // Lek
                case "TWD": return "\x004e\x0054\x0024";    // NT$      REVIEW Taïwan
                case "DOP": return "\x0052\x0044\x0024";    // RD$
                case "PEN": return "\x0053\x002f\x002e";    // S/.
                case "TTD": return "\x0054\x0054\x0024";    // TT$
                case "RON": return "\x006c\x0065\x0069";    // lei
                case "MKD": return "\x0434\x0435\x043d";    // ден
                case "AZN": return "\x043c\x0430\x043d";    // ман
                case "RUB": return "\x0440\x0443\x0431";    // руб      REVIEW Russia

                // Symbols made up from two characters.
                case "UYU": return "\x0024\x0055";          // $U
                case "BOB": return "\x0024\x0062";          // $b
                case "VEF": return "\x0042\x0073";          // Bs
                case "NIO": return "\x0043\x0024";          // C$
                case "HUF": return "\x0046\x0074";          // Ft
                ////case "PYG": return "\x0047\x0073";        // Gs       REVIEW PYG
                case "JMD": return "\x004a\x0024";          // J$
                case "BAM": return "\x004b\x004d";          // KM
                case "CZK": return "\x004b\x010d";          // Kč
                case "LVL": return "\x004c\x0073";          // Ls
                case "LTL": return "\x004c\x0074";          // Lt
                case "MZN": return "\x004d\x0054";          // MT
                case "BRL": return "\x0052\x0024";          // R$
                case "MYR": return "\x0052\x004d";          // RM
                case "IDR": return "\x0052\x0070";          // Rp
                case "ZWD": return "\x005a\x0024";          // Z$
                case "HRK": return "\x006b\x006e";          // kn
                case "DKK":
                case "EEK":
                case "ISK":
                case "NOK":
                case "SEK": return "\x006b\x0072";          // kr
                case "BYR": return "\x0070\x002e";          // p.
                case "PLN": return "\x007a\x0142";          // zł
                case "BGN":
                case "KGS":
                case "KZT":
                case "UZS": return "\x043b\x0432";          // лв

                // Symbols NOT in the Unicode range: \x20a0-\x20cf
                case "ARS":
                case "AUD":
                case "BND":
                case "BBD":
                case "BMD":
                case "BSD":
                case "CAD":
                case "CLP":
                case "COP":
                case "FJD":
                case "GYD":
                case "HKD": // Sometimes spelled HK$ - REVIEW also \x5143 ?
                case "KYD":
                case "LRD":
                case "MXN":
                case "NAD":
                case "NZD":
                case "SBD":
                case "SGD":
                case "SRD":
                case "SVC":
                case "TVD":
                case "USD":
                case "XCD": return "\x0024";    // $ - DOLLAR SIGN
                case "HNL": return "\x004c";    // L - LATIN CAPITAL LETTER L
                case "BWP": return "\x0050";    // P - LATIN CAPITAL LETTER P
                case "GTQ": return "\x0051";    // Q - LATIN CAPITAL LETTER Q
                case "ZAR": return "\x0052";    // R - LATIN CAPITAL LETTER R
                case "SOS": return "\x0053";    // S - LATIN CAPITAL LETTER S
                case "GHC": return "\x00a2";    // ¢ - CENT SIGN                    NB: LEGACY -> GHS REVIEW
                case "EGP":
                case "FKP":
                case "GBP":
                case "GGP":
                case "GIP":
                case "IMP":
                case "ITL":
                case "JEP":
                case "LBP":
                case "SHP":
                case "SYP": return "\x00a3";    // £ - POUND SIGN
                case "XDR": return "\x00a4";    // ¤ - CURRENCY SIGN
                case "CNY":
                case "JPY": return "\x00a5";    // ¥ - YEN SIGN
                case "AWG":
                case "ANG": return "\x0192";    // ƒ - LATIN SMALL LETTER F WITH HOOK
                case "AFN": return "\x060b";    // ؋ - AFGHANI SIGN
                case "THB": return "\x0e3f";    // ฿ - THAI CURRENCY SYMBOL BAHT
                case "KHR": return "\x17db";    // ៛ - KHMER CURRENCY SYMBOL RIEL
                case "IRR":
                case "OMR":
                case "QAR":
                case "SAR":
                case "YER": return "\xfdfc";    // ﷼ - RIAL SIGN

                // Symbols in the Unicode range: \x20a0-\x20cf
                //// \x20a0 EURO-CURRENCY SIGN - NB: Intended for ECU before the EURO came to life.
                case "CRC": return "\x20a1";    // ₡ - COLON SIGN
                case "BRE": return "\x20a2";    // ₢ - CRUZEIRO SIGN        NB: LEGACY -> EUR
                case "FRF": return "\x20a3";    // ₣ - FRENCH FRANC SIGN    NB: LEGACY -> EUR
                case "TRL": return "\x20a4";    // ₤ - LIRA SIGN            NB: LEGACY -> EUR
                //// \x20a5 MILL SIGN
                case "NGN": return "\x20a6";    // ₦ - NAIRA SIGN
                case "ESP": return "\x20a7";    // ₧ - PESETA SIGN          NB: LEGACY -> EUR
                case "LKR":
                case "MUR":
                case "NPR":
                case "PKR":
                case "SCR": return "\x20a8";    // ₨ - RUPEE SIGN
                case "KPW":
                case "KRW": return "\x20a9";    // ₩ - WON SIGN
                case "ILS": return "\x20aa";    // ₪ - NEW SHEQEL SIGN
                case "VND": return "\x20ab";    // ₫ - DONG SIGN
                case "EUR": return "\x20ac";    // € - EURO SIGN
                case "LAK": return "\x20ad";    // ₭ - KIP SIGN
                case "MNT": return "\x20ae";    // ₮ - TUGRIK SIGN
                case "GRD": return "\x20af";    // ₯ - DRACHMA SIGN         NB: LEGACY -> EUR
                //// \x20b0 GERMAN PENNY SIGN
                case "CUP":
                case "PHP": return "\x20b1";    // ₱ - PESO SIGN
                case "PYG": return "\x20b2";    // ₲ - GUARANI SIGN         NB: Often represented by G or Gs - REVIEW Paraguay
                case "ARA": return "\x20b3";    // ₳ - AUSTRAL SIGN         NB: LEGACY -> ARS
                case "UAH": return "\x20b4";    // ₴ - HRYVNIA SIGN
                ////case "GHS": return "\x20b5";    // ₵ - CEDI SIGN          REVIEW Ghana, GH₵?
                //// \x20b6 LIVRE TOURNOIS SIGN - NB: Historical
                //// \x20b7 SPESMILO SIGN - NB: Historical
                ////case "KZT": return "\x20b8";    // ₸ - TENGE SIGN         REVIEW Kazakhstan
                case "INR": return "\x20b9";    // ₹ - INDIAN RUPEE SIGN
                case "TRY": return "\x20ba";    // ₺ - TURKISH LIRA SIGN
                //// \x20bb NORDIC MARK SIGN                                  REVIEW Denmark, Norway
                //// \x20bc MANAT SIGN                                        REVIEW Azerbaijan
                //// \x20bd RUBLE SIGN                                        REVIEW Russia

                default:
                    Debug.WriteLine("No symbol found for the currency: " + code);
                    return "\x00a4";                  // ¤ - CURRENCY SIGN
            }
        }
    }
}
