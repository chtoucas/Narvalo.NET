namespace Narvalo.Web
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.UI;

    public static class HttpApplicationUtility
    {
        public static UnhandledErrorType GetUnhandledErrorType(Exception ex)
        {
            Requires.NotNull(ex, "ex");

            var httpException = ex as HttpException;
            if (httpException == null) {
                return UnhandledErrorType.Unknown;
            }

            Type type = ex.GetType();

            UnhandledErrorType err;

            if (type == typeof(HttpRequestValidationException)) {
                err = UnhandledErrorType.PotentiallyDangerousForm;
            }
            else if (type == typeof(ViewStateException)) {
                err = UnhandledErrorType.InvalidViewState;
            }
            else {
                int httpCode = httpException.GetHttpCode();

                if (httpCode == (int)HttpStatusCode.NotFound) {
                    err = UnhandledErrorType.NotFound;
                }
                else if (httpCode == (int)HttpStatusCode.BadRequest) {
                    // WARNING: on ne peut déterminer de manière certaine l'origine de l'erreur,
                    // mais dans la plupart des cas il s'agit d'une requête contenant des
                    // caractères spéciaux.
                    err = UnhandledErrorType.PotentiallyDangerousPath;
                    // FIXME: est-ce une bonne idée de changer ex ?
                    ex = new HttpRequestPathException(ex.Message, ex);
                }
                else {
                    err = UnhandledErrorType.Unknown;
                }
            }

            return err;
        }
    }
}
