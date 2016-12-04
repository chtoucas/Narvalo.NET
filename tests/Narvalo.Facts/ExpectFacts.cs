// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class ExpectFacts
    {
        #region State()

        [Fact]
        public static void State_DoesNothing()
        {
            Expect.State(true);

            Expect.State(false);
        }

        #endregion

        #region True()

        [Fact]
        public static void True_DoesNothing()
        {
            Expect.True(true);

            Expect.True(false);
        }

        #endregion

        #region Range()

        [Fact]
        public static void Range_DoesNothing()
        {
            Expect.Range(true);

            Expect.Range(false);
        }

        #endregion

        #region NotNull()

        [Fact]
        public static void NotNull_DoesNothing()
        {
            Object obj = null;
            Expect.NotNull(obj);

            Expect.NotNull(new Object());
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNothing()
        {
            Expect.NotNullOrEmpty(null);
            Expect.NotNullOrEmpty(String.Empty);

            Expect.NotNullOrEmpty("value");
        }

        #endregion

        #region Object()

        [Fact]
        public static void Object_DoesNothing()
        {
            Object obj = null;
            Expect.Object(obj);

            Expect.Object("this");
        }

        #endregion

        #region Property()

        [Fact]
        public static void Property_DoesNothing()
        {
            Object obj = null;
            Expect.Property(obj);

            Expect.Property("value");
        }

        #endregion

        #region PropertyNotEmpty()

        [Fact]
        public static void PropertyNotEmpty_DoesNothing()
        {
            Expect.PropertyNotEmpty(null);
            Expect.PropertyNotEmpty(String.Empty);

            Expect.PropertyNotEmpty("value");
        }

        #endregion
    }
}
