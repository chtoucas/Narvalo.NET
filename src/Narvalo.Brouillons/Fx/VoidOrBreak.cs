// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public sealed class VoidOrBreak
    {
        private static readonly VoidOrBreak s_Success = new VoidOrBreak();

        private readonly bool _isBreak;
        private readonly string _reason;

        private VoidOrBreak()
        {
            _isBreak = false;
        }

        private VoidOrBreak(string reason)
        {
            Contract.Requires(reason != null);

            _isBreak = true;
            _reason = reason;
        }

        public static VoidOrBreak Success
        {
            get
            {
                Contract.Ensures(Contract.Result<VoidOrBreak>() != null);
                return s_Success;
            }
        }

        public bool IsBreak { get { return _isBreak; } }

        public string Message
        {
            get
            {
                if (!IsBreak)
                {
                    throw new InvalidOperationException(Strings.VoidOrError_NotWarning);
                }

                return _reason;
            }
        }

        public static VoidOrBreak Abort(string reason)
        {
            Require.NotNullOrEmpty(reason, "reason");
            Contract.Ensures(Contract.Result<VoidOrBreak>() != null);

            return new VoidOrBreak(reason);
        }

        public override string ToString()
        {
            return IsBreak ? _reason : "{Void}";
        }
        
#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(!_isBreak || _reason != null);
        }

#endif
    }
}
