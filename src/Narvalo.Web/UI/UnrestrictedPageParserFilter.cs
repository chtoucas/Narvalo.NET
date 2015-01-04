// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Web.UI;

    public abstract class UnrestrictedPageParserFilter : PageParserFilter
    {
        protected UnrestrictedPageParserFilter() { }

        public sealed override bool AllowCode { get { return true; } }

        public sealed override int NumberOfControlsAllowed
        {
            get { return PageParserFilterConstants.UnlimitedNumber; }
        }

        public sealed override int NumberOfDirectDependenciesAllowed
        {
            get { return PageParserFilterConstants.UnlimitedNumber; }
        }

        public sealed override int TotalNumberOfDependenciesAllowed
        {
            get { return PageParserFilterConstants.UnlimitedNumber; }
        }

        public sealed override bool AllowBaseType(Type baseType)
        {
            return true;
        }

        public sealed override bool AllowControl(Type controlType, ControlBuilder builder)
        {
            return true;
        }

        public sealed override bool AllowServerSideInclude(string includeVirtualPath)
        {
            return true;
        }

        public sealed override bool AllowVirtualReference(string referenceVirtualPath, VirtualReferenceType referenceType)
        {
            return true;
        }
    }
}