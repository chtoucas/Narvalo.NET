namespace Narvalo.Fx
{
    using System;

    public partial struct Identity<T> : IEquatable<Identity<T>>, IEquatable<T> where T : class
    {
        readonly T _value;

        Identity(T value)
        {
            // NB: La seule manière d'appeler le constructeur est via la méthode Outcome<T>.η() 
            // qui se charge de vérifier que "value" n'est pas null.
            _value = value;
        }

        public T Value { get { return _value; } }
    }
}
