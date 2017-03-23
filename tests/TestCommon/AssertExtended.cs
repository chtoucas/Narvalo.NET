// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Resources;

    using Xunit;

    public partial class AssertExtended : Assert
    {
        /// <summary>
        /// Check that the given function uses deferred execution.
        /// A "spiked" source is given to the function: the function
        /// call itself shouldn't throw an exception. However, using
        /// the result (by calling GetEnumerator() then MoveNext() on it) *should*
        /// throw InvalidOperationException.
        /// </summary>
        // Adapted from https://raw.githubusercontent.com/jskeet/edulinq/master/src/Edulinq.TestSupport/ThrowingEnumerable.cs
        public static void IsDeferred<T>(Func<IEnumerable<int>, IEnumerable<T>> deferredFunction)
        {
            var result = deferredFunction(new ThrowingEnumerable());

            using (var iter = result.GetEnumerator())
            {
                Assert.Throws<InvalidOperationException>(() => iter.MoveNext());
            }
        }

        public static void IsNotLocalized(LocalizedStrings localizedStrings)
        {
            var dict = localizedStrings.GetStrings();

            Assert.Null(dict);
        }

        public static void IsLocalized(ResourceManager manager)
            => IsLocalized(new LocalizedStrings(manager));

        public static void IsLocalized(LocalizedStrings localizedStrings)
        {
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

        public static void IsLocalizationComplete(LocalizedStrings localizedStrings)
        {
            var dict = localizedStrings.GetStrings();
            if (dict == null)
            {
                Assert.True(false, $"No localized strings found in {CultureInfo.CurrentCulture.EnglishName}.");
                return;
            }

            var keys = localizedStrings.ReferenceKeys;
            if (keys == null)
            {
                Assert.True(false, $"Unable to load the reference keys.");
                return;
            }

            foreach (var pair in dict)
            {
                Assert.True(keys.Contains(pair.Key),
                    $"The resource contains an unrecognized key '{pair.Key}' in {CultureInfo.CurrentCulture.EnglishName}.");
            }

            foreach (var key in keys)
            {
                Assert.True(dict.ContainsKey(key),
                    $"The key '{key}' does not exist in {CultureInfo.CurrentCulture.EnglishName}.");
            }
        }
    }
}
