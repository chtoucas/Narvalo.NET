// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class /*Default*/PresenterBindingAttributesResolver : IPresenterBindingAttributesResolver
    {
        public IEnumerable<PresenterBindingAttribute> Resolve(Type viewType)
        {
            Require.NotNull(viewType, "viewType");

            Tracer.Info(this, @"Attempting to resolve ""{0}"".", viewType.FullName);

            var attributes = viewType
                .GetCustomAttributes(typeof(PresenterBindingAttribute), inherit: true)
                .OfType<PresenterBindingAttribute>()
                .ToArray();

            if (attributes.Any(a =>
                    a.BindingMode == PresenterBindingMode.SharedPresenter && a.ViewType == null)) {
                throw new PresenterBindingException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        @"When shared presenter binding is requested, the ""ViewType"" must be explicitly specified. One of the bindings on ""{0}"" violates this restriction.",
                        viewType.FullName));
            }

            return attributes
                .Select(pba =>
                    new PresenterBindingAttribute(pba.PresenterType)
                    {
                        BindingMode = pba.BindingMode,
                        Origin = viewType,
                        ViewType = pba.ViewType ?? viewType,
                    })
                .ToArray();
        }
    }
}
