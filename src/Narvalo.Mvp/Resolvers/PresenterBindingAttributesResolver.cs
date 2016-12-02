// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    public class /*Default*/PresenterBindingAttributesResolver : IPresenterBindingAttributesResolver
    {
        public IEnumerable<PresenterBindingAttribute> Resolve(Type viewType)
        {
            Require.NotNull(viewType, nameof(viewType));

            Trace.TraceInformation(
                "[PresenterBindingAttributesResolver] Attempting to resolve '{0}'.",
                viewType.FullName);

            var attributes = viewType
                .GetCustomAttributes(typeof(PresenterBindingAttribute), inherit: true)
                .OfType<PresenterBindingAttribute>()
                .ToArray();

            if (attributes.Any(a =>
                    a.BindingMode == PresenterBindingMode.SharedPresenter && a.ViewType == null))
            {
                throw new PresenterBindingException(Format.Current(
                    "When shared presenter binding is requested, the view type must be explicitly specified. One of the bindings on '{0}' violates this restriction.",
                    viewType.FullName));
            }

            return attributes
                .Select(pba =>
                    new PresenterBindingAttribute(pba.PresenterType) {
                        BindingMode = pba.BindingMode,
                        Origin = viewType,
                        ViewType = pba.ViewType ?? viewType,
                    })
                .ToArray();
        }
    }
}
