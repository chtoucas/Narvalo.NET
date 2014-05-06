// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp.Resolvers;

    public sealed class AspNetPresenterBindingAttributesResolver : PresenterBindingAttributesResolver
    {
        public AspNetPresenterBindingAttributesResolver() : base() { }

        public override IEnumerable<PresenterBindingAttribute> Resolve(Type input)
        {
            if (input.IsAspNetDynamicType()) {
                return base.Resolve(input.BaseType);
            }
            else {
                return base.Resolve(input);
            }
        }
    }
}
