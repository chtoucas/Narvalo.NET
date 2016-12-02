// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Internal
{
    using System;
    using System.Web.UI;

    internal static class TypeExtensions
    {
        public static bool IsAspNetDynamicType(this Type @this)
        {
            Require.Object(@this);

            // REVIEW: The following remarks are taken from the original WebFormsMvp code.
            // Use the base type for pages & user controls as that is the code-behind file.
            // TODO: Ensure using BaseType still works in WebSite projects with code-beside files
            // instead of code-behind files.
            return @this.Namespace == "ASP"
                && (typeof(Page).IsAssignableFrom(@this) || typeof(Control).IsAssignableFrom(@this));
        }
    }
}
