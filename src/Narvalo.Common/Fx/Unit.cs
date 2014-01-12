namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Single = new Unit();

        #region IEquatable<Unit>

        public bool Equals(Unit other)
        {
            return true;
        }

        #endregion

        /// <summary />
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right")]
        public static bool operator ==(Unit left, Unit right)
        {
            return true;
        }

        /// <summary />
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right")]
        public static bool operator !=(Unit left, Unit right)
        {
            return false;
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            return (obj is Unit);
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
