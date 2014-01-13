namespace Narvalo
{
    public sealed class Singleton<T> where T : class, new()
    {
        Singleton() { }

        public static T Instance
        {
            get { return Inner.Instance; }
        }

        class Inner
        {
            static Inner() { }
            internal static readonly T Instance = new T();
        }
    }
}
