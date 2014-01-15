namespace Narvalo.Web.Semantic
{
    using System;
    using System.Collections.Generic;

    public interface IOpenGraphMetadata
    {
        // > Propriétés obligatoires <

        OpenGraphImage Image { get; set; }
        string Title { get; }
        string Type { get; set; }
        Uri Url { get; }

        // > Propriétés facultatives <

        string Description { get; }
        string Determiner { get; set; }
        OpenGraphLocale Locale { get; }

        IEnumerable<OpenGraphLocale> AlternativeLocales { get; }

        string SiteName { get; set; }

        // > Méthodes <

        void AddAlternativeLocale(OpenGraphLocale locale);
        void AddAlternativeLocales(IEnumerable<OpenGraphLocale> locales);
    }
}
