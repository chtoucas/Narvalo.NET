namespace Narvalo
{
    public static class Check
    {
        public static T Property<T>(T value) where T : class
        {
            Requires.Property(value);

            return value;
        }

        public static string PropertyNotEmpty(string value)
        {
            Requires.PropertyNotEmpty(value);

            return value;
        }
    }
}
