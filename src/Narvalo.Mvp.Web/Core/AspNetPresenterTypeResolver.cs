// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Mvp.Resolvers;
    using Narvalo.Mvp.Web.Internal;

    using static System.Diagnostics.Contracts.Contract;

    public sealed class AspNetPresenterTypeResolver : PresenterTypeResolver
    {
        public AspNetPresenterTypeResolver(
            IBuildManager buildManager,
            IEnumerable<string> defaultNamespaces,
            IEnumerable<string> viewSuffixes,
            IEnumerable<string> presenterNameTemplates)
            : base(buildManager, defaultNamespaces, viewSuffixes, presenterNameTemplates)
        {
            Expect.NotNull(buildManager);
            Expect.NotNull(defaultNamespaces);
            Expect.NotNull(viewSuffixes);
            Expect.NotNull(presenterNameTemplates);
        }

        // REVIEW: Prefers composition over extension?
        public override Type Resolve(Type viewType)
        {
            Require.NotNull(viewType, nameof(viewType));

            if (viewType.IsAspNetDynamicType())
            {
                Assume(viewType.BaseType != null);
                return base.Resolve(viewType.BaseType);
            }
            else
            {
                return base.Resolve(viewType);
            }
        }
    }
}
