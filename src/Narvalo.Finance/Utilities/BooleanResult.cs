﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerDisplay("True")]
    public class BooleanResult
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly BooleanResult s_True = new BooleanResult();

        private BooleanResult() { }

        private BooleanResult(bool isFalse)
        {
            IsFalse = isFalse;
        }

        public static BooleanResult True
        {
            get { Warrant.NotNull<BooleanResult>(); return s_True; }
        }

        public bool IsFalse { get; }

        public bool IsTrue => !IsFalse;

        public virtual string Message
        {
            get { throw new InvalidOperationException(); }
        }

        public static BooleanResult False(string message)
        {
            Require.NotNullOrEmpty(message, nameof(message));

            return new BooleanResult.False_(message);
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return "True";
        }

        [DebuggerDisplay("False")]
        [DebuggerTypeProxy(typeof(False_.DebugView))]
        private sealed partial class False_ : BooleanResult
        {
            private readonly string _message;

            public False_(string message) : base(true)
            {
                Demand.NotNull(message);

                _message = message;
            }

            public override string Message
            {
                get { Warrant.NotNull<string>(); return _message; }
            }

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Invariant("False({0})", _message);
            }

            /// <summary>
            /// Represents a debugger type proxy for <see cref="BooleanResult.False_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView
            {
                private readonly False_ _inner;

                public DebugView(False_ inner)
                {
                    _inner = inner;
                }

                public string Message => _inner.Message;
            }
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance.Text
{
    using System.Diagnostics.Contracts;

    public partial class BooleanResult
    {
        private sealed partial class False_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_message != null);
                Contract.Invariant(IsFalse);
            }
        }
    }
}

#endif
