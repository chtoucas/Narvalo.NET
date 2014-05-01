// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp;

    internal class ViewInterfacesProvider : IComponentProvider<Type, IEnumerable<Type>>
    {
        public virtual IEnumerable<Type> GetComponent(Type input)
        {
            DebugCheck.NotNull(input);

            return input.GetInterfaces().Where(typeof(IView).IsAssignableFrom).ToArray();
        }
    }
}
