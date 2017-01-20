// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Globalization
{
    using System;

    public sealed class CustomMoneyFormatter : IFormatProvider
    {
        private static readonly Formatter s_Formatter = new Formatter();

        // Let's see how it goes with:
        // > var provider = new CustomMoneyFormatter();
        //
        // formatType = typeof(Money) is required for Money.ToString(format, IFormatProvider) to work:
        // > money.ToString("N", provider);
        // > formatter = provider.GetFormat(money.GetType()) as ICustomFormatter;
        // where formatter is of type MoneyFormatInfo.Formatter,
        // > formatter.Format("N", money, provider);
        // money being of type Money, formatter knows how to format it. NB: it **never** returns null.
        //
        // formatType = typeof(ICustomFormatter) is required for String.Format(IFormatProvider, format, args),
        // and StringBuilder.AppendFormat(IFormatProvider, format, args) to work; see
        // https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Text/StringBuilder.cs
        // For instance,
        // > String.Format(provider, "{0:N} {1:G}", money, whatever);
        // > formatter = provider.GetFormat(typeof(ICustomFormatter));
        // where formatter is of type CustomMoneyFormatter.Formatter, then:
        // > formatter.Format("N", money, provider)
        // > formatter.Format("G", whatever, provider)
        // The first call never returns null (see remark above).
        // If the second call returns something, we are done, otherwise, String.Format() checks
        // if 'whatever' implements IFormattable. If it does, it calls:
        // > whatever.ToString("G", provider);
        // otherwise, it falls back to Object.ToString():
        // > whatever.ToString();
        // If (again) the result is null, then it outputs String.Empty.
        // Remark: If we did not handle typeof(ICustomFormatter), String.Format() would check if
        // the object implemented IFormattable. For 'money', since it is the case, it would call
        // money.ToString("N", provider) and we are back to the first case. Everything would work
        // as expected, but it would be less effective.
        //
        // If formatType is anything else, for instance,
        // > whatever.ToString("N", provider);
        // might call
        // > formatter = provider.GetFormat(whatever.GetType());
        // like we do in Money.ToString(). We always return null and it's up to Whatever.ToString()
        // to deal with this. We could return CultureInfo.CurrentCulture.GetFormat(formatType)
        // that is a NumberFormatInfo, a DateTimeFormatInfo or null, matter of taste I think.
        public object GetFormat(Type formatType)
            => formatType == typeof(Money) || formatType == typeof(ICustomFormatter) ? s_Formatter : null;

        private sealed class Formatter : ICustomFormatter
        {
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg == null) { return String.Empty; }

                //        var mfi = formatProvider as CustomMoneyFormatter;
                //        if (mfi == null)
                //        {
                //            // This should never happen. Normally, formatProvider is not null and of type
                //            // MoneyFormatInfo.
                //            // Nevertheless, one might call formatter = provider.GetFormat(...) but use the result
                //            // with another provider (I don't see why, but we never know):
                //            // > formatter.Format(format, arg, anotherProvider);
                //            // Rather than trying to deal with it (we could set provider to the current culture)
                //            // we prefer to return null since this most certainly means a coding error from the
                //            // caller.
                //            return null;
                //        }

                if (arg.GetType() == typeof(Money))
                {
                    //return MoneyFormatters.FormatMoney((Money)arg, format, formatProvider);
                    throw new NotImplementedException();
                }

                var formattable = arg as IFormattable;
                return formattable == null ? arg.ToString() : formattable.ToString(format, formatProvider);
            }
        }
    }
}
