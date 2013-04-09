namespace Narvalo
{
    using System;

    [Serializable]
    public class Error
    {
        public Error(Exception exception)
        {
        }

        public Error(string errorMessage)
        {
        }

        public Error(Exception exception, string errorMessage)
        {
        }

        public string ErrorMessage { get; set; }

        public Exception Exception { get; set; }
    }
}
