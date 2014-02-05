namespace Narvalo.Fx
{
    using System;

    public sealed partial class Identity<T> : IEquatable<Identity<T>>, IEquatable<T>
    {
        readonly T _value;

        Identity(T value)
        {
            _value = value;
        }

        public T Value { get { return _value; } }
    }
}
