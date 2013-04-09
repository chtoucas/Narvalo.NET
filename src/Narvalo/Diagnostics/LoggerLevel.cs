namespace Narvalo.Diagnostics
{
    using System;

    [Flags]
    public enum LoggerLevel
    {
        None = 0,

        /// <summary>
        /// Système inutilisable.
        /// </summary>
        Emergency = 1 << 0,

        /// <summary>
        /// Une intervention immédiate est nécessaire.
        /// </summary>
        Alert = 1 << 1,

        /// <summary>
        /// Erreur critique pour le système.
        /// </summary>
        Critical = 1 << 2,

        /// <summary>
        /// Erreur de fonctionnement.
        /// </summary>
        Error = 1 << 3,

        /// <summary>
        /// Avertissement.
        /// </summary>
        Warning = 1 << 4,

        /// <summary>
        /// Événement normal méritant d'être signalé.
        /// </summary>
        Notice = 1 << 5,

        /// <summary>
        /// Pour information seulement.
        /// </summary>
        Informational = 1 << 6,

        /// <summary>
        /// Message de mise au point.
        /// </summary>
        Debug = 1 << 7,

        // Aliases
        Fatal = Emergency,
        All = Emergency | Alert | Critical | Error | Warning | Notice | Informational | Debug,
    }
}
