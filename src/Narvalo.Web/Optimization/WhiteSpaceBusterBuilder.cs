namespace Narvalo.Web.Optimization
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class WhiteSpaceBusterBuilder
    {
        static WhiteSpaceBusterBuilder Instance_ = new WhiteSpaceBusterBuilder();

        Func<IWhiteSpaceBuster> _thunk;

        public WhiteSpaceBusterBuilder() : this(new GentleWhiteSpaceBuster()) { }

        internal WhiteSpaceBusterBuilder(IWhiteSpaceBuster whiteSpaceBuster)
        {
            _thunk = () => whiteSpaceBuster;
        }

        public static WhiteSpaceBusterBuilder Current { get { return Instance_; } }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public IWhiteSpaceBuster GetWhiteSpaceBuster()
        {
            return _thunk();
        }

        public void SetWhiteSpaceBuster(IWhiteSpaceBuster whiteSpaceBuster)
        {
            Requires.NotNull(whiteSpaceBuster, "whiteSpaceBuster");

            _thunk = () => whiteSpaceBuster;
        }
    }
}
