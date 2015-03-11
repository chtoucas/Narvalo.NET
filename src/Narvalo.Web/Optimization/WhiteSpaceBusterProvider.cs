// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System;
    using System.Diagnostics.Contracts;

    public sealed class WhiteSpaceBusterProvider
    {
        private static readonly object s_Lock = new Object();
        private static WhiteSpaceBusterProvider s_Instance = new WhiteSpaceBusterProvider();

        private IWhiteSpaceBuster _pageBuster;
        private IWhiteSpaceBuster _razorBuster;

        public WhiteSpaceBusterProvider() { }

        public static WhiteSpaceBusterProvider Current
        {
            get
            {
                Contract.Ensures(Contract.Result<WhiteSpaceBusterProvider>() != null);

                return s_Instance;
            }
        }

        public IWhiteSpaceBuster PageBuster
        {
            get
            {
                Contract.Ensures(Contract.Result<IWhiteSpaceBuster>() != null);

                if (_pageBuster == null)
                {
                    lock (s_Lock)
                    {
                        if (_pageBuster == null)
                        {
                            _pageBuster = new DefaultWhiteSpaceBuster();
                        }
                    }
                }

                return _pageBuster;
            }

            set
            {
                Require.Property(value);

                lock (s_Lock)
                {
                    _pageBuster = value;
                }
            }
        }

        public IWhiteSpaceBuster RazorBuster
        {
            get
            {
                Contract.Ensures(Contract.Result<IWhiteSpaceBuster>() != null);

                if (_razorBuster == null)
                {
                    lock (s_Lock)
                    {
                        if (_razorBuster == null)
                        {
                            _razorBuster = new DefaultWhiteSpaceBuster();
                        }
                    }
                }

                return _razorBuster;
            }

            set
            {
                Require.Property(value);

                lock (s_Lock)
                {
                    _razorBuster = value;
                }
            }
        }
    }
}
