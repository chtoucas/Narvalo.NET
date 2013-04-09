namespace Narvalo.Web.UI
{
    using System;
    using System.Web.UI;

    public abstract class UnrestrictedParserFilterBase : PageParserFilter
    {
        public override bool AllowCode
        {
            get { return true; }
        }

        public override int NumberOfControlsAllowed
        {
            get { return -1; }
        }

        public override int NumberOfDirectDependenciesAllowed
        {
            get { return -1; }
        }

        public override int TotalNumberOfDependenciesAllowed
        {
            get { return -1; }
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

        //public override CompilationMode GetCompilationMode(CompilationMode current) {
        //    return base.GetCompilationMode(current);
        //}

        //public override Type GetNoCompileUserControlType() {
        //    return base.GetNoCompileUserControlType();
        //}

        //public override void PreprocessDirective(string directiveName, IDictionary attributes) {
        //    base.PreprocessDirective(directiveName, attributes);
        //}

        //public override bool ProcessCodeConstruct(CodeConstructType codeType, string code) {
        //    return base.ProcessCodeConstruct(codeType, code);
        //}

        //public override bool ProcessDataBindingAttribute(string controlId, string name, string value) {
        //    return base.ProcessDataBindingAttribute(controlId, name, value);
        //}

        //public override bool ProcessEventHookup(string controlId, string eventName, string handlerName) {
        //    return base.ProcessEventHookup(controlId, eventName, handlerName);
        //}

        //public override void ParseComplete(ControlBuilder rootBuilder) {
        //    base.ParseComplete(rootBuilder);
        //}

        //protected override void Initialize() {
        //    base.Initialize();
        //}
    }
}