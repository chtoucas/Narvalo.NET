namespace Narvalo.Web.Semantic
{
    using System.Globalization;
    using Narvalo;

    public sealed class OpenGraphLocale
    {
        CultureInfo _culture;

        public OpenGraphLocale(CultureInfo culture)
        {
            Require.NotNull(culture, "culture");

            _culture = culture;
        }

        public CultureInfo Culture { get { return _culture; } }

        public override string ToString()
        {
            return _culture.ToString().Replace('-', '_');
        }
    }
}
