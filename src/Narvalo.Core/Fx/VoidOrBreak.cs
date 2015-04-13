// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Properties;

    /// <seealso cref="Output{T}"/>
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
                Contract.Ensures(Contract.Result<VoidOrBreak>() != null);

                return s_Void;
            }
        }

        public bool IsBreak { get { return _isBreak; } }

        public virtual string Reason
        {
            get
            {
                throw new InvalidOperationException(Strings_Core.VoidOrBreak_BreakHasNoReason);
            }
        }

        public static VoidOrBreak Break(string message)
        {
            Require.NotNullOrEmpty(message, "message");
            Contract.Ensures(Contract.Result<VoidOrBreak>() != null);

            return new VoidOrBreak.Break_(message);
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

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
                Contract.Requires(reason != null);

                _reason = reason;
            }

            public override string Reason
            {
                get
                {
                    Contract.Ensures(Contract.Result<string>() != null);

                    return _reason;
                }
            }

            public override string ToString()
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return Format.CurrentCulture("Break({0})", _reason);
            }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_reason != null);
                Contract.Invariant(_isBreak);
            }

#endif

            /// <summary>
            /// Represents a debugger type proxy for <see cref="VoidOrBreak.Break_"/>.
            /// </summary>
            private sealed class DebugView
            {
                private readonly Break_ _inner;

                public DebugView(Break_ inner)
                {
                    _inner = inner;
                }

                [SuppressMessage("Gendarme.Rules.Performance", "AvoidUncalledPrivateCodeRule",
                    Justification = "[Ignore] Debugger-only code.")]
                public string Reason
                {
                    get { return _inner.Reason; }
                }
            }
        }
    }
}
