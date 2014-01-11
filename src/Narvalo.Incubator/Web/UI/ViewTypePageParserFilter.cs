namespace System.Web.Mvc
{
    using System;
    using System.Collections;
    using System.Web.Mvc;
    using System.Web.UI;
    using Narvalo.Web.UI;

    internal class ViewTypePageParserFilter : UnrestrictedPageParserFilter
    {
        string _viewBaseType;
        DirectiveType _directiveType = DirectiveType.Unknown;
        bool _viewTypeControlAdded;

        public ViewTypePageParserFilter() : base() { }

        public override void ParseComplete(ControlBuilder rootBuilder)
        {
            base.ParseComplete(rootBuilder);

            // If it's our page ControlBuilder, give it the base type string
            var pageBuilder = rootBuilder as ViewPageControlBuilder;
            if (pageBuilder != null) {
                pageBuilder.PageBaseType = _viewBaseType;
            }
            var userControlBuilder = rootBuilder as ViewUserControlControlBuilder;
            if (userControlBuilder != null) {
                userControlBuilder.UserControlBaseType = _viewBaseType;
            }
        }

        public override void PreprocessDirective(string directiveName, IDictionary attributes)
        {
            base.PreprocessDirective(directiveName, attributes);

            string defaultBaseType = null;

            // If we recognize the directive, keep track of what it was. If we don't recognize
            // the directive then just stop.
            switch (directiveName) {
                case "page":
                    _directiveType = DirectiveType.Page;
                    defaultBaseType = typeof(ViewPage).FullName;
                    break;
                case "control":
                    _directiveType = DirectiveType.UserControl;
                    defaultBaseType = typeof(ViewUserControl).FullName;
                    break;
                case "master":
                    _directiveType = DirectiveType.Master;
                    defaultBaseType = typeof(ViewMasterPage).FullName;
                    break;
            }

            if (_directiveType == DirectiveType.Unknown) {
                // If we're processing an unknown directive (e.g. a register directive), stop processing
                return;
            }

            // Look for an inherit attribute
            string inherits = (string)attributes["inherits"];
            if (!String.IsNullOrEmpty(inherits)) {
                // If it doesn't look like a generic type, don't do anything special,
                // and let the parser do its normal processing
                if (IsGenericTypeString(inherits)) {
                    // Remove the inherits attribute so the parser doesn't blow up
                    attributes["inherits"] = defaultBaseType;

                    // Remember the full type string so we can later give it to the ControlBuilder
                    _viewBaseType = inherits;
                }
            }
        }

        public override bool ProcessCodeConstruct(CodeConstructType codeType, string code)
        {
            if (codeType == CodeConstructType.ExpressionSnippet
                && !_viewTypeControlAdded 
                && _viewBaseType != null 
                && _directiveType == DirectiveType.Master) {

                // If we're dealing with a master page that needs to have its base type set, do it here.
                // It's done by adding the ViewType control, which has a builder that sets the base type.

                // The code currently assumes that the file in question contains a code snippet, since
                // that's the item we key off of in order to know when to add the ViewType control.

                var attrs = new Hashtable();
                attrs["typename"] = _viewBaseType;
                AddControl(typeof(ViewType), attrs);
                _viewTypeControlAdded = true;
            }

            return base.ProcessCodeConstruct(codeType, code);
        }

        // Everything else in this class is unrelated to our 'inherits' handling.
        // Since PageParserFilter blocks everything by default, we need to unblock it

        static bool IsGenericTypeString(string typeName)
        {
            // Detect C# and VB generic syntax
            // REVIEW: what about other languages?
            return typeName.IndexOfAny(new char[] { '<', '(' }) >= 0;
        }

        enum DirectiveType
        {
            Unknown = 0,
            Master,
            Page,
            UserControl,
        }
    }
}