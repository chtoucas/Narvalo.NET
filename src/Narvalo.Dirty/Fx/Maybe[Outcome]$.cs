namespace Narvalo.Fx
{
    public static class MaybeOutcomeExtensions
    {
        public static bool Successful<T>(this Maybe<Outcome<T>> @this)
        {
            return @this.IsSome && @this.Value.Successful;
        }
    }
}
