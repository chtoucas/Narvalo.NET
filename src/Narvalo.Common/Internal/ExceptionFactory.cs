namespace Narvalo.Internal
{
    using System;

    static class ExceptionFactory
    {
        public static ArgumentNullException ArgumentNull(string parameterName)
        {
            return new ArgumentNullException(parameterName, Format.CurrentCulture(SR.ExceptionFactory_ArgumentNullFormat, parameterName));
        }
    }
}
