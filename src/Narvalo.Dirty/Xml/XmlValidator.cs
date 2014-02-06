// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Xml
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    // Cf. http://www.cafeconleche.org/books/xmljava/chapters/ch06s10.html
    public sealed class XmlValidator
    {
        readonly XmlReaderSettings _settings;

        bool _isValid = true;
        IList<ValidationEventArgs> _errors = new List<ValidationEventArgs>();

        public XmlValidator(XmlReaderSettings settings)
        {
            Require.NotNull(settings, "settings");

            _settings = settings;
            _settings.ValidationEventHandler += (sender, e) => {
                _isValid = false;
                _errors.Add(e);
            };
        }

        public bool IsValid { get { return _isValid; } }

        public IReadOnlyCollection<ValidationEventArgs> ValidationErrors
        {
            get { return new ReadOnlyCollection<ValidationEventArgs>(_errors); }
        }

        public bool Validate(string file)
        {
            Reset_();

            using (var reader = XmlReader.Create(file, _settings)) {
                while (reader.Read()) { }
            }

            return _isValid;
        }

        public bool Validate(TextReader input)
        {
            Reset_();

            using (var reader = XmlReader.Create(input, _settings)) {
                while (reader.Read()) { }
            }

            return _isValid;
        }

        void Reset_()
        {
            _isValid = true;
            _errors.Clear();
        }
    }
}
