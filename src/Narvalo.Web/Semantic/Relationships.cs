namespace Narvalo.Web.Semantic
{
    using System;

    public sealed class Relationships
    {
        Uri _humansTxtUrl = new Uri("/human.txt", UriKind.Relative);

        public Uri CanonicalUrl { get; set; }
        
        public Uri HumansTxtUrl { get { return _humansTxtUrl; } set { _humansTxtUrl = value; } }
    }
}
