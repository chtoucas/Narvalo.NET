// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;

    using Narvalo.Finance.Properties;

    [DebuggerDisplay("True")]
    public partial class BooleanResult
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly BooleanResult s_True = new BooleanResult(false);

        private BooleanResult(bool isFalse)
        {
            IsFalse = isFalse;
        }

        internal static BooleanResult True => s_True;

        public bool IsFalse { get; }

        public bool IsTrue => !IsFalse;

        public virtual string Error
        {
            get { throw new InvalidOperationException(Strings.BooleanResult_NoErrorMessage); }
        }

        public static implicit operator bool(BooleanResult value) => value == null ? false : value.IsTrue;

        internal static BooleanResult False(string message)
        {
            Require.NotNull(message, nameof(message));

            return new False_(message);
        }

        public bool ToBoolean() => IsTrue;

        public override string ToString() => "True";

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

            public override string Error => _message;

            public override string ToString() => Format.Current("False({0})", _message);

            /// <summary>
            /// Represents a debugger type proxy for <see cref="BooleanResult.False_"/>.
            /// </summary>
            [ExcludeFromCodeCoverage]
            private sealed class DebugView
            {
                private readonly False_ _inner;

                public DebugView(False_ inner)
                {
                    _inner = inner;
                }

                public string ErrorMessage => _inner.Error;
            }
        }
    }
}
