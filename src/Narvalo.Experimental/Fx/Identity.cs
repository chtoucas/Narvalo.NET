namespace Narvalo.Fx
{
    public static class Identity
    {
        public static Identity<T> Create<T>(T value)
        {
            return new Identity<T>(value);
        }
    }
}
