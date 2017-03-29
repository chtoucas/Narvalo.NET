// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Resources;

    using Xunit;

    public partial class AssertExtended : Assert {
        public static void Fail() => Assert.True(false);

        public static void DoesNotThrow(Action testCode) {
            var ex = Record.Exception(testCode);

            Assert.Null(ex);
        }

        public static T DoesNotThrow<T>(Func<T> testCode) {
            T result = default(T);
            var ex = Record.Exception(() => result = testCode());

            Assert.Null(ex);

            return result;
        }

        public static void ThrowsOnNext<T>(IEnumerable<T> seq) {
            using (var iter = seq.GetEnumerator()) {
                Assert.Throws<InvalidOperationException>(() => iter.MoveNext());
            }
        }

        public static void ThrowsAfter<T>(IEnumerable<T> seq, int count) {
            int i = 0;
            using (var iter = seq.GetEnumerator()) {
                while (i < count) { Assert.True(iter.MoveNext()); i++; }
                Assert.Throws<InvalidOperationException>(() => iter.MoveNext());
            }
        }

        public static void CalledOnNext<T>(IEnumerable<T> seq, ref bool notCalled) {
            using (var iter = seq.GetEnumerator()) {
                iter.MoveNext();
                Assert.False(notCalled);
            }
        }

        public static void CalledAfter<T>(IEnumerable<T> seq, int count, ref bool notCalled) {
            int i = 0;
            using (var iter = seq.GetEnumerator()) {
                while (i < count) { Assert.True(iter.MoveNext()); i++; }
                iter.MoveNext();
                Assert.False(notCalled);
            }
        }

        public static void IsNotLocalized(LocalizedStrings localizedStrings) {
            var dict = localizedStrings.GetStrings();

            Assert.Null(dict);
        }

        public static void IsLocalized(ResourceManager manager)
            => IsLocalized(new LocalizedStrings(manager));

        public static void IsLocalized(LocalizedStrings localizedStrings) {
            var dict = localizedStrings.GetStrings();

            Assert.NotNull(dict);

            Assert.All(dict, pair =>
                Assert.True(!String.IsNullOrWhiteSpace(pair.Value),
                       $"The resource '{pair.Key}' is empty or contains only white spaces.")
            );

            Assert.All(dict, pair =>
                Assert.True(pair.Value != "XXX",
                    $"The key '{pair.Key}' contains a temporary string in {CultureInfo.CurrentCulture.EnglishName}.")
            );
        }

        public static void IsLocalizationComplete(LocalizedStrings localizedStrings) {
            var dict = localizedStrings.GetStrings();
            if (dict == null) {
                Assert.True(false, $"No localized strings found in {CultureInfo.CurrentCulture.EnglishName}.");
                return;
            }

            var keys = localizedStrings.ReferenceKeys;
            if (keys == null) {
                Assert.True(false, $"Unable to load the reference keys.");
                return;
            }

            foreach (var pair in dict) {
                Assert.True(keys.Contains(pair.Key),
                    $"The resource contains an unrecognized key '{pair.Key}' in {CultureInfo.CurrentCulture.EnglishName}.");
            }

            foreach (var key in keys) {
                Assert.True(dict.ContainsKey(key),
                    $"The key '{key}' does not exist in {CultureInfo.CurrentCulture.EnglishName}.");
            }
        }
    }
}
