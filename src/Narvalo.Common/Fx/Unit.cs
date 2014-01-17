namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Single = new Unit();

        /// <summary />
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "Cette méthode renvoie toujours la même valeur quelques soient les paramètres.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "Cette méthode renvoie toujours la même valeur quelques soient les paramètres.")]
        public static bool operator ==(Unit left, Unit right)
        {
            return true;
        }

        /// <summary />
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left", Justification = "Cette méthode renvoie toujours la même valeur quelques soient les paramètres.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right", Justification = "Cette méthode renvoie toujours la même valeur quelques soient les paramètres.")]
        public static bool operator !=(Unit left, Unit right)
        {
            return false;
        }

        public bool Equals(Unit other)
        {
            return true;
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        /// <summary />
        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return "()";
        }
    }
}
