// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public sealed class VoidOrWarning
    {
        static readonly VoidOrWarning Success_ = new VoidOrWarning();

        readonly bool _isWarning;
        readonly string _message;

        VoidOrWarning()
        {
            _isWarning = false;
        }

        VoidOrWarning(string message)
        {
            _isWarning = true;
            _message = message;
        }

        public static VoidOrWarning Success { get { return Success_; } }

        public bool IsWarning { get { return _isWarning; } }

        public string Message
        {
            get
            {
                if (!IsWarning) {
                    throw new InvalidOperationException(SR.VoidOrError_NotWarning);
                }

                return _message;
            }
        }

        public static VoidOrWarning Warning(string message)
        {
            Require.NotNullOrEmpty(message, "message");

            return new VoidOrWarning(message);
        }

        public override string ToString()
        {
            return IsWarning ? _message : "{Void}";
        }
    }
}
