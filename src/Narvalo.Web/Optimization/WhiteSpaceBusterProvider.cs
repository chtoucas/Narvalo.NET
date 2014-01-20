namespace Narvalo.Web.Optimization
{
    using System;

    public class WhiteSpaceBusterProvider
    {
        static readonly object Lock_ = new Object();
        static WhiteSpaceBusterProvider Instance_ = new WhiteSpaceBusterProvider();

        IWhiteSpaceBuster _whiteSpaceBuster;

        public WhiteSpaceBusterProvider() { }

        public static WhiteSpaceBusterProvider Current { get { return Instance_; } }

        public IWhiteSpaceBuster WhiteSpaceBuster
        {
            get
            {
                if (_whiteSpaceBuster == null) {
                    lock (Lock_) {
                        if (_whiteSpaceBuster == null) {
                            _whiteSpaceBuster = new GentleWhiteSpaceBuster();
                        }
                    }
                }

                return _whiteSpaceBuster;
            }

            set
            {
                lock (Lock_) {
                    _whiteSpaceBuster = Require.Property(value);
                }
            }
        }
    }
}
