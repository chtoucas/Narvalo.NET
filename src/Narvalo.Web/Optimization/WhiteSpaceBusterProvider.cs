// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System;

    public sealed class WhiteSpaceBusterProvider
    {
        static readonly object Lock_ = new Object();
        static WhiteSpaceBusterProvider Instance_ = new WhiteSpaceBusterProvider();

        IWhiteSpaceBuster _pageBuster;
        IWhiteSpaceBuster _razorBuster;

        public WhiteSpaceBusterProvider() { }

        public static WhiteSpaceBusterProvider Current { get { return Instance_; } }

        public IWhiteSpaceBuster PageBuster
        {
            get
            {
                if (_pageBuster == null) {
                    lock (Lock_) {
                        if (_pageBuster == null) {
                            _pageBuster = new DefaultWhiteSpaceBuster();
                        }
                    }
                }

                return _pageBuster;
            }

            set
            {
                Require.Property(value);

                lock (Lock_) {
                    _pageBuster = value;
                }
            }
        }

        public IWhiteSpaceBuster RazorBuster
        {
            get
            {
                if (_razorBuster == null) {
                    lock (Lock_) {
                        if (_razorBuster == null) {
                            _razorBuster = new DefaultWhiteSpaceBuster();
                        }
                    }
                }

                return _razorBuster;
            }

            set
            {
                Require.Property(value);

                lock (Lock_) {
                    _razorBuster = value;
                }
            }
        }
    }
}
