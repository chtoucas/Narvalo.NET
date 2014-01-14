namespace Narvalo
{
    using System;
    using System.Globalization;

    public static class Format
    {
        public static string CurrentCulture(string format, params object[] args)
        {
            return String.Format(CultureInfo.CurrentCulture, format, args);
        }
    }
}
