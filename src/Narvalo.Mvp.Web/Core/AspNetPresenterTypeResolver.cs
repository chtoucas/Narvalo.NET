// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp.Resolvers;
    using Narvalo.Mvp.Web.Internal;

    public sealed class AspNetPresenterTypeResolver : PresenterTypeResolver
    {
        public AspNetPresenterTypeResolver(
            IBuildManager buildManager,
            IEnumerable<string> defaultNamespaces,
            IEnumerable<string> viewSuffixes,
            IEnumerable<string> presenterNameTemplates)
            : base(buildManager, defaultNamespaces, viewSuffixes, presenterNameTemplates) { }

        // REVIEW: Prefers composition over extension?
        public override Type Resolve(Type input)
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
