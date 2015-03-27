// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Resources;

    /// <seealso cref="Output{T}"/>
    /// <seealso cref="Either{T1, T2}"/>
    /// <seealso cref="Switch{T1, T2}"/>
    /// <seealso cref="VoidOrError"/>
    public class VoidOrBreak
    {
        private static readonly VoidOrBreak s_Void = new VoidOrBreak();

        private readonly bool _isBreak;

        private VoidOrBreak() { }

        private VoidOrBreak(bool arboted)
        {
            _isBreak = arboted;
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
        }
    }
}
