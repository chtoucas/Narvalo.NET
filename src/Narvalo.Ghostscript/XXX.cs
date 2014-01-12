namespace Narvalo.GhostScript {
	//using System;
	//using System.Collections.Generic;
	//using System.ComponentModel;
	//using System.Runtime.ConstrainedExecution;
	//using System.Runtime.InteropServices;
	//using System.Security;
	//using System.Security.Permissions;
	//using System.Text;
	//using Microsoft.Win32.SafeHandles;

	// See: http://code.google.com/p/nohros-onix/@this/browse/trunk/src/base/streamer/third_party/7zip/?r=13
	//http://msdn.microsoft.com/en-us/library/system.runtime.interopservices.safehandle.aspx

	// See:
	// -
	//		http://www.codeproject.com/Articles/29534/IDisposable-What-Your-Mother-Never-Told-You-About
	// - SafeHandles: the best V2.0 feature of the .NET Framework 
	//		http://blogs.msdn.com/b/bclteam/archive/2005/03/15/396335.aspx
	// - Keep Your Code Running with the Reliability Features of the .NET Framework (Stephen Toub)
	//		http://msdn.microsoft.com/en-us/magazine/cc163716.aspx
	// - Constrained Execution Regions and other errata
	//		http://blogs.msdn.com/b/bclteam/archive/2005/06/14/429181.aspx
	// - SafeHandle and Constrained Execution Regions
	//		http://www.codeproject.com/Articles/16157/SafeHandle-and-Constrained-Execution-Regions
	// - Sam Saffron
	//		http://stackoverflow.com/questions/1101147/code-demonstrating-the-importance-of-a-constrained-execution-region?rq=1
	/// <summary>
	/// Utility class to wrap an unmanaged DLL and be responsible for freeing it.
	/// </summary>
	/// <remarks>
	/// This is a managed wrapper over the native LoadLibrary, GetProcAddress, 
	/// and FreeLibrary calls.
	/// </example>
	/// <see cref=
	/// "http://blogs.msdn.com/jmstall/archive/2007/01/06/Typesafe-GetProcAddress.aspx"
	/// />
	//public sealed class UnmanagedLibrary : IDisposable {
	//    // Unmanaged resource. CLR will ensure SafeHandles get freed, without 
	//    // requiring a finalizer on this class.
	//    private SafeLibraryHandle _libHandle;

	//    #region Safe handles and Native imports

	//    /// <summary>
	//    /// See http://msdn.microsoft.com/msdnmag/issues/05/10/Reliability/ 
	//    /// for more about safe handles.
	//    /// </summary>
	//    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	//    private sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid {
	//        /// <summary>
	//        /// Create safe library handle
	//        /// </summary>
	//        private SafeLibraryHandle() : base(true) { }

	//        /// <summary>
	//        /// Release handle
	//        /// </summary>
	//        protected override bool ReleaseHandle() {
	//            return NativeMethods.FreeLibrary(handle);
	//        }
	//    }

	//    /// <summary>
	//    /// Native methods
	//    /// </summary>
	//    private static class NativeMethods {
	//        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
	//        public static extern SafeLibraryHandle LoadLibrary(string fileName);

	//        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	//        [DllImport("kernel32", SetLastError = true)]
	//        [return: MarshalAs(UnmanagedType.Bool)]
	//        public static extern bool FreeLibrary(IntPtr hModule);

	//        [DllImport("kernel32", EntryPoint = "GetProcAddress")]
	//        public static extern IntPtr GetProcAddress(SafeLibraryHandle hModule,
	//            String procname);
	//    }

	//    #endregion

	//    /// <summary>
	//    /// Constructor to load a dll and be responible for freeing it.
	//    /// </summary>
	//    /// <param name="fileName">full path name of dll to load</param>
	//    /// <exception cref="System.IO.FileNotFoundException">
	//    /// If fileName can't be found
	//    /// </exception>
	//    /// <remarks>
	//    /// Throws exceptions on failure. Most common failure would be 
	//    /// file-not-found, or that the file is not a loadable image.
	//    /// </remarks>
	//    public UnmanagedLibrary(string fileName) {
	//        _libHandle = NativeMethods.LoadLibrary(fileName);

	//        if (_libHandle.IsInvalid) {
	//            int hr = Marshal.GetHRForLastWin32Error();
	//            Marshal.ThrowExceptionForHR(hr);
	//        }
	//    }

	//    /// <summary>
	//    /// Dynamically lookup a function in the dll via kernel32!GetProcAddress.
	//    /// </summary>
	//    /// <param name="functionName">
	//    /// raw name of the function in the export table.
	//    /// </param>
	//    /// <returns>
	//    /// null if function is not found. Else a delegate to the unmanaged 
	//    /// function.
	//    /// </returns>
	//    /// <remarks>
	//    /// GetProcAddress results are valid as long as the dll is not yet 
	//    /// unloaded. This is very very dangerous to use since you need to 
	//    /// ensure that the dll is not unloaded until after you're done with any 
	//    /// objects implemented by the dll. For example, if you get a delegate 
	//    /// that then gets an IUnknown implemented by this dll, you can not 
	//    /// dispose this library until that IUnknown is collected. Else, you may 
	//    /// free the library and then the CLR may call release on that IUnknown 
	//    /// and it will crash.
	//    /// </remarks>
	//    public TDelegate GetUnmanagedFunction<TDelegate>(string functionName)
	//        where TDelegate : class {
	//        IntPtr p = NativeMethods.GetProcAddress(_libHandle, functionName);

	//        // Failure is a common case, especially for adaptive code.
	//        if (p == IntPtr.Zero) {
	//            return null;
	//        }

	//        Delegate function = Marshal.GetDelegateForFunctionPointer(
	//            p, typeof(TDelegate));

	//        // Ideally, we'd just make the constraint on TDelegate be
	//        // System.Delegate, but compiler error CS0702 
	//        // (constrained can't be System.Delegate)
	//        // prevents that. So we make the constraint system.object and do the
	//        // cast from object-->TDelegate.
	//        object o = function;

	//        return (TDelegate)o;
	//    }


	//    #region IDisposable

	//    /// <summary>
	//    /// Call FreeLibrary on the unmanaged dll. All function pointers handed 
	//    /// out from this class become invalid after this.
	//    /// </summary>
	//    /// <remarks>
	//    /// This is very dangerous because it suddenly invalidate everything
	//    /// retrieved from this dll. This includes any functions handed out via 
	//    /// GetProcAddress, and potentially any objects returned from those 
	//    /// functions (which may have an implemention in the dll).
	//    /// </remarks>
	//    public void Dispose() {
	//        if (!_libHandle.IsClosed) {
	//            _libHandle.Close();
	//        }
	//    }

	//    #endregion
	//}

	//public class GhostScriptApiV3 : IDisposable {
	//    private const string Kernel32Dll = "kernel32.dll";
	//    private const string GhostScript32Dll = "gsdll32.dll";

	//    private SafeLibraryHandle _libHandle;

	//    #region Win32 API

	//    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	//    private sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid {
	//        public SafeLibraryHandle() : base(true) { }

	//        /// <summary>Release library handle</summary>
	//        /// <returns>true if the handle was released</returns>
	//        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	//        protected override bool ReleaseHandle() {
	//            return Win32NativeMethods.FreeLibrary(handle);
	//        }
	//    }

	//    private static class Win32NativeMethods {
	//        [DllImport(Kernel32Dll, CharSet = CharSet.Auto, SetLastError = true)]
	//        public static extern SafeLibraryHandle LoadLibrary(
	//          [MarshalAs(UnmanagedType.LPTStr)] string lpFileName);

	//        [SuppressUnmanagedCodeSecurity]
	//        [DllImport(Kernel32Dll, SetLastError = true)]
	//        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	//        [return: MarshalAs(UnmanagedType.Bool)]
	//        public static extern bool FreeLibrary(IntPtr hModule);

	//        [DllImport(Kernel32Dll, EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, SetLastError = true)]
	//        public static extern IntPtr GetProcAddress(
	//          SafeLibraryHandle hModule,
	//          [MarshalAs(UnmanagedType.LPStr)] string procName);
	//    }

	//    #endregion

	//    #region GhostScript API

	//    private static class GSNativeMethods {
	//        // int gsapi_init_with_args (void *instance, int argc, char **argv);
	//        //[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	//        [DllImport("gsdll32.dll", EntryPoint = "gsapi_init_with_args", SetLastError = true)]
	//        public static extern int gsapi_init_with_args(GhostScriptSafeHandle instance, int argc, IntPtr argv);

	//        // int gsapi_new_instance (void **pinstance, void *caller_handle);
	//        //[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	//        [DllImport("gsdll32.dll", EntryPoint = "gsapi_new_instance", SetLastError = true)]
	//        public static extern int gsapi_new_instance(out GhostScriptSafeHandle pinstance, IntPtr caller_handle);

	//        // int gsapi_set_poll (void *instance, int(*poll_fn)(void *caller_handle));
	//        // int gsapi_set_display_callback (void *instance, display_callback *callback);
	//        // int gsapi_run_string_begin (void *instance, int user_errors, int *pexit_code);
	//        // int gsapi_run_string_continue (void *instance, const char *str, unsigned int length, int user_errors, int *pexit_code);
	//        // int gsapi_run_string_end (void *instance, int user_errors, int *pexit_code);
	//        // int gsapi_run_string_with_length (void *instance, const char *str, unsigned int length, int user_errors, int *pexit_code);
	//        // int gsapi_run_string (void *instance, const char *str, int user_errors, int *pexit_code);
	//        // int gsapi_set_visual_tracer (gstruct vd_trace_interface_s *I);

	//        //// int gsapi_revision (gsapi_revision_t *pr, int len);
	//        //[DllImport("gsdll32.dll", EntryPoint = "gsapi_revision")]
	//        //public static extern int gsapi_revision(out NativeStructs.GS_Revision pr, int len);
	//        //// int gsapi_run_file (void *instance, const char *file_name, int user_errors, int *pexit_code);
	//        //[DllImport("gsdll32.dll", EntryPoint = "gsapi_run_file")]
	//        //public static extern int gsapi_run_file(IntPtr instance, string file_name, int user_errors, int pexit_code);
	//        ////int gsapi_set_stdio (void *instance, int(*stdin_fn)(void *caller_handle, char *buf, int len), int(*stdout_fn)(void *caller_handle, const char *str, int len), int(*stderr_fn)(void *caller_handle, const char *str, int len));
	//        //[DllImport("gsdll32.dll", EntryPoint = "gsapi_set_stdio")]
	//        //public static extern int gsapi_set_stdio(IntPtr instance, StdioCallBack stdin_fn, StdioCallBack stdout_fn, StdioCallBack stderr_fn);
	//    }

	//    #endregion

	//    public GhostScriptApiV3() {
	//        _libHandle = Win32NativeMethods.LoadLibrary(GhostScript32Dll);

	//        if (_libHandle.IsInvalid) {
	//            int hr = Marshal.GetHRForLastWin32Error();
	//            Marshal.ThrowExceptionForHR(hr);
	//            //throw new Win32Exception();
	//        }

	//        IntPtr functionPtr = Win32NativeMethods.GetProcAddress(_libHandle, "GetHandlerProperty");

	//        // Not a valid dll
	//        if (functionPtr == IntPtr.Zero) {
	//            _libHandle.Close();

	//            throw new ArgumentException();
	//        }
	//    }

	//    ~GhostScriptApiV3() {
	//        Dispose(false);
	//    }

	//    protected void Dispose(bool disposing) {
	//        if (_libHandle != null && !_libHandle.IsClosed) {
	//            _libHandle.Close();
	//        }
	//        _libHandle = null;
	//    }

	//}
}
