namespace Narvalo.Web.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Web;

    internal class TagBuilder
    {
        readonly static string DefaultIdAttributeDotReplacement = "_";
        //= WebPages.Html.HtmlHelper.IdAttributeDotReplacement;

        readonly string _tagName;

        IDictionary<string, string> _attributes
            = new SortedDictionary<string, string>(StringComparer.Ordinal);
        string _idAttributeDotReplacement = DefaultIdAttributeDotReplacement;
        string _innerHtml = String.Empty;

        public TagBuilder(string tagName)
        {
            Requires.NotNullOrEmpty(tagName, "tagName");

            _tagName = tagName;
        }

        public IDictionary<string, string> Attributes
        {
            get { return _attributes; }
        }

        public string IdAttributeDotReplacement
        {
            get { return _idAttributeDotReplacement; }
            set
            {
                Requires.NotNullOrEmpty(value, "value");
                _idAttributeDotReplacement = value;
            }
        }

        public string InnerHtml
        {
            get { return _innerHtml; }
            set { _innerHtml = value; }
        }

        public string TagName
        {
            get { return _tagName; }
        }

        //public static string CreateSanitizedId(string originalId) {
        //    return CreateSanitizedId(originalId, DefaultIdAttributeDotReplacement);
        //}

        public static string CreateSanitizedId(string originalId, string invalidCharReplacement)
        {
            //Requires.NotNullOrEmpty(originalId, "originalId");
            Requires.NotNullOrEmpty(invalidCharReplacement, "invalidCharReplacement");

            if (String.IsNullOrEmpty(originalId)) {
                return null;
            }

            //if (invalidCharReplacement == null) {
            //    throw new ArgumentNullException("invalidCharReplacement");
            //}

            char firstChar = originalId[0];
            if (!Html401IdUtil.IsLetter(firstChar)) {
                // the first character must be a letter
                return null;
            }

            var sb = new StringBuilder(originalId.Length);
            sb.Append(firstChar);

            for (int i = 1; i < originalId.Length; i++) {
                char thisChar = originalId[i];
                if (Html401IdUtil.IsValidIdCharacter(thisChar)) {
                    sb.Append(thisChar);
                }
                else {
                    sb.Append(invalidCharReplacement);
                }
            }

            return sb.ToString();
        }

        public void AddCssClass(string value)
        {
            string currentValue;

            if (_attributes.TryGetValue("class", out currentValue)) {
                _attributes["class"] = value + " " + currentValue;
            }
            else {
                _attributes["class"] = value;
            }
        }

        public void GenerateId(string name)
        {
            if (_attributes.ContainsKey("id")) {
                return;
            }

            string sanitizedId = CreateSanitizedId(name, IdAttributeDotReplacement);
            if (!String.IsNullOrEmpty(sanitizedId)) {
                _attributes["id"] = sanitizedId;
            }
        }

        public void MergeAttribute(string key, string value)
        {
            MergeAttribute(key, value, false /* replaceExisting */);
        }

        public void MergeAttribute(string key, string value, bool replaceExisting)
        {
            Requires.NotNullOrEmpty(key, "key");

            if (replaceExisting || !_attributes.ContainsKey(key)) {
                _attributes[key] = value;
            }
        }

        public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
        {
            MergeAttributes(attributes, false /* replaceExisting */);
        }

        public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting)
        {
            if (attributes == null) {
                return;
            }

            foreach (var entry in attributes) {
                string key = Convert.ToString(entry.Key, CultureInfo.InvariantCulture);
                string value = Convert.ToString(entry.Value, CultureInfo.InvariantCulture);
                MergeAttribute(key, value, replaceExisting);
            }
        }

        public void SetInnerText(string innerText)
        {
            InnerHtml = HttpUtility.HtmlEncode(innerText);
        }

        public IHtmlString ToHtmlString()
        {
            return ToHtmlString(TagRenderMode.Normal);
        }

        public IHtmlString ToHtmlString(TagRenderMode renderMode)
        {
            return new HtmlString(ToString(renderMode));
        }

        public override string ToString()
        {
            return ToString(TagRenderMode.Normal);
        }

        public string ToString(TagRenderMode renderMode)
        {
            var sb = new StringBuilder();
            switch (renderMode) {
                case TagRenderMode.StartTag:
                    sb.Append('<')
                        .Append(_tagName);
                    AppendAttributes(sb);
                    sb.Append('>');
                    break;
                case TagRenderMode.EndTag:
                    sb.Append("</")
                        .Append(_tagName)
                        .Append('>');
                    break;
                case TagRenderMode.SelfClosing:
                    sb.Append('<')
                        .Append(_tagName);
                    AppendAttributes(sb);
                    sb.Append(" />");
                    break;
                default:
                    sb.Append('<')
                        .Append(_tagName);
                    AppendAttributes(sb);
                    sb.Append('>')
                        .Append(InnerHtml)
                        .Append("</")
                        .Append(_tagName)
                        .Append('>');
                    break;
            }
            return sb.ToString();
        }

        void AppendAttributes(StringBuilder sb)
        {
            foreach (var attribute in _attributes) {
                string key = attribute.Key;

                if (String.Equals(key, "id", StringComparison.Ordinal /* case-sensitive */)
                    && String.IsNullOrEmpty(attribute.Value)) {
                    continue; // DevDiv Bugs #227595: don't output empty IDs
                }
                string value = HttpUtility.HtmlAttributeEncode(attribute.Value);

                sb.Append(' ').Append(key);
                if (value.Contains(" ")) {
                    sb.Append("=\"").Append(value).Append('"');
                }
                else {
                    sb.Append('=').Append(value);
                }
            }
        }

        // Valid IDs are defined in http://www.w3.org/TR/html401/types.html#type-id
        static class Html401IdUtil
        {
            public static bool IsLetter(char c)
            {
                return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
            }

            public static bool IsValidIdCharacter(char c)
            {
                return IsLetter(c) || IsDigit(c) || IsAllowableSpecialCharacter(c);
            }

            static bool IsAllowableSpecialCharacter(char c)
            {
                switch (c) {
                    case '-':
                    case '_':
                    case ':':
                        // note that we're specifically excluding the '.' character
                        return true;

                    default:
                        return false;
                }
            }

            static bool IsDigit(char c)
            {
                return '0' <= c && c <= '9';
            }
        }
    }
}
