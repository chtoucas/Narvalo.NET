namespace Narvalo.Web.UI
{
    using System;
    using System.Web.UI;

    public abstract class UnrestrictedPageParserFilter : PageParserFilter
    {
        protected UnrestrictedPageParserFilter() : base() { }

        public override bool AllowCode { get { return true; } }

        public override int NumberOfControlsAllowed
        {
            get { return PageParserFilterConstants.UnlimitedNumber; }
        }

        public override int NumberOfDirectDependenciesAllowed
        {
            get { return PageParserFilterConstants.UnlimitedNumber; }
        }

        public override int TotalNumberOfDependenciesAllowed
        {
            get { return PageParserFilterConstants.UnlimitedNumber; }
        }

        public override bool AllowBaseType(Type baseType)
        {
            return true;
        }

        public override bool AllowControl(Type controlType, ControlBuilder builder)
        {
            return true;
        }

        public override bool AllowServerSideInclude(string includeVirtualPath)
        {
            return true;
        }

        public override bool AllowVirtualReference(string referenceVirtualPath, VirtualReferenceType referenceType)
        {
            return true;
        }
    }
}