namespace Narvalo
{
    using System;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class AlienAttribute : Attribute
    {
        readonly AlienSource _source;

        string _link = String.Empty;
        string _genuineName = String.Empty;
        string _remark = String.Empty;

        public AlienAttribute(AlienSource source)
        {
            _source = source;
        }

        public AlienSource Source
        {
            get { return _source; }
        }

        public string GenuineName
        {
            get { return _genuineName; }
            set { _genuineName = value; }
        }

        public string Link
        {
            get { return _link; }
            set { _link = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
