// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

using Autofac.Core;

namespace Narvalo.Internal
{
    using System;

    internal class LooselyTypedParameter : ConstantParameter
    {
        public LooselyTypedParameter(Type type, object value)
            : base(value, pi => pi.ParameterType.IsAssignableFrom(type))
        {
            DebugCheck.NotNull(type);
        }
    }
}
