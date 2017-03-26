// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo {
    using Xunit;

    public static class ControlFlowExceptionFacts {
        internal sealed class factAttribute : FactAttribute_ {
            public factAttribute(string message) : base(nameof(ControlFlowException), message) { }
        }

        [fact("Default ctor sets a default message.")]
        public static void Ctor_default() {
            var ex = new ControlFlowException();

            Assert.NotNull(ex.Message);
        }

        [fact("Ctor sets message.")]
        public static void Ctor_message() {
            var message = "My message";
            var ex = new ControlFlowException(message);

            Assert.Equal(message, ex.Message);
        }

        [fact("Ctor sets message and inner exception.")]
        public static void Ctor_inner_exception() {
            var message = "My message";
            var innerException = new My.SimpleException();
            var ex = new ControlFlowException(message, innerException);

            Assert.Equal(message, ex.Message);
            Assert.Same(innerException, ex.InnerException);
        }
    }
}
