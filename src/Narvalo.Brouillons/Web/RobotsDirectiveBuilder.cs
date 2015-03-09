// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Pour une liste complète des directives possibles, cf.
    // https://developers.google.com/webmasters/control-crawl-index/docs/robots_meta_tag?hl=fr
    // TODO: Manque les directives suivantes : all, none, unavailable_after.
    public class RobotsDirectiveBuilder
    {
        private IList<String> _values = new List<String>();

        public RobotsDirectiveBuilder Follow()
        {
            Append_("follow");
            return this;
        }

        public RobotsDirectiveBuilder Index()
        {
            Append_("index");
            return this;
        }

        public RobotsDirectiveBuilder NoArchive()
        {
            Append_("noarchive");
            return this;
        }

        public RobotsDirectiveBuilder NoIndex()
        {
            Append_("noindex");
            return this;
        }

        public RobotsDirectiveBuilder NoFollow()
        {
            Append_("nofollow");
            return this;
        }

        public RobotsDirectiveBuilder NoSnippet()
        {
            Append_("nosnippet");
            return this;
        }

        public RobotsDirectiveBuilder NoOdp()
        {
            Append_("noodp");
            return this;
        }

        public RobotsDirectiveBuilder NoTranslate()
        {
            Append_("notranslate");
            return this;
        }

        public RobotsDirectiveBuilder NoImageIndex()
        {
            Append_("noimageindex");
            return this;
        }

        public override string ToString()
        {
            return String.Join(",", _values.ToArray());
        }

        private void Append_(string value)
        {
            _values.Add(value);
        }
    }
}
