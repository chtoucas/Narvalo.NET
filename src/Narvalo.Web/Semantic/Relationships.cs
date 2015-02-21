// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Semantic
{
    using System;

    public sealed class Relationships
    {
        // FIXME: Prendre en compte les sites web dans un répertoire virtuel.
        private Uri _humansTxtUrl = new Uri("/human.txt", UriKind.Relative);

        public Uri CanonicalUrl { get; set; }
        
        public Uri HumansTxtUrl { get { return _humansTxtUrl; } set { _humansTxtUrl = value; } }
    }
}
