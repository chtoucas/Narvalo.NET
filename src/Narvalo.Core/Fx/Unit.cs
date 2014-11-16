// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    //// FIXME_PCL: Serializable
    //// [Serializable]
    public struct Unit : IEquatable<Unit>
    {
        public static readonly Unit Single = new Unit();

        /// <summary />
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left",
            Justification = "This method always returns the same result.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right",
            Justification = "This method always returns the same result.")]
        public static bool operator ==(Unit left, Unit right)
        {
            return true;
        }

        /// <summary />
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "left",
            Justification = "This method always returns the same result.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "right",
            Justification = "This method always returns the same result.")]
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
