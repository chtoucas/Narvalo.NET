// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    public sealed class VoidOrBreak
    {
        private static readonly VoidOrBreak Success_ = new VoidOrBreak();

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

        public static VoidOrBreak Success { get { return Success_; } }

        public bool IsBreak { get { return _isBreak; } }

        public string Message
        {
            get
            {
                if (!IsBreak) {
                    throw new InvalidOperationException(Strings_Core.VoidOrError_NotWarning);
                }

                return _reason;
            }
        }

        public static VoidOrBreak Abort(string reason)
        {
            Require.NotNullOrEmpty(reason, "reason");

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
