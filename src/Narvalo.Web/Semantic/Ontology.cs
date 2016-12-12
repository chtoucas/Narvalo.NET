// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public sealed class Ontology
    {
        public const string OpenGraphNamespace = "og: http://ogp.me/ns#";

        private readonly CultureInfo _culture;
        private readonly Relationships _relationships = new Relationships();
        private readonly SchemaOrgVocabulary _schemaOrg = new SchemaOrgVocabulary();
        private readonly IOpenGraphMetadata _openGraph;

        private string _keywords = String.Empty;
        private string _robotsDirective = "index, follow";

        public Ontology(CultureInfo culture)
        {
            Require.NotNull(culture, nameof(culture));

            _culture = culture;
            _openGraph = new OpenGraphMetadata(this);
        }

        public CultureInfo Culture
        {
            get
            {
                Warrant.NotNull<CultureInfo>();

                return _culture;
            }
        }

        public string LanguageName
        {
            get
            {
                Warrant.NotNull<string>();

                return Culture.TwoLetterISOLanguageName;
            }
        }

        public IOpenGraphMetadata OpenGraph
        {
            get
            {
                Warrant.NotNull<IOpenGraphMetadata>();

                return _openGraph;
            }
        }

        public Relationships Relationships
        {
            get
            {
                Warrant.NotNull<Relationships>();

                return _relationships;
            }
        }

        public SchemaOrgVocabulary SchemaOrg
        {
            get
            {
                Warrant.NotNull<SchemaOrgVocabulary>();

                return _schemaOrg;
            }
        }

        public string RobotsDirective { get { return _robotsDirective; } set { _robotsDirective = value; } }

        public string Description { get; set; }

        public string Keywords { get { return _keywords; } set { _keywords = value; } }

        public string Title { get; set; }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_culture != null);
            Contract.Invariant(_openGraph != null);
            Contract.Invariant(_relationships != null);
            Contract.Invariant(_schemaOrg != null);
        }

#endif
    }
}