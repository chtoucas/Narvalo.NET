﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using Narvalo.Fx;

    public static partial class MayConvertTo
    {
        public static Maybe<TEnum> Enum<TEnum>(object value) where TEnum : struct
        {
            TEnum result;
            if (!TryConvertTo.Enum<TEnum>(value, out result)) {
                return Maybe<TEnum>.None;
            }
            
            return Maybe.Create(result);
        }
    }
}
