// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    // Ce code est basé sur la classe Omari.Web.UI.AggregateParserFilter.

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;

    /// <summary>
    /// Représente un filtre composé d'une collection de <see cref="System.Web.UI.PageParserFilter"/>.
    /// </summary>
    /// <remarks>
    /// Plutôt que d'utiliser cette classe, il est préférable de créer directement un PageParserFilter.
    /// </remarks>
    public abstract class AggregatePageParserFilterBase : PageParserFilter
    {
        const int Sentinel_ = Int32.MaxValue;

        IEnumerable<PageParserFilter> _filters;

        /// <summary>
        /// Initialise un objet de type <see cref="Narvalo.Web.UI.AggregatePageParserFilterBase"/>.
        /// </summary>
        protected AggregatePageParserFilterBase() : base() { }

        /// <summary>
        /// Initialise la liste des filtres dont est composé ce filtre.
        /// </summary>
        /// <returns>Retourne la liste de filtres à utiliser.</returns>
        protected abstract IEnumerable<PageParserFilter> InitializeFilters();

        public override bool AllowCode
        {
            get
            {
                foreach (var filter in _filters)
                {
                    if (!filter.AllowCode)
                    {
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
                int num = Sentinel_;

                foreach (var filter in _filters)
                {
                    num = Min_(num, filter.NumberOfControlsAllowed);
                }

                return UnlimitedNumberIfSentinel_(num);
            }
        }

        public override int NumberOfDirectDependenciesAllowed
        {
            get
            {
                int num = Sentinel_;

                foreach (var filter in _filters)
                {
                    num = Min_(num, filter.NumberOfDirectDependenciesAllowed);
                }

                return UnlimitedNumberIfSentinel_(num);
            }
        }

        public override int TotalNumberOfDependenciesAllowed
        {
            get
            {
                int num = Sentinel_;

                foreach (var filter in _filters)
                {
                    num = Min_(num, filter.TotalNumberOfDependenciesAllowed);
                }

                return UnlimitedNumberIfSentinel_(num);
            }
        }

        public override bool AllowBaseType(Type baseType)
        {
            foreach (var filter in _filters)
            {
                if (!filter.AllowBaseType(baseType))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool AllowControl(Type controlType, ControlBuilder builder)
        {
            foreach (var filter in _filters)
            {
                if (!filter.AllowControl(controlType, builder))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool AllowServerSideInclude(string includeVirtualPath)
        {
            foreach (var filter in _filters)
            {
                if (!filter.AllowServerSideInclude(includeVirtualPath))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool AllowVirtualReference(string referenceVirtualPath, VirtualReferenceType referenceType)
        {
            foreach (var filter in _filters)
            {
                if (!filter.AllowVirtualReference(referenceVirtualPath, referenceType))
                {
                    return false;
                }
            }

            return true;
        }

        public override CompilationMode GetCompilationMode(CompilationMode current)
        {
            var filter = _filters.FirstOrDefault();

            if (filter != null)
            {
                return filter.GetCompilationMode(current);
            }

            return base.GetCompilationMode(current);
        }

        public override Type GetNoCompileUserControlType()
        {
            foreach (var filter in _filters)
            {
                Type type = filter.GetNoCompileUserControlType();

                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        public override void ParseComplete(ControlBuilder rootBuilder)
        {
            foreach (var filter in _filters)
            {
                filter.ParseComplete(rootBuilder);
            }
        }

        public override void PreprocessDirective(string directiveName, IDictionary attributes)
        {
            foreach (var filter in _filters)
            {
                filter.PreprocessDirective(directiveName, attributes);
            }
        }

        public override bool ProcessCodeConstruct(CodeConstructType codeType, string code)
        {
            foreach (var filter in _filters)
            {
                if (filter.ProcessCodeConstruct(codeType, code))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool ProcessDataBindingAttribute(string controlId, string name, string value)
        {
            foreach (var filter in _filters)
            {
                if (filter.ProcessDataBindingAttribute(controlId, name, value))
                {
                    return true;
                }
            }

            return false;
        }

        public override bool ProcessEventHookup(string controlId, string eventName, string handlerName)
        {
            foreach (var filter in _filters)
            {
                if (filter.ProcessEventHookup(controlId, eventName, handlerName))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _filters = InitializeFilters();
        }

        static int Min_(int num1, int num2)
        {
            return Math.Min(num1, num2 < 0 ? Sentinel_ : num2);
        }

        static int UnlimitedNumberIfSentinel_(int value)
        {
            return value == Sentinel_ ? PageParserFilterConstants.UnlimitedNumber : value;
        }
    }
}
