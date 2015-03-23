// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.Contracts;

    public class VoidOrBreak
    {
        private static readonly VoidOrBreak s_Success = new VoidOrBreak();

        private readonly bool _aborted;

        private VoidOrBreak() { }

        private VoidOrBreak(bool arboted)
        {
            _aborted = arboted;
        }

        public static VoidOrBreak Success
        {
            get
            {
                Contract.Ensures(Contract.Result<VoidOrBreak>() != null);

                return s_Success;
            }
        }

        public bool Aborted { get { return _aborted; } }

        public virtual string Reason
        {
            get
            {
                throw new InvalidOperationException(Strings_Core.VoidOrBreak_IsNotBreak);
            }
        }

        public static VoidOrBreak Abort(string reason)
        {
            Require.NotNullOrEmpty(reason, "reason");
            Contract.Ensures(Contract.Result<VoidOrBreak>() != null);

            return new VoidOrBreak.Break_(reason);
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return "{Void}";
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

                return _reason;
            }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

            [ContractInvariantMethod]
            private void ObjectInvariants()
            {
                Contract.Invariant(_reason != null);
            }

#endif
        }
    }
}
