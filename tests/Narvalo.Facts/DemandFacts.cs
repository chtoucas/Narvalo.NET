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
            => Assert.Throws<AssertFailedException>(() => Demand.State(false));

        [ReleaseOnlyFact]
        public static void State_Passes_ForFalse() => Demand.State(false);

        #endregion

        #region True()

        [Fact]
        public static void True_Passes_ForTrue() => Demand.True(true);

        [DebugOnlyFact]
        public static void True_Fails_ForFalse()
            => Assert.Throws<AssertFailedException>(() => Demand.State(false));

        [ReleaseOnlyFact]
        public static void True_Passes_ForFalse() => Demand.State(false);

        #endregion

        #region Range()

        [Fact]
        public static void Range_Passes_ForTrue() => Demand.Range(true);

        [DebugOnlyFact]
        public static void Range_Fails_ForFalse()
            => Assert.Throws<AssertFailedException>(() => Demand.Range(false));

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
            Assert.Throws<AssertFailedException>(() => Demand.NotNull(obj));
        }

        [ReleaseOnlyFact]
        public static void NotNull_Passes_ForNull()
        {
            Object obj = null;
            Demand.NotNull(obj);
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_Passes_ForNonNullorEmptyString()
            => Demand.NotNullOrEmpty("value");

        [DebugOnlyFact]
        public static void NotNullOrEmpty_Fails_ForNullString()
            => Assert.Throws<AssertFailedException>(() => Demand.NotNullOrEmpty(null));

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_Passes_ForNullString() => Demand.NotNullOrEmpty(null);

        [DebugOnlyFact]
        public static void NotNullOrEmpty_Fails_ForEmptyString()
            => Assert.Throws<AssertFailedException>(() => Demand.NotNullOrEmpty(String.Empty));

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_Passes_ForEmptyString() => Demand.NotNullOrEmpty(String.Empty);

        #endregion

        #region Object()

        [Fact]
        public static void Object_Passes_ForNonNull() => Demand.Object(new Object());

        [DebugOnlyFact]
        public static void Object_Fails_ForNull()
        {
            Object obj = null;
            Assert.Throws<AssertFailedException>(() => Demand.Object(obj));
        }

        [ReleaseOnlyFact]
        public static void Object_Passes_ForNull()
        {
            Object obj = null;
            Demand.Object(obj);
        }

        #endregion

        #region Property()

        [Fact]
        public static void Property_Passes_ForTrue() => Demand.Property(true);

        [DebugOnlyFact]
        public static void Property_Fails_ForFalse()
            => Assert.Throws<AssertFailedException>(() => Demand.Property(false));

        [ReleaseOnlyFact]
        public static void Property_Passes_ForFalse() => Demand.Property(false);

        #endregion

        #region Property<T>()

        [Fact]
        public static void PropertyT_Passes_ForNonNull() => Demand.Property(new Object());

        [DebugOnlyFact]
        public static void PropertyT_Fails_ForNull()
        {
            Object obj = null;
            Assert.Throws<AssertFailedException>(() => Demand.Property(obj));
        }

        [ReleaseOnlyFact]
        public static void PropertyT_Passes_ForNull()
        {
            Object obj = null;
            Demand.Property(obj);
        }

        #endregion

        #region PropertyNotEmpty()

        [Fact]
        public static void PropertyNotEmptyy_Passes_ForNonNullorEmptyString()
            => Demand.PropertyNotEmpty("value");

        [DebugOnlyFact]
        public static void PropertyNotEmpty_Fails_ForNullString()
            => Assert.Throws<AssertFailedException>(() => Demand.PropertyNotEmpty(null));

        [DebugOnlyFact]
        public static void PropertyNotEmpty_Fails_ForEmptyString()
            => Assert.Throws<AssertFailedException>(() => Demand.PropertyNotEmpty(String.Empty));

        [ReleaseOnlyFact]
        public static void PropertyNotEmpty_Passes_ForNullString()
            => Demand.PropertyNotEmpty(null);

        [ReleaseOnlyFact]
        public static void PropertyNotEmpty_Passes_ForEmptyString()
            => Demand.PropertyNotEmpty(String.Empty);

        #endregion
    }
}
