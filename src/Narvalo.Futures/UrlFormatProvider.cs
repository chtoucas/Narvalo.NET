// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    // See https://www.thomaslevesque.com/2015/02/24/customizing-string-interpolation-in-c-6/
    // $"http://www.blabla.org/?id={Id}".ToString(new UrlFormatProvider())
    public class UrlFormatProvider : IFormatProvider
    {
        private readonly UrlFormatter s_Formatter = new UrlFormatter();

        public object GetFormat(Type formatType)
            => formatType == typeof(ICustomFormatter) ? s_Formatter : null;

        private class UrlFormatter : ICustomFormatter
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg == null) { return String.Empty; }

                var strval = arg.ToString();

                if ((format[0] & 0xDF) == 'R') { return strval; }

                return Uri.EscapeDataString(strval);
            }
        }
    }
}
