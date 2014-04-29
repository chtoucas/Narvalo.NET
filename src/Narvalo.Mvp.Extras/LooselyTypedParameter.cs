// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using Autofac.Core;

    internal class LooselyTypedParameter : ConstantParameter
    {
        public LooselyTypedParameter(Type type, object value)
            : base(value, pi => pi.ParameterType.IsAssignableFrom(type))
        {
            DebugCheck.NotNull(type);
        }
    }
}
