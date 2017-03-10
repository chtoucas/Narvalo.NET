// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public sealed class DemandFacts : IClassFixture<DebugAssertFixture>
    {
        #region State()

        [Fact]
        public static void State_Passes_ForTrue() => Demand.State(true);

        [ReleaseOnlyFact]
        public static void State_Passes_ForFalse() => Demand.State(false);

        [DebugOnlyFact]
        public void State_Fails_ForFalse()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.State(false));

        #endregion

        #region True()

        [Fact]
        public static void True_Passes_ForTrue() => Demand.True(true);

        [ReleaseOnlyFact]
        public static void True_Passes_ForFalse() => Demand.True(false);

        [DebugOnlyFact]
        public void True_Fails_ForFalse()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.True(false));

        #endregion

        #region Range()

        [Fact]
        public static void Range_Passes_ForTrue() => Demand.Range(true);

        [ReleaseOnlyFact]
        public static void Range_Passes_ForFalse() => Demand.Range(false);

        [DebugOnlyFact]
        public void Range_Fails_ForFalse()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.Range(false));

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_Passes_ForNonNull() => Demand.NotNull(new Object());

        [ReleaseOnlyFact]
        public static void NotNull_Passes_ForNull()
        {
            Object obj = null;
            Demand.NotNull(obj);
        }

        [DebugOnlyFact]
        public void NotNull_Fails_ForNull()
        {
            Object obj = null;
            Assert.Throws<DebugAssertFailedException>(() => Demand.NotNull(obj));
        }

        #endregion

        #region NotNullUnconstrained()

        [Fact]
        public static void NotNullUnconstrained_Passes_ForStruct()
            => Demand.NotNullUnconstrained(new My.EmptyStruct());

        [Fact]
        public static void NotNullUnconstrained_Passes_ForNonNull()
            => Demand.NotNullUnconstrained(new Object());

        [ReleaseOnlyFact]
        public static void NotNullUnconstrained_Passes_ForNull()
        {
            Object obj = null;
            Demand.NotNullUnconstrained(obj);
        }

        [DebugOnlyFact]
        public void NotNullUnconstrained_Fails_ForNull()
        {
            Object obj = null;
            Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullUnconstrained(obj));
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_Passes_ForNonNullOrEmptyString()
            => Demand.NotNullOrEmpty("value");

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_Passes_ForNullString() => Demand.NotNullOrEmpty(null);

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_Passes_ForEmptyString() => Demand.NotNullOrEmpty(String.Empty);

        [DebugOnlyFact]
        public void NotNullOrEmpty_Fails_ForNullString()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullOrEmpty(null));

        [DebugOnlyFact]
        public void NotNullOrEmpty_Fails_ForEmptyString()
            => Assert.Throws<DebugAssertFailedException>(() => Demand.NotNullOrEmpty(String.Empty));

        #endregion
    }
}
