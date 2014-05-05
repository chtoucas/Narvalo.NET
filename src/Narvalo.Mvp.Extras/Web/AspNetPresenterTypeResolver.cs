// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using Narvalo.Mvp.Resolvers;

    public class AspNetPresenterTypeResolver : PresenterTypeResolver
    {
        public AspNetPresenterTypeResolver(
            IBuildManager buildManager,
            IEnumerable<string> defaultNamespaces,
            string[] viewSuffixes,
            string[] presenterNameTemplates)
            : base(buildManager, defaultNamespaces, viewSuffixes, presenterNameTemplates) { }

        public override Type Resolve(Type input)
        {
            // Use the base type for pages & user controls as that is the code-behind file.
            // TODO: Ensure using BaseType still works in WebSite projects with code-beside files
            // instead of code-behind files.
            if (input.Namespace == "ASP"
                && (typeof(Page).IsAssignableFrom(input) || typeof(Control).IsAssignableFrom(input))) {
                input = input.BaseType;
            }

            return base.Resolve(input);
        }
    }
}
