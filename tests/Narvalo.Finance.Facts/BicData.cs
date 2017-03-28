// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance {
    using System.Collections.Generic;

    public static class BicData {
        public static IEnumerable<object[]> SampleValues {
            get {
                yield return new object[] { "ABCDBEBB" };
                yield return new object[] { "ABCDBEBBXXX" };
            }
        }

        public static IEnumerable<object[]> IdenticalValues {
            get {
                yield return new object[] { "ABCDBEBB", "ABCDBEBB" };
                yield return new object[] { "ABCDBEBBXXX", "ABCDBEBBXXX" };
            }
        }

        public static IEnumerable<object[]> DistinctValues {
            get {
                yield return new object[] { "ABCDBEBB", "ABFRBEBB" };
                yield return new object[] { "ABCDBEBB", "ABFRBEBBXXX" };
                yield return new object[] { "ABCDBEBBXXX", "ABFRBEBB" };
                yield return new object[] { "ABCDBEBBXXX", "ABFRBEBBXXX" };
            }
        }

        public static IEnumerable<object[]> ValidISOValues {
            get {
                // Short form.
                yield return new object[] { "ABCDBEBB" };
                // Short form: digits in prefix.
                yield return new object[] { "1BCDBEBB" };
                yield return new object[] { "11CDBEBB" };
                yield return new object[] { "111DBEBB" };
                yield return new object[] { "1111BEBB" };
                yield return new object[] { "A1CDBEBB" };
                yield return new object[] { "A11DBEBB" };
                yield return new object[] { "A111BEBB" };
                yield return new object[] { "AB1DBEBB" };
                yield return new object[] { "AB11BEBB" };
                yield return new object[] { "ABC1BEBB" };
                // Short form: digits in suffix.
                yield return new object[] { "ABCDBE1B" };
                yield return new object[] { "ABCDBE11" };
                yield return new object[] { "ABCDBEB1" };
                // Long form.
                yield return new object[] { "ABCDBEBBXXX" };
                // Long form: digit in prefix.
                yield return new object[] { "1BCDBEBBXXX" };
                yield return new object[] { "11CDBEBBXXX" };
                yield return new object[] { "111DBEBBXXX" };
                yield return new object[] { "1111BEBBXXX" };
                yield return new object[] { "A1CDBEBBXXX" };
                yield return new object[] { "A11DBEBBXXX" };
                yield return new object[] { "A111BEBBXXX" };
                yield return new object[] { "AB1DBEBBXXX" };
                yield return new object[] { "AB11BEBBXXX" };
                yield return new object[] { "ABC1BEBBXXX" };
                // Long form: digit in suffix.
                yield return new object[] { "ABCDBE1BXXX" };
                yield return new object[] { "ABCDBE11XXX" };
                yield return new object[] { "ABCDBEB1XXX" };
                // Long form: digit in branch code.
                yield return new object[] { "ABCDBEBB1XX" };
                yield return new object[] { "ABCDBEBB11X" };
                yield return new object[] { "ABCDBEBB111" };
                yield return new object[] { "ABCDBEBBX1X" };
                yield return new object[] { "ABCDBEBBX11" };
                yield return new object[] { "ABCDBEBBXX1" };
            }
        }

        public static IEnumerable<object[]> InvalidISOValues {
            get {
                // Short form: lowercase letter in prefix.
                yield return new object[] { "aBCDBEBB" };
                yield return new object[] { "AbCDBEBB" };
                yield return new object[] { "ABcDBEBB" };
                yield return new object[] { "ABCdBEBB" };
                // Short form: lowercase letter in country code.
                yield return new object[] { "ABCDbEBB" };
                yield return new object[] { "ABCDBeBB" };
                // Short form: lowercase letter in suffix.
                yield return new object[] { "ABCDBEbB" };
                yield return new object[] { "ABCDBEBb" };
                // Short form: digits in country code.
                yield return new object[] { "ABCD1EBB" };
                yield return new object[] { "ABCDB1BB" };
                // Long form: lowercase letter in branch code.
                yield return new object[] { "ABCDBEBBxXX" };
                yield return new object[] { "ABCDBEBBXxX" };
                yield return new object[] { "ABCDBEBBXXx" };
            }
        }

        public static IEnumerable<object[]> ValidSwiftValues {
            get {
                // Short form.
                yield return new object[] { "ABCDBEBB" };
                // Short form: digits in suffix.
                yield return new object[] { "ABCDBE1B" };
                yield return new object[] { "ABCDBE11" };
                yield return new object[] { "ABCDBEB1" };
                // Long form.
                yield return new object[] { "ABCDBEBBXXX" };
                // Long form: digit in suffix.
                yield return new object[] { "ABCDBE1BXXX" };
                yield return new object[] { "ABCDBE11XXX" };
                yield return new object[] { "ABCDBEB1XXX" };
                // Long form: digit in branch code.
                yield return new object[] { "ABCDBEBB1XX" };
                yield return new object[] { "ABCDBEBB11X" };
                yield return new object[] { "ABCDBEBB111" };
                yield return new object[] { "ABCDBEBBX1X" };
                yield return new object[] { "ABCDBEBBX11" };
                yield return new object[] { "ABCDBEBBXX1" };
            }
        }

        public static IEnumerable<object[]> InvalidSwiftValues {
            get {
                // Short form: digits in prefix.
                yield return new object[] { "1BCDBEBB" };
                yield return new object[] { "11CDBEBB" };
                yield return new object[] { "111DBEBB" };
                yield return new object[] { "1111BEBB" };
                yield return new object[] { "A1CDBEBB" };
                yield return new object[] { "A11DBEBB" };
                yield return new object[] { "A111BEBB" };
                yield return new object[] { "AB1DBEBB" };
                yield return new object[] { "AB11BEBB" };
                yield return new object[] { "ABC1BEBB" };
                // Long form: digit in prefix.
                yield return new object[] { "1BCDBEBBXXX" };
                yield return new object[] { "11CDBEBBXXX" };
                yield return new object[] { "111DBEBBXXX" };
                yield return new object[] { "1111BEBBXXX" };
                yield return new object[] { "A1CDBEBBXXX" };
                yield return new object[] { "A11DBEBBXXX" };
                yield return new object[] { "A111BEBBXXX" };
                yield return new object[] { "AB1DBEBBXXX" };
                yield return new object[] { "AB11BEBBXXX" };
                yield return new object[] { "ABC1BEBBXXX" };
                // Short form: lowercase letter in prefix.
                yield return new object[] { "aBCDBEBB" };
                yield return new object[] { "AbCDBEBB" };
                yield return new object[] { "ABcDBEBB" };
                yield return new object[] { "ABCdBEBB" };
                // Short form: lowercase letter in country code.
                yield return new object[] { "ABCDbEBB" };
                yield return new object[] { "ABCDBeBB" };
                // Short form: lowercase letter in suffix.
                yield return new object[] { "ABCDBEbB" };
                yield return new object[] { "ABCDBEBb" };
                // Short form: digits in country code.
                yield return new object[] { "ABCD1EBB" };
                yield return new object[] { "ABCDB1BB" };
                // Long form: lowercase letter in branch code.
                yield return new object[] { "ABCDBEBBxXX" };
                yield return new object[] { "ABCDBEBBXxX" };
                yield return new object[] { "ABCDBEBBXXx" };
            }
        }

        public static IEnumerable<object[]> InvalidValues {
            get {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "12" };
                yield return new object[] { "123" };
                yield return new object[] { "1234" };
                yield return new object[] { "12345" };
                yield return new object[] { "123456" };
                yield return new object[] { "1234567" };
                yield return new object[] { "123456789" };
                yield return new object[] { "1234567890" };
                yield return new object[] { "123456789012" };
            }
        }

        public static IEnumerable<object[]> InvalidBranchCodes {
            get {
                yield return new object[] { "1" };
                yield return new object[] { "12" };
                yield return new object[] { "1234" };
            }
        }

        public static IEnumerable<object[]> InvalidCountryCodes {
            get {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "123" };
            }
        }

        public static IEnumerable<object[]> InvalidInstitutionCodes {
            get {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "12" };
                yield return new object[] { "123" };
                yield return new object[] { "12345" };
            }
        }

        public static IEnumerable<object[]> InvalidLocationCodes {
            get {
                yield return new object[] { "" };
                yield return new object[] { "1" };
                yield return new object[] { "123" };
            }
        }
    }
}
