// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public sealed class VoidOrBreak
    {
        static readonly VoidOrBreak Success_ = new VoidOrBreak();

        readonly bool _isBreak;
        readonly string _reason;

        VoidOrBreak()
        {
            _isBreak = false;
        }

        VoidOrBreak(string reason)
        {
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
                    throw new InvalidOperationException(SR.VoidOrError_NotWarning);
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
    }
}
