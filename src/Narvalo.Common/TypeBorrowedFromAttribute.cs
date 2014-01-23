namespace Narvalo
{
    using System;

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class TypeBorrowedFromAttribute : Attribute
    {
        readonly string _genuineName;

        string _remark = String.Empty;

        public TypeBorrowedFromAttribute(string genuineName)
        {
            Require.NotNullOrEmpty(genuineName, "genuineName");

            _genuineName = genuineName;
        }


        public string GenuineName
        {
            get { return _genuineName; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
