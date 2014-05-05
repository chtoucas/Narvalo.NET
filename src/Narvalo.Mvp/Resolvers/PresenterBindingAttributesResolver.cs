// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Narvalo.Mvp.Internal;

    public class PresenterBindingAttributesResolver
        : IPresenterBindingAttributesResolver
    {
        public virtual IEnumerable<PresenterBindingAttribute> Resolve(Type input)
        {
            Require.NotNull(input, "input");

            __Trace.Write("[PresenterBindingAttributesResolver] Attempting to resolve: " + input.FullName);

            var attributes = input
                .GetCustomAttributes(typeof(PresenterBindingAttribute), true /* inherit */)
                .OfType<PresenterBindingAttribute>()
                .ToArray();

            if (attributes.Any(a =>
                    a.BindingMode == PresenterBindingMode.SharedPresenter && a.ViewType == null
                )) {
                throw new PresenterBindingException(String.Format(
                    CultureInfo.InvariantCulture,
                    "When a {1} is applied with BindingMode={2}, the ViewType must be explicitly specified. One of the bindings on {0} violates this restriction.",
                    input.FullName,
                    typeof(PresenterBindingAttribute).Name,
                    Enum.GetName(typeof(PresenterBindingMode), PresenterBindingMode.SharedPresenter)
                ));
            }

            return attributes
                .Select(pba =>
                    new PresenterBindingAttribute(pba.PresenterType)
                    {
                        ViewType = pba.ViewType ?? input,
                        BindingMode = pba.BindingMode,
                        Origin = input,
                    })
                .ToArray();
        }
    }
}
