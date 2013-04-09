namespace Narvalo.Web.Internal
{
    using System.Collections.Generic;
    using System.Web.Routing;

    internal static class TypeHelpers
    {
        public static IDictionary<string, object> ObjectToDictionary(object value)
        {
            return new RouteValueDictionary(value);
        }
    }
}
