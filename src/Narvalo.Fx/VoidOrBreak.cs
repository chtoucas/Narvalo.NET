// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx.Properties;

    // Friendly version of Either<String, Unit>.
    [DebuggerDisplay("Void")]
    public partial class VoidOrBreak
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly VoidOrBreak s_Void = new VoidOrBreak();

        private VoidOrBreak() { }

        private VoidOrBreak(bool isBreak)
        {
            IsBreak = isBreak;
        }

        public static VoidOrBreak Void
        {
            get
            {
                Warrant.NotNull<VoidOrBreak>();

                return s_Void;
            }
        }

        public bool IsBreak { get; }

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
            Warrant.NotNull<VoidOrBreak>();

            return new VoidOrBreak.Break_(message);
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return "Void";
        }

        [DebuggerDisplay("Break")]
        [DebuggerTypeProxy(typeof(Break_.DebugView))]
        private sealed partial class Break_ : VoidOrBreak
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
                    Warrant.NotNull<string>();

                    return _reason;
                }
            }

            public override string ToString()
            {
                Warrant.NotNull<string>();

                return Format.Current("Break({0})", _reason);
            }

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

#if CONTRACTS_FULL

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;

    public partial class VoidOrBreak
    {
        private sealed partial class Break_
        {
            [ContractInvariantMethod]
            private void ObjectInvariant()
            {
                Contract.Invariant(_reason != null);
                Contract.Invariant(IsBreak);
            }
        }
    }
}

#endif
