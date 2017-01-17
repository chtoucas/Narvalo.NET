// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Globalization;

    // Used internally to ensure that Currency and Currency<T> look alike.
    internal interface ICurrencyUnit
    {
        string Code { get; }

        short? MinorUnits { get; }

        int DecimalPlaces { get; }

        bool HasFixedDecimalPlaces { get; }

        bool IsMetaCurrency { get; }

        bool IsPseudoCurrency { get; }

        decimal Epsilon { get; }

        uint Factor { get; }

        decimal One { get; }

        //bool HasMinorCurrency { get; }

        //FractionalCurrency MinorCurrency { get; }

        //string MinorCurrencyCode { get; }

        bool IsNativeTo(CultureInfo cultureInfo);
    }
}
