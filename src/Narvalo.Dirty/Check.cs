namespace Narvalo
{
    public static class Check
    {
        public static T Property<T>(T value) where T : class
        {
            Requires.Property(value);

            return value;
        }

        public static T NotNull<T>(T value, string parameterName) where T : class
        {
            Requires.NotNull(value, parameterName);

            return value;
        }

        //public static T? NotNull<T>(T? value, string parameterName) where T : struct
        //{
        //    if (value == null) {
        //        throw new ArgumentNullException(parameterName);
        //    }

        //    return value;
        //}

        public static string NotNullOrEmpty(string value, string parameterName)
        {
            Requires.NotNullOrEmpty(value, parameterName);

            return value;
        }
    }
}
