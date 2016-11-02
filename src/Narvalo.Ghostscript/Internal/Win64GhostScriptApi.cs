// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Internal
{
    using System;
    using System.Runtime.ExceptionServices;

    internal sealed class Win64GhostScriptApi : GhostScriptApi
    {
        public Win64GhostScriptApi() : base() { }

        [HandleProcessCorruptedStateExceptions]
        protected override void ExecuteNative(int argc, IntPtr argv)
        {
            int code;

            using (Win64GhostScriptSafeHandle handle = CreateGSHandle())
            {
                try
                {
                    code = Win64NativeMethods.gsapi_init_with_args(handle, argc, argv);
                }
                catch (Exception ex)
                {
                    // WARNING: the handle is executing in CER but the last method may throw a CSE,
                    // in which case the finally block won't be executed.
                    handle.Dispose();

                    throw new GhostScriptCriticalStateException("Critical State Exception (CSE) caught.", ex);
                }
            }

            if (NativeMethodsUtility.IsError(code))
            {
                throw new GhostScriptException("An error occured while executing gsapi_init_with_args().");
            }
        }

        private static Win64GhostScriptSafeHandle CreateGSHandle()
        {
            Win64GhostScriptSafeHandle handle = null;

            int code = Win64NativeMethods.gsapi_new_instance(out handle, IntPtr.Zero);

            if (handle.IsInvalid || NativeMethodsUtility.IsError(code))
            {
                throw new GhostScriptException(
                    "It seems another instance of GhostScript is running.");
            }

            return handle;
        }
    }
}
