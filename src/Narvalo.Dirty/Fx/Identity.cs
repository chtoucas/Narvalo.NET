namespace Narvalo.Fx
{
    public static class Identity
    {
        public static Identity<T> Create<T>(T value) where T : class
        {
            return Identity<T>.η(value);
        }
    }
}
