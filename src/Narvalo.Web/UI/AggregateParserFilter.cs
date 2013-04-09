namespace Narvalo.Web.UI
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Configuration;
    using System.Web.UI;
    using Narvalo.Web.Configuration;

    public class AggregateParserFilter : PageParserFilter
    {
        const BindingFlags InstPubNonpub 
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        List<PageParserFilter> _filters = new List<PageParserFilter>();

        public override bool AllowCode
        {
            get
            {
                foreach (PageParserFilter filter in _filters) {
                    if (!filter.AllowCode) {
                        return false;
                    }
                }

                return true;
            }
        }

        public override int NumberOfControlsAllowed
        {
            get
            {
                int num = Int32.MaxValue;

                foreach (PageParserFilter filter in _filters) {
                    num = Min_(num, filter.NumberOfControlsAllowed);
                }

                return MinusOneIfSentinel_(num);
            }
        }

        public override int NumberOfDirectDependenciesAllowed
        {
            get
            {
                int num = Int32.MaxValue;

                foreach (PageParserFilter filter in _filters) {
                    num = Min_(num, filter.NumberOfDirectDependenciesAllowed);
                }

                return MinusOneIfSentinel_(num);
            }
        }

        public override int TotalNumberOfDependenciesAllowed
        {
            get
            {
                int num = Int32.MaxValue;

                foreach (PageParserFilter filter in _filters) {
                    num = Min_(num, filter.TotalNumberOfDependenciesAllowed);
                }

                return MinusOneIfSentinel_(num);
            }
        }

        public override bool AllowBaseType(Type baseType)
        {
            foreach (PageParserFilter filter in _filters) {
                if (!filter.AllowBaseType(baseType)) {
                    return false;
                }
            }

            return true;
        }

        public override bool AllowControl(Type controlType, ControlBuilder builder)
        {
            foreach (PageParserFilter filter in _filters) {
                if (!filter.AllowControl(controlType, builder)) {
                    return false;
                }
            }

            return true;
        }

        public override bool AllowServerSideInclude(string includeVirtualPath)
        {
            foreach (PageParserFilter filter in _filters) {
                if (!filter.AllowServerSideInclude(includeVirtualPath)) {
                    return false;
                }
            }

            return true;
        }

        public override bool AllowVirtualReference(string referenceVirtualPath, VirtualReferenceType referenceType)
        {
            foreach (PageParserFilter filter in _filters) {
                if (!filter.AllowVirtualReference(referenceVirtualPath, referenceType)) {
                    return false;
                }
            }

            return true;
        }

        public override CompilationMode GetCompilationMode(CompilationMode current)
        {
            // the answer of the first filter
            if (_filters.Count > 0) {
                return _filters[0].GetCompilationMode(current);
            }
            return base.GetCompilationMode(current);
        }

        public override Type GetNoCompileUserControlType()
        {
            // the answer of the first filter returning non null value
            foreach (PageParserFilter filter in _filters) {
                Type type = filter.GetNoCompileUserControlType();

                if (type != null) {
                    return type;
                }
            }

            return null;
        }

        public override bool ProcessCodeConstruct(CodeConstructType codeType, string code)
        {
            bool handled = false;

            foreach (var filter in _filters) {
                if (filter.ProcessCodeConstruct(codeType, code)) {
                    handled = true;
                }
            }

            return handled;
        }

        public override bool ProcessDataBindingAttribute(string controlId, string name, string value)
        {
            bool handled = false;

            foreach (PageParserFilter filter in _filters) {
                if (filter.ProcessDataBindingAttribute(controlId, name, value))
                    handled = true;
            }

            return handled;
        }

        public override bool ProcessEventHookup(string controlId, string eventName, string handlerName)
        {
            bool handled = false;

            foreach (PageParserFilter filter in _filters) {
                if (filter.ProcessEventHookup(controlId, eventName, handlerName)) {
                    handled = true;
                }
            }

            return handled;
        }

        public override void PreprocessDirective(string directiveName, System.Collections.IDictionary attributes)
        {
            foreach (PageParserFilter filter in _filters) {
                filter.PreprocessDirective(directiveName, attributes);
            }
        }

        public override void ParseComplete(ControlBuilder rootBuilder)
        {
            foreach (PageParserFilter filter in _filters) {
                filter.ParseComplete(rootBuilder);
            }
        }

        protected override void Initialize()
        {
            MethodInfo initializeInternalMethod 
                = typeof(PageParserFilter).GetMethod("InitializeInternal", InstPubNonpub);
            object parser = typeof(PageParserFilter).GetField("_parser", InstPubNonpub).GetValue(this);
            object virtualPath = typeof(PageParserFilter).GetField("_virtualPath", InstPubNonpub).GetValue(this);

            LoadFilters_();

            foreach (var filter in _filters) {
                initializeInternalMethod.Invoke(filter, new[] { virtualPath, parser });
            }
        }

        #region Membres privés

        void LoadFilters_()
        {
            var section 
                = WebConfigurationManager.GetSection(ParserFiltersSection.DefaultName, VirtualPath)
                    as ParserFiltersSection;

            foreach (ParserFilterElement parserFilterElement in section.ParserFilters) {
                _filters.Add((PageParserFilter)Activator.CreateInstance(parserFilterElement.ElementType, true /* nonPublic */));
            }
        }

        static int Min_(int num1, int num2)
        {
            return Math.Min(num1, num2 < 0 ? Int32.MaxValue : num2);
        }

        static int MinusOneIfSentinel_(int value)
        {
            return value == Int32.MaxValue ? -1 : value;
        }

        #endregion
    }
}