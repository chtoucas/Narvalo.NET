namespace Narvalo.Web
{
    using System.Web.Mvc;

    public static class HttpVerbsExtensions
    {
        public static bool Contains(this HttpVerbs @this, string httpMethod)
        {
            return MayParse.ToEnum<HttpVerbs>(httpMethod, true /* ignoreCase */)
                   .Map(_ => @this.HasFlag(_))
                   .ValueOrElse(false);
        }
    }
}
