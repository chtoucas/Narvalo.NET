namespace Narvalo.Reliability
{
    internal static class GuardExtensions
    {
        public static bool IsChainable(this IGuard guard)
        {
            Require.NotNull(guard, "guard");

            return guard.Multiplicity == 1;
        }
    }
}
