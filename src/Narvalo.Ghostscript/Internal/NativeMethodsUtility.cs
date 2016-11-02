// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Internal
{
    internal static class NativeMethodsUtility
    {
        private const int SUCCESS_CODE = 0;

        // const int QuitCode = -101;
        // const int InterruptCode = ;
        // const int NeedInputCode = -106;
        // const int InfoCode = ;
        private const int FATAL_CODE = -100;

        // TODO: Often if GhostScript fails because you've passed the wrong combination of params etc,
        // it still returns zero... unhelpful
        public static bool IsSuccess(int code) => code == SUCCESS_CODE;

        public static bool IsError(int code) => code < SUCCESS_CODE;

        // En cas d'erreur fatal il faut appeler tout de suite gsapi_exit()
        public static bool IsFatal(int code) => code <= FATAL_CODE;
    }
}
