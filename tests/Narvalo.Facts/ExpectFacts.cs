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
            Object obj = null;
            Expect.NotNull(obj);
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

        #region Object()

        [Fact]
        public static void Object_Passes_ForNonNull() => Expect.Object(new Object());

        [Fact]
        public static void Object_Passes_ForNull()
        {
            Object obj = null;
            Expect.Object(obj);
        }

        #endregion

        #region Property()

        [Fact]
        public static void Property_Passes_ForTrue() => Expect.Property(true);

        [Fact]
        public static void Property_Passes_ForFalse() => Expect.Property(false);

        #endregion

        #region Property<T>()

        [Fact]
        public static void PropertyT_Passes_ForNonNull() => Expect.Property(new Object());

        [Fact]
        public static void PropertyT_Passes_ForNull()
        {
            Object obj = null;
            Expect.Property(obj);
        }

        #endregion

        #region PropertyNotEmpty()

        [Fact]
        public static void PropertyNotEmpty_Passes_ForNonNullOrEmptyString()
            => Expect.PropertyNotEmpty("value");

        [Fact]
        public static void PropertyNotEmpty_Passes_ForNullOrEmptyString()
        {
            Expect.PropertyNotEmpty(null);
            Expect.PropertyNotEmpty(String.Empty);
        }

        #endregion
    }
}
