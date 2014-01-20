namespace Narvalo.Fx
{
    using System;

    public partial struct Failure<T> : IEquatable<T>, IEquatable<Failure<T>> where T : Exception
    {
        readonly T _exception;

        Failure(T exception)
        {
            // NB: La seule manière d'appeler le constructeur est via la méthode Outcome<T>.η() 
            // qui se charge de vérifier que exception n'est pas null.
            _exception = exception;
        }

        public string Message
        {
            get { return _exception.Message; }
        }

        public void Throw()
        {
            throw _exception;
        }

        public override string ToString()
        {
            return _exception.ToString();
        }
    }
}