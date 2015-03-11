// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    internal class Win32GhostScriptApi : GhostScriptApiBase
    {
        public Win32GhostScriptApi() : base() { }

        [SuppressMessage("Microsoft.Usage", "CA2202:Ne pas supprimer d'objets plusieurs fois")]
        [HandleProcessCorruptedStateExceptions]
        protected override void ExecuteNative(int argc, IntPtr argv)
        {
            int code;

            using (var handle = CreateGSHandle_())
            {
                try
                {
                    code = Win32NativeMethods.gsapi_init_with_args(handle, argc, argv);
                }
                catch (Exception ex)
                {
                    // WARNING: the handle is executing in CER but the last method may throw a CSE, 
                    // in which case the finally block won't be executed.
                    handle.Dispose();

                    throw new GhostScriptCriticalStateException("Critical State Exception (CSE) caught.", ex);
                }
            }

            if (NativeMethodsUtil.IsError(code))
            {
                throw new GhostScriptException("An error occured while executing gsapi_init_with_args().");
            }
        }

        private static Win32GhostScriptSafeHandle CreateGSHandle_()
        {
            Win32GhostScriptSafeHandle handle = null;

            int code = Win32NativeMethods.gsapi_new_instance(out handle, IntPtr.Zero);

            if (handle.IsInvalid || NativeMethodsUtil.IsError(code))
            {
                throw new GhostScriptException(
                    "Apparently another instance of GhostScript is already running.");
            }

            return handle;
        }
    }
}
