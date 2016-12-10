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
            Assert.Throws<AssertFailedException>(() => Demand.NotNullUnconstrained(obj));
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
            => Assert.Throws<AssertFailedException>(() => Demand.NotNullOrEmpty(null));

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_Passes_ForNullString() => Demand.NotNullOrEmpty(null);

        [DebugOnlyFact]
        public static void NotNullOrEmpty_Fails_ForEmptyString()
            => Assert.Throws<AssertFailedException>(() => Demand.NotNullOrEmpty(String.Empty));

        [ReleaseOnlyFact]
        public static void NotNullOrEmpty_Passes_ForEmptyString() => Demand.NotNullOrEmpty(String.Empty);

        #endregion

        #region NotNullOrWhiteSpace()

        [Fact]
        public static void NotNullOrWhiteSpace_Passes_ForNonNullOrWhiteSpace()
            => Demand.NotNullOrWhiteSpace("value");

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Fails_ForNullString()
            => Assert.Throws<AssertFailedException>(() => Demand.NotNullOrWhiteSpace(null));

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_Passes_ForNullString() => Demand.NotNullOrWhiteSpace(null);

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Fails_ForEmptyString()
            => Assert.Throws<AssertFailedException>(() => Demand.NotNullOrWhiteSpace(String.Empty));

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_Passes_ForEmptyString() => Demand.NotNullOrWhiteSpace(String.Empty);

        [DebugOnlyFact]
        public static void NotNullOrWhiteSpace_Fails_ForWhiteSpaceOnlyString()
            => Assert.Throws<AssertFailedException>(() => Demand.NotNullOrWhiteSpace(My.WhiteSpaceOnlyString));

        [ReleaseOnlyFact]
        public static void NotNullOrWhiteSpace_Passes_ForWhiteSpaceOnlyString()
            => Demand.NotNullOrWhiteSpace(My.WhiteSpaceOnlyString);

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

        #region ObjectNotNull()

        [Fact]
        public static void ObjectNotNull_Passes_ForStruct() => Demand.ObjectNotNull(new My.EmptyStruct());

        [Fact]
        public static void ObjectNotNull_Passes_ForNonNull() => Demand.ObjectNotNull(new Object());

        [DebugOnlyFact]
        public static void ObjectNotNull_Fails_ForNull()
        {
            Object obj = null;
            Assert.Throws<AssertFailedException>(() => Demand.ObjectNotNull(obj));
        }

        [ReleaseOnlyFact]
        public static void ObjectNotNull_Passes_ForNull()
        {
            Object obj = null;
            Demand.ObjectNotNull(obj);
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

        #region PropertyNotNull<T>()

        [Fact]
        public static void PropertyNotNull_Passes_ForStruct() => Demand.PropertyNotNull(new My.EmptyStruct());

        [Fact]
        public static void PropertyNotNull_Passes_ForNonNull() => Demand.PropertyNotNull(new Object());

        [DebugOnlyFact]
        public static void PropertyNotNull_Fails_ForNull()
        {
            Object obj = null;
            Assert.Throws<AssertFailedException>(() => Demand.PropertyNotNull(obj));
        }

        [ReleaseOnlyFact]
        public static void PropertyNotNull_Passes_ForNull()
        {
            Object obj = null;
            Demand.PropertyNotNull(obj);
        }

        #endregion

        #region PropertyNotNullOrEmpty()

        [Fact]
        public static void PropertyNotNullOrEmpty_Passes_ForNonNullorEmptyString()
            => Demand.PropertyNotNullOrEmpty("value");

        [DebugOnlyFact]
        public static void PropertyNotNullOrEmpty_Fails_ForNullString()
            => Assert.Throws<AssertFailedException>(() => Demand.PropertyNotNullOrEmpty(null));

        [ReleaseOnlyFact]
        public static void PropertyNotNullOrEmpty_Passes_ForNullString()
            => Demand.PropertyNotNullOrEmpty(null);

        [DebugOnlyFact]
        public static void PropertyNotNullOrEmpty_Fails_ForEmptyString()
            => Assert.Throws<AssertFailedException>(() => Demand.PropertyNotNullOrEmpty(String.Empty));

        [ReleaseOnlyFact]
        public static void PropertyNotNullOrEmpty_Passes_ForEmptyString()
            => Demand.PropertyNotNullOrEmpty(String.Empty);

        #endregion

        #region PropertyNotNullOrWhiteSpace()

        [Fact]
        public static void PropertyNotNullOrWhiteSpace_Passes_ForNonNullorEmptyString()
            => Demand.PropertyNotNullOrWhiteSpace("value");

        [DebugOnlyFact]
        public static void PropertyNotNullOrWhiteSpace_Fails_ForNullString()
            => Assert.Throws<AssertFailedException>(() => Demand.PropertyNotNullOrWhiteSpace(null));

        [ReleaseOnlyFact]
        public static void PropertyNotNullOrWhiteSpace_Passes_ForNullString()
            => Demand.PropertyNotNullOrWhiteSpace(null);

        [DebugOnlyFact]
        public static void PropertyNotNullOrWhiteSpace_Fails_ForEmptyString()
            => Assert.Throws<AssertFailedException>(() => Demand.PropertyNotNullOrWhiteSpace(String.Empty));

        [ReleaseOnlyFact]
        public static void PropertyNotNullOrWhiteSpace_Passes_ForEmptyString()
            => Demand.PropertyNotNullOrWhiteSpace(String.Empty);

        [DebugOnlyFact]
        public static void PropertyNotNullOrWhiteSpace_Fails_ForWhiteSpaceOnlyString()
            => Assert.Throws<AssertFailedException>(()
                => Demand.PropertyNotNullOrWhiteSpace(My.WhiteSpaceOnlyString));

        [ReleaseOnlyFact]
        public static void PropertyNotNullOrWhiteSpace_Passes_ForWhiteSpaceOnlyString()
            => Demand.PropertyNotNullOrWhiteSpace(My.WhiteSpaceOnlyString);

        #endregion
    }
}
