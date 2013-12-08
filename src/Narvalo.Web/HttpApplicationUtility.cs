namespace Narvalo.Web
{
    using System;
    //using System.Security;
    using System.Net;
    using System.Web;
    using System.Web.UI;

    public static class HttpApplicationUtility
    {
        //static readonly AspNetHostingPermissionLevel[] TrustLevels_ = {
        //    AspNetHostingPermissionLevel.Unrestricted,
        //    AspNetHostingPermissionLevel.High,
        //    AspNetHostingPermissionLevel.Medium,
        //    AspNetHostingPermissionLevel.Low,
        //    AspNetHostingPermissionLevel.Minimal
        //};

        ///// <summary>
        ///// Récupération du niveau de confiance AspNet.
        ///// </summary>
        ///// <returns></returns>
        //public static AspNetHostingPermissionLevel FindCurrentTrustLevel()
        //{
        //    foreach (var trustLevel in TrustLevels_) {
        //        try {
        //            new AspNetHostingPermission(trustLevel).Demand();
        //        }
        //        catch (SecurityException) {
        //            continue;
        //        }

        //        return trustLevel;
        //    }

        //    return AspNetHostingPermissionLevel.None;
        //}

        ///// <see cref="SquishIt"/> et <see cref="http://www.encosia.com"/>
        //public static bool IsDebuggingEnabled()
        //{
        //    if (HttpContext.Current != null && HttpContext.Current.IsDebuggingEnabled) {
        //        // Check retail setting in machine.config
        //        Configuration machineConfig = ConfigurationManager.OpenMachineConfiguration();
        //        var group = machineConfig.GetSectionGroup("system.web");
        //        if (group != null) {
        //            var appSettingSection = (DeploymentSection)group.Sections["deployment"];
        //            if (appSettingSection.Retail) {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //    return false;
        //}

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
