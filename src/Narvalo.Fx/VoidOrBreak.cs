// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx.Properties;

    using static System.Diagnostics.Contracts.Contract;

    /// <seealso cref="Outcome{T}"/>
    /// <seealso cref="Either{T1, T2}"/>
    /// <seealso cref="Switch{T1, T2}"/>
    /// <seealso cref="VoidOrError"/>
    [DebuggerDisplay(@"""Void""")]
    public class VoidOrBreak
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOrBreak s_Void = new VoidOrBreak();

        private readonly bool _isBreak;

        private VoidOrBreak() { }

        private VoidOrBreak(bool isBreak)
        {
            _isBreak = isBreak;
        }

        public static VoidOrBreak Void
        {
            get
            {
                Ensures(Result<VoidOrBreak>() != null);

                return s_Void;
            }
        }

        public bool IsBreak { get { return _isBreak; } }

        public virtual string Reason
        {
            get
            {
                throw new InvalidOperationException(Strings.VoidOrBreak_BreakHasNoReason);
            }
        }

        public static VoidOrBreak Break(string message)
        {
            Require.NotNullOrEmpty(message, nameof(message));
            Ensures(Result<VoidOrBreak>() != null);

            return new VoidOrBreak.Break_(message);
        }

        public override string ToString()
        {
            Ensures(Result<string>() != null);

            return "Void";
        }

        [DebuggerDisplay(@"""Break""")]
        [DebuggerTypeProxy(typeof(Break_.DebugView))]
        private sealed class Break_ : VoidOrBreak
        {
            private readonly string _reason;

            public Break_(string reason)
                : base(true)
            {
                Demand.NotNull(reason);

                _reason = reason;
            }

            public override string Reason
            {
                get
                {
                    Ensures(Result<string>() != null);

                    return _reason;
                }
            }

            public override string ToString()
            {
                Ensures(Result<string>() != null);

                return Format.CurrentCulture("Break({0})", _reason);
            }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Invariant(_reason != null);
                Invariant(_isBreak);
            }

#endif

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOrBreak.Break_"/>.
            /// </summary>
            [ContractVerification(false)] // Debugger-only code.
            [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
            private sealed class DebugView
            {
                private readonly Break_ _inner;

                public DebugView(Break_ inner)
                {
                    _inner = inner;
                }

                public string Reason
                {
                    get { return _inner.Reason; }
                }
            }
        }
    }
}
