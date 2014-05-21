// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class /*Default*/PresenterBindingAttributesResolver : IPresenterBindingAttributesResolver
    {
        public virtual IEnumerable<PresenterBindingAttribute> Resolve(Type input)
        {
            Require.NotNull(input, "input");

            __Tracer.Info(this, @"Attempting to resolve ""{0}"".", input.FullName);

            var attributes = input
                .GetCustomAttributes(typeof(PresenterBindingAttribute), inherit: true)
                .OfType<PresenterBindingAttribute>()
                .ToArray();

            if (attributes.Any(a =>
                    a.BindingMode == PresenterBindingMode.SharedPresenter && a.ViewType == null)) {
                throw new PresenterBindingException(
                    String.Format(
                        CultureInfo.InvariantCulture,
                        @"When shared presenter binding is requested, the ""ViewType"" must be explicitly specified. One of the bindings on ""{0}"" violates this restriction.",
                        input.FullName));
            }

            return attributes
                .Select(pba =>
                    new PresenterBindingAttribute(pba.PresenterType)
                    {
                        BindingMode = pba.BindingMode,
                        Origin = input,
                        ViewType = pba.ViewType ?? input,
                    })
                .ToArray();
        }
    }
}
