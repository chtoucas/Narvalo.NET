// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using Xunit;

    public static class AcknowledgeFacts
    {
        #region Object()

        [Fact]
        public static void Object_DoesNotThrow_ForNull()
        {
            // Act
            Acknowledge.Object((string)null);
        }

        [Fact]
        public static void Object_DoesNotThrow_ForNonNull()
        {
            // Act
            Acknowledge.Object("this");
        }

        #endregion
    }
}
