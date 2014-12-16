namespace Narvalo.Reliability
{
    public enum GuardStatus
    {
        Busy,

        // Unrecoverable: we're basically dead for good.
        Down,

        // Currently unavailable.
        TemporarilyDown,

        // Something's wrong, but we can still serve requests. (Degraded)
        Degraded,

        // Everything is operating as expected.
        Up,
    }
}
