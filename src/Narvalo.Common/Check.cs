namespace Narvalo
{
    public static class Check
    {
        public static T NotNull<T>(T value) where T : class
        {
            Requires.Property(value);

            return value;
        }

        public static string NotNullOrEmpty(string value)
        {
            Requires.PropertyNotEmpty(value);

            return value;
        }
    }
}
