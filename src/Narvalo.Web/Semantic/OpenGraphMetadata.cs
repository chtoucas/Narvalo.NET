// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public sealed class OpenGraphMetadata : IOpenGraphMetadata
    {
        private readonly IList<OpenGraphLocale> _alternativeLocales = new List<OpenGraphLocale>();
        private readonly OpenGraphLocale _locale;
        private readonly Ontology _ontology;

        private string _type = OpenGraphType.WebSite;

        private OpenGraphImage _image;

        public OpenGraphMetadata(Ontology ontology)
        {
            Require.NotNull(ontology, "ontology");

            _ontology = ontology;
            _locale = new OpenGraphLocale(ontology.Culture);
        }

        public OpenGraphImage Image
        {
            get
            {
                return _image;
            }

            set
            {
                Require.Property(value);

                _image = value;
            }
        }

        public string Title { get { return _ontology.Title; } }

        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                Require.Property(value);

                _type = value;
            }
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
            Require.NotNull(locales, "locales");

            foreach (var locale in locales) {
                _alternativeLocales.Add(locale);
            }
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_alternativeLocales != null);
            Contract.Invariant(_locale != null);
            Contract.Invariant(_ontology != null);
            Contract.Invariant(_ontology.Culture != null);
        }

#endif
    }
}