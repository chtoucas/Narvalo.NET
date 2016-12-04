// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Xunit;

    public static class ControlFlowExceptionFacts
    {
        #region Ctor

        [Fact]
        public static void Ctor_DefaultConstructor()
        {
            var ex = new ControlFlowException();

            Assert.NotNull(ex.Message);
        }

        [Fact]
        public static void Ctor_MessageConstructor()
        {
            var message = "My message";
            var ex = new ControlFlowException(message);

            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public static void Ctor_MessageInnerExceptionConstructor()
        {
            var message = "My message";
            var innerException = new Exception();
            var ex = new ControlFlowException(message, innerException);

            Assert.Equal(message, ex.Message);
            Assert.Same(innerException, ex.InnerException);
        }

        #endregion
    }
}
