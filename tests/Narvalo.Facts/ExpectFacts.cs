// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class ExpectFacts
    {
        #region State()

        [Fact]
        public static void State_Passes_ForTrue() => Expect.State(true);

        [Fact]
        public static void State_Passes_ForFalse() => Expect.State(false);

        #endregion

        #region True()

        [Fact]
        public static void True_Passes_ForTrue() => Expect.True(true);

        [Fact]
        public static void True_Passes_ForFalse() => Expect.True(false);

        #endregion

        #region Range()

        [Fact]
        public static void Range_Passes_ForTrue() => Expect.Range(true);

        [Fact]
        public static void Range_Passes_ForFalse() => Expect.Range(false);

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_Passes_ForNonNull() => Expect.NotNull(new Object());

        [Fact]
        public static void NotNull_Passes_ForNull()
        {
            Expect.NotNull((Object)null);
        }

        #endregion

        #region NotNullUnconstrained()

        [Fact]
        public static void NotNullUnconstrained_Passes_ForStruct()
            => Expect.NotNullUnconstrained(new My.EmptyStruct());

        [Fact]
        public static void NotNullUnconstrained_Passes_ForNonNull()
            => Expect.NotNullUnconstrained(new Object());

        [Fact]
        public static void NotNullUnconstrained_Passes_ForNull()
        {
            Expect.NotNull((Object)null);
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_Passes_ForNonNullOrEmptyString()
            => Expect.NotNullOrEmpty("value");

        [Fact]
        public static void NotNullOrEmpty_Passes_ForNullOrEmptyString()
        {
            Expect.NotNullOrEmpty(null);
            Expect.NotNullOrEmpty(String.Empty);
        }

        #endregion
    }
}
