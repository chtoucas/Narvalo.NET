namespace Narvalo.Oueb
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Language
    {
        public string Name;
        public string Symbol;
        public Regex CommentMatcher { get { return new Regex(@"^\s*" + Symbol); } }
        public Regex CommentFilter { get { return new Regex(@"(^#![/]|^\s*#\{)"); } }
        public IDictionary<string, string> MarkdownMaps;
        public IList<string> Ignores;
    }
}
