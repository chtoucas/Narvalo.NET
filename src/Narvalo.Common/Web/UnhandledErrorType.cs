namespace Narvalo.Web
{
    public enum UnhandledErrorType
    {
        Unknown = 0,

        InvalidViewState,
        NotFound,
        PotentiallyDangerousForm,
        PotentiallyDangerousPath,
    }
}
