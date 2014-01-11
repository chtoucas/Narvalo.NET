namespace Narvalo.Web.Optimization
{
    public class WhiteSpaceBuster : IWhiteSpaceBuster
    {
        IWhiteSpaceBuster _inner = new GentleWhiteSpaceBuster();

        WhiteSpaceBuster() { }

        public static WhiteSpaceBuster Current { get { return Inner.Instance; } }

        #region IWhiteSpaceBuster

        public string Bust(string value)
        {
            return _inner.Bust(value);
        }

        #endregion

        public void SetWhiteSpaceBuster(IWhiteSpaceBuster buster)
        {
            Requires.NotNull(buster, "buster");

            _inner = buster;
        }

        class Inner
        {
            static Inner() { }
            internal static readonly WhiteSpaceBuster Instance = new WhiteSpaceBuster();
        }
    }
}
