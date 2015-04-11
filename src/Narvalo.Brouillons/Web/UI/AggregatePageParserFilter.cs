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
        private const BindingFlags BINDING_ATTR
             = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private static readonly Type s_FilterType = typeof(PageParserFilter);

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

            MethodInfo initialize = s_FilterType.GetMethod("InitializeInternal", BINDING_ATTR);
            object parser = s_FilterType.GetField("_parser", BINDING_ATTR).GetValue(this);
            object virtualPath = s_FilterType.GetField("_virtualPath", BINDING_ATTR).GetValue(this);

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
