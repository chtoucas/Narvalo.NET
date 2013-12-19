namespace Narvalo.Web.Semantic
{
    using System;
    using System.Collections.Generic;
    using Narvalo;

    public class OpenGraphMetadata : IOpenGraphMetadata
    {
        readonly IList<OpenGraphLocale> _alternativeLocales = new List<OpenGraphLocale>();
        readonly OpenGraphLocale _locale;
        readonly Ontology _ontology;

        string _type = OpenGraphType.WebSite;

        OpenGraphImage _image;

        public OpenGraphMetadata(Ontology ontology)
        {
            Requires.NotNull(ontology, "ontology");

            _ontology = ontology;
            _locale = new OpenGraphLocale(ontology.Culture);
        }

        #region IOpenGraphMetadata

        public OpenGraphImage Image
        {
            get { return _image; }
            set { Requires.NotNull(value, "value"); _image = value; }
        }

        public string Title { get { return _ontology.Title; } }

        public string Type
        {
            get { return _type; }
            set { Requires.NotNullOrEmpty(value, "value"); _type = value; }
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

        #endregion
    }
}