namespace Narvalo.Runtime.Reliability
{
    using Narvalo.Diagnostics;

    internal static class GuardExtensions
    {
        public static bool IsChainable(this IGuard guard)
        {
            Requires.NotNull(guard, "guard");

            return guard.Multiplicity == 1;
        }
    }
}
