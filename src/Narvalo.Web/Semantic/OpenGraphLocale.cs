namespace Narvalo.Web.Semantic
{
    using System;
    using System.Globalization;
    using Narvalo;

    public struct OpenGraphLocale : IEquatable<OpenGraphLocale>
    {
        CultureInfo _culture;

        public OpenGraphLocale(CultureInfo culture)
        {
            Require.NotNull(culture, "culture");

            _culture = culture;
        }

        public CultureInfo Culture { get { return _culture; } }

        public static bool operator ==(OpenGraphLocale left, OpenGraphLocale right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OpenGraphLocale left, OpenGraphLocale right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return _culture.ToString().Replace('-', '_');
        }

        public bool Equals(OpenGraphLocale other)
        {
            return _culture.Equals(other._culture);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is OpenGraphLocale)) {
                return false;
            }

            return Equals((OpenGraphLocale)obj);
        }

        public override int GetHashCode()
        {
            return _culture.GetHashCode();
        }
    }
}
