namespace Narvalo.Fx
{
    using System;

    public partial struct Identity<T> : IEquatable<Identity<T>>, IEquatable<T> where T : class
    {
        readonly T _value;

        Identity(T value)
        {
            _value = value;
        }

        public T Value { get { return _value; } }
    }
}
