// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.GhostScript.Internal
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    internal static class Win64NativeMethods
    {
        private const string DLL_NAME = "gsdll64.dll";

        // int gsapi_new_instance (void **pinstance, void *caller_handle);
        [DllImport(DLL_NAME, EntryPoint = "gsapi_new_instance", SetLastError = true)]
        public static extern int gsapi_new_instance(out Win64GhostScriptSafeHandle pinstance, IntPtr caller_handle);

        // void gsapi_delete_instance (void *instance);
        [DllImport(DLL_NAME, EntryPoint = "gsapi_delete_instance", SetLastError = true)]
        public static extern void gsapi_delete_instance(IntPtr handle);

        // int gsapi_init_with_args (void *instance, int argc, char **argv);
        [DllImport(DLL_NAME, EntryPoint = "gsapi_init_with_args", SetLastError = true)]

        // WARNING: this method may raise a CSE: SecurityCriticalAttribute
        [SecurityCritical]
        public static extern int gsapi_init_with_args(Win64GhostScriptSafeHandle instance, int argc, IntPtr argv);

        // int gsapi_exit (void *instance);
        [DllImport(DLL_NAME, EntryPoint = "gsapi_exit", SetLastError = true)]
        public static extern int gsapi_exit(IntPtr handle);

        //// int gsapi_revision (gsapi_revision_t *pr, int len);
        //// int gsapi_set_stdio (void *instance, int(*stdin_fn)(void *caller_handle, char *buf, int len), int(*stdout_fn)(void *caller_handle, const char *str, int len), int(*stderr_fn)(void *caller_handle, const char *str, int len));
        //// int gsapi_set_poll (void *instance, int(*poll_fn)(void *caller_handle));
        //// int gsapi_set_display_callback (void *instance, display_callback *callback);
        //// int gsapi_run_string_begin (void *instance, int user_errors, int *pexit_code);
        //// int gsapi_run_string_continue (void *instance, const char *str, unsigned int length, int user_errors, int *pexit_code);
        //// int gsapi_run_string_end (void *instance, int user_errors, int *pexit_code);
        //// int gsapi_run_string_with_length (void *instance, const char *str, unsigned int length, int user_errors, int *pexit_code);
        //// int gsapi_run_string (void *instance, const char *str, int user_errors, int *pexit_code);
        //// int gsapi_run_file (void *instance, const char *file_name, int user_errors, int *pexit_code);
        //// int gsapi_set_visual_tracer (gstruct vd_trace_interface_s *I);

        //// public static extern int gsapi_set_stdio(IntPtr instance, StdioCallBack stdin_fn, StdioCallBack stdout_fn, StdioCallBack stderr_fn);
    }
}
