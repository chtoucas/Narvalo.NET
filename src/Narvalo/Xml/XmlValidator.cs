namespace Narvalo.Xml
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml;
    using System.Xml.Schema;

    // Cf. http://www.cafeconleche.org/books/xmljava/chapters/ch06s10.html
    public class XmlValidator
    {
        readonly XmlReaderSettings _settings;

        bool _isValid = true;
        IList<ValidationEventArgs> _errors = new List<ValidationEventArgs>();

        public bool IsValid { get { return _isValid; } }

        public XmlValidator(XmlReaderSettings settings)
        {
            _settings = settings;
            _settings.ValidationEventHandler += (object sender, ValidationEventArgs e) => {
                _isValid = false;
                _errors.Add(e);
            };
        }

        public ReadOnlyCollection<ValidationEventArgs> ValidationErrors
        {
            get { return new ReadOnlyCollection<ValidationEventArgs>(_errors); }
        }

        public bool Validate(string file)
        {
            Reset_();

            using (var reader = XmlReader.Create(file, _settings)) {
                while (reader.Read()) { ; }
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
