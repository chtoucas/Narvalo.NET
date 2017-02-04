// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    // See https://www.thomaslevesque.com/2015/02/24/customizing-string-interpolation-in-c-6/
    // FormattableString formattable = ...
    // formattable.ToString(new UrlFormatProvider())
    public class UrlFormatProvider : IFormatProvider
    {
        private readonly UrlFormatter _formatter = new UrlFormatter();

        public object GetFormat(Type formatType)
        {
            if (formatType != typeof(ICustomFormatter)) { return null; }
            return _formatter;
        }

        private class UrlFormatter : ICustomFormatter
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg == null) { return String.Empty; }
                if (format == "r") { return arg.ToString(); }

                return Uri.EscapeDataString(arg.ToString());
            }
        }
    }
}
