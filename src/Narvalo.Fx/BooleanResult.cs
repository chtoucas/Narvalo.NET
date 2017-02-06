// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [DebuggerDisplay("True")]
    public partial class BooleanResult
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly BooleanResult s_True = new BooleanResult(true);

        private BooleanResult(bool isTrue)
        {
            IsTrue = isTrue;
        }

        public static BooleanResult True
        {
            get { Warrant.NotNull<BooleanResult>(); return s_True; }
        }

        public bool IsFalse => !IsTrue;

        public bool IsTrue { get; }

        public virtual string ErrorMessage
        {
            get { throw new InvalidOperationException("XXX"); }
        }

        #region Operators

        public static implicit operator bool(BooleanResult value) => value == null ? false : value.IsTrue;

        //public static bool operator true(BooleanResult value) => value != null && value.IsTrue;

        //public static bool operator false(BooleanResult value) => value == null || value.IsFalse;

        //public static BooleanResult operator !(BooleanResult value)
        //    => new BooleanResult(!value.IsTrue);

        //public static BooleanResult operator &(BooleanResult left, BooleanResult right)
        //    => new BooleanResult(left.IsTrue && right.IsTrue);

        //public static BooleanResult operator |(BooleanResult left, BooleanResult right)
        //    => new BooleanResult(left.IsTrue || right.IsTrue);

        #endregion

        public static BooleanResult False(string message)
        {
            Require.NotNull(message, nameof(message));

            return new False_(message);
        }

        public bool ToBoolean() => IsTrue;

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

            public False_(string message) : base(false)
            {
                Demand.NotNull(message);

                _message = message;
            }

            public override string ErrorMessage
            {
                get { Warrant.NotNull<string>(); return _message; }
            }

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("False({0})", _message);
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

                public string ErrorMessage => _inner.ErrorMessage;
            }
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
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
                Contract.Invariant(!IsTrue);
            }
        }
    }
}

#endif

