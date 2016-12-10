// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class DemandFacts
    {
        #region State()

        [Fact]
        public static void State_Passes_ForTrue() => Demand.State(true);

        [DebugOnlyFact]
        public static void State_Fails_ForFalse()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.State(false));

        [ReleaseOnlyFact]
        public static void State_Passes_ForFalse() => Demand.State(false);

        #endregion

        #region True()

        [Fact]
        public static void True_Passes_ForTrue() => Demand.True(true);

        [DebugOnlyFact]
        public static void True_Fails_ForFalse()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.State(false));

        [ReleaseOnlyFact]
        public static void True_Passes_ForFalse() => Demand.State(false);

        #endregion

        #region Range()

        [Fact]
        public static void Range_Passes_ForTrue() => Demand.Range(true);

        [DebugOnlyFact]
        public static void Range_Fails_ForFalse()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.Range(false));

        [ReleaseOnlyFact]
        public static void Range_Passes_ForFalse() => Demand.Range(false);

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_Passes_ForNonNull() => Demand.NotNull(new Object());

        [DebugOnlyFact]
        public static void NotNull_Fails_ForNull()
        {
            Object obj = null;
            Assert.Throws<DebugAssertFailedException>(() => Demand.NotNull(obj));
        }

        [ReleaseOnlyFact]
        public static void NotNull_Passes_ForNull()
        {
            Object obj = null;
            Demand.NotNull(obj);
        }

        #endregion

        #region NotNullUnconstrained()

        [Fact]
        public static void NotNullUnconstrained_Passes_ForStruct()
            => Demand.NotNullUnconstrained(new My.EmptyStruct());

        [Fact]
        public static void NotNullUnconstrained_Passes_ForNonNull()
            => Demand.NotNullUnconstrained(new Object());

        [DebugOnlyFact]
        public static void NotNullUnconstrained_Fails_ForNull()
        {
            Object obj = null;
            Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullUnconstrained(obj));
        }

        [ReleaseOnlyFact]
        public static void NotNullUnconstrained_Passes_ForNull()
        {
            Object obj = null;
            Demand.NotNullUnconstrained(obj);
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_Passes_ForNonNullorEmptyString()
            => Demand.NotNullOrEmpty("value");

        [DebugOnlyFact]
        public static void NotNullOrEmpty_Fails_ForNullString()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullOrEmpty(null));

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_Passes_ForNullString() => Demand.NotNullOrEmpty(null);

        [DebugOnlyFact]
        public static void NotNullOrEmpty_Fails_ForEmptyString()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullOrEmpty(String.Empty));

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_Passes_ForEmptyString() => Demand.NotNullOrEmpty(String.Empty);

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_Passes_ForNonNullOrWhiteSpace()
            => Demand.NotNullOrWhiteSpace("value");

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Fails_ForNullString()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullOrWhiteSpace(null));

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_Passes_ForNullString() => Demand.NotNullOrWhiteSpace(null);

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Fails_ForEmptyString()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullOrWhiteSpace(String.Empty));

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_Passes_ForEmptyString() => Demand.NotNullOrWhiteSpace(String.Empty);

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Fails_ForWhiteSpaceOnlyString()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullOrWhiteSpace(My.WhiteSpaceOnlyString));

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_Passes_ForWhiteSpaceOnlyString()
            => Demand.NotNullOrWhiteSpace(My.WhiteSpaceOnlyString);

        #endregion
    }
}
