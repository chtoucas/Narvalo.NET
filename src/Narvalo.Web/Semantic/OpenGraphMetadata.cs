namespace Narvalo.Web.Semantic
{
    using System;
    using System.Collections.Generic;
    using Narvalo;

    public sealed class OpenGraphMetadata : IOpenGraphMetadata
    {
        readonly IList<OpenGraphLocale> _alternativeLocales = new List<OpenGraphLocale>();
        readonly OpenGraphLocale _locale;
        readonly Ontology _ontology;

        string _type = OpenGraphType.WebSite;

        OpenGraphImage _image;

        public OpenGraphMetadata(Ontology ontology)
        {
            Require.NotNull(ontology, "ontology");

            _ontology = ontology;
            _locale = new OpenGraphLocale(ontology.Culture);
        }

        public OpenGraphImage Image
        {
            get { return _image; }
            set { _image = Require.Property(value); }
        }

        public string Title { get { return _ontology.Title; } }

        public string Type
        {
            get { return _type; }
            set { _type = Require.PropertyNotEmpty(value); }
        }

        public Uri Url { get { return _ontology.Relationships.CanonicalUrl; } }

        public string Description { get { return _ontology.Description; } }

        public string Determiner { get; set; }

        public OpenGraphLocale Locale { get { return _locale; } }

        public IEnumerable<OpenGraphLocale> AlternativeLocales
        {
            get
            {
                return _alternativeLocales;
            }
        }

        public string SiteName { get; set; }

        public void AddAlternativeLocale(OpenGraphLocale locale)
        {
            _alternativeLocales.Add(locale);
        }

        public void AddAlternativeLocales(IEnumerable<OpenGraphLocale> locales)
        {
            foreach (var locale in locales) {
                _alternativeLocales.Add(locale);
            }
        }
    }
}