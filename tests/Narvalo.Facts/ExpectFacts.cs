// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class ExpectFacts
    {
        #region Object()

        [Fact]
        public static void Object_DoesNotThrow_ForNull()
        {
            // Act
            Expect.Object((string)null);
        }

        [Fact]
        public static void Object_DoesNotThrow_ForNonNull()
        {
            // Act
            Expect.Object("this");
        }

        #endregion

        #region NotNullOrEmpty()

        [Fact]
        public static void NotNullOrEmpty_DoesNothing()
        {
            // Act
            Expect.NotNullOrEmpty(null);
            Expect.NotNullOrEmpty(String.Empty);
            Expect.NotNullOrEmpty("value");
        }

        #endregion

        #region Object()

        [Fact]
        public static void Object_DoesNothing()
        {
            // Act
            Expect.Object((string)null);
            Expect.Object("value");
        }

        #endregion
    }
}
