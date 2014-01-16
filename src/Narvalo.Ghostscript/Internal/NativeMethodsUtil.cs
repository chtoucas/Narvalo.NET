namespace Narvalo.GhostScript.Internal
{
    static class NativeMethodsUtil
    {
        const int SuccessCode_ = 0;
        // const int QuitCode = -101;
        // const int InterruptCode = ;
        // const int NeedInputCode = -106;
        // const int InfoCode = ;
        const int FatalCode_ = -100;

        public static bool IsSuccess(int code)
        {
            // TODO: Often if GhostScript fails because you've passed the wrong combination of params etc,
            // it still returns zero... unhelpful
            return code == SuccessCode_;
        }

        public static bool IsError(int code)
        {
            return code < SuccessCode_;
        }

        // En cas d'erreur fatal il faut appeler tout de suite gsapi_exit()
        public static bool IsFatal(int code)
        {
            return code <= FatalCode_;
        }
    }
}