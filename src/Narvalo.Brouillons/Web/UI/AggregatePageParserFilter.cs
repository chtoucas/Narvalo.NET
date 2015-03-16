// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    // Ce code est basé sur la classe Omari.Web.UI.AggregateParserFilter.

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Configuration;
    using System.Web.UI;
    using Narvalo.Web.Configuration;

    /// <summary>
    /// Représente un filtre composé d'une collection de <see cref="System.Web.UI.PageParserFilter"/>
    /// telle que spécifiée dans le Web.config de l'application.
    /// </summary>
    public class AggregatePageParserFilter : AggregatePageParserFilterBase
    {
        const BindingFlags BindingAttr_
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        static readonly Type FilterType_ = typeof(PageParserFilter);

        /// <summary>
        /// Initialise un objet de type <see cref="Narvalo.Web.UI.AggregatePageParserFilter"/>.
        /// </summary>
        public AggregatePageParserFilter() : base() { }

        protected override IEnumerable<PageParserFilter> InitializeFilters()
        {
            var section
                = WebConfigurationManager.GetSection(ParserFiltersSection.DefaultName, VirtualPath)
                    as ParserFiltersSection;

            if (section == null)
            {
                throw new NotSupportedException("You forgot to create a ParserFiltersSection configuration.");
            }

            MethodInfo initialize = FilterType_.GetMethod("InitializeInternal", BindingAttr_);
            object parser = FilterType_.GetField("_parser", BindingAttr_).GetValue(this);
            object virtualPath = FilterType_.GetField("_virtualPath", BindingAttr_).GetValue(this);

            var filters = new List<PageParserFilter>();

            foreach (ParserFilterElement element in section.ParserFilters)
            {
                var filter = (PageParserFilter)Activator.CreateInstance(element.ElementType, true /* nonPublic */);
                initialize.Invoke(filter, new[] { virtualPath, parser });

                filters.Add(filter);
            }

            return filters;
        }
    }
}
