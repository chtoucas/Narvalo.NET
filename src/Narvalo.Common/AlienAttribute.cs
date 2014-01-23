namespace Narvalo
{
    using System;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class AlienAttribute : Attribute
    {
        readonly string _origin;

        string _remark = String.Empty;

        public AlienAttribute(string origin)
        {
            Require.NotNullOrEmpty(origin, "origin");

            _origin = origin;
        }

        public string Origin
        {
            get { return _origin; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
