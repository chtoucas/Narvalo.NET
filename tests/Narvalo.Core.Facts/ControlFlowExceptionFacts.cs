// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using Xunit;

    public static class ControlFlowExceptionFacts {
        internal sealed class tAttribute : TestAttribute {
            public tAttribute(string message) : base(nameof(ControlFlowException), message) { }
        }

        [t("Default ctor sets a default message.")]
        public static void Ctor1() {
            var ex = new ControlFlowException();

            Assert.NotNull(ex.Message);
        }

        [t("Ctor sets message.")]
        public static void Ctor2() {
            var message = "My message";
            var ex = new ControlFlowException(message);

            Assert.Equal(message, ex.Message);
        }

        [t("Ctor sets message and inner exception.")]
        public static void Ctor3() {
            var message = "My message";
            var innerException = new My.SimpleException();
            var ex = new ControlFlowException(message, innerException);

            Assert.Equal(message, ex.Message);
            Assert.Same(innerException, ex.InnerException);
        }
    }
}
