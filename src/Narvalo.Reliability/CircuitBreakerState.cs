// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    public enum CircuitBreakerState
    {
        /// <summary>
        /// Le circuit est fermé et accepte toutes les demandes.
        /// </summary>
        Closed,

        /// <summary>
        /// Le circuit est semi-ouvert, une seule opération sera executée.
        /// </summary>
        HalfOpen,

        /// <summary>
        /// Le circuit est ouvert et ne laisse passer aucune opération.
        /// </summary>
        Open,

        // Synonyms
        HalfClosed = HalfOpen,
    }
}
