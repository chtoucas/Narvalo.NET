// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;
    using System.Collections.Generic;

    public interface IOpenGraphMetadata
    {
        OpenGraphImage Image { get; set; }
        
        string Title { get; }
        
        string Type { get; set; }
        
        Uri Url { get; }

        string Description { get; }
        
        string Determiner { get; set; }
        
        OpenGraphLocale Locale { get; }

        IEnumerable<OpenGraphLocale> AlternativeLocales { get; }

        string SiteName { get; set; }

        void AddAlternativeLocale(OpenGraphLocale locale);
        
        void AddAlternativeLocales(IEnumerable<OpenGraphLocale> locales);
    }
}
