// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Linq;
    using Narvalo.Mvp.Binder;

    public sealed class AspNetConventionBasedPresenterDiscoveryStrategy
        : ConventionBasedPresenterDiscoveryStrategy
    {
        static readonly string[] ViewSuffixes_ = new[] 
        {
            "UserControl",
            "Control",
            "Page",
            "Handler",
            "WebService",
            "Service",
            "View",
        };

        static readonly string[] PresenterNameTemplates_ = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        public AspNetConventionBasedPresenterDiscoveryStrategy()
            : base(
                new BuildManagerWrapper(),
                // FIXME
                Enumerable.Empty<string>(),
                ViewSuffixes_,
                PresenterNameTemplates_) { }
    }
}
