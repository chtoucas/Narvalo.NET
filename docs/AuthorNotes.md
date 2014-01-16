
// http://social.msdn.microsoft.com/Forums/en-US/b26ffe68-1350-4d05-95ca-48c442a833f7/a-better-way-to-resolve-ca1006?forum=vstscode
// http://reader.feedshow.com/show_items-feed=bf3134fdd71dd85e8c2972a011d216e6


Narvalo.Build
=============

http://codepaste.net/cnmgnd
http://codingcockerel.codeplex.com/releases/view/12739

Use OutputFiles
http://codepaste.net/cnmgnd
http://codingcockerel.co.uk/2008/05/11/a-custom-msbuild-task-for-merging-config-files/
http://codingcockerel.codeplex.com/releases/view/12739
http://stackoverflow.com/questions/2426494/run-and-kill-a-selenium-server-singleton-process-for-duration-of-c-dll-lifetime
http://stackoverflow.com/questions/1085584/how-do-i-programatically-retrieve-the-actual-path-to-program-files-folder

Narvalo.Ghostscript
===================

Other GhostScript implementations:
- http://cyotek.com
- http://www.codeproject.com/Articles/32274/How-To-Convert-PDF-to-Image-Using-Ghostscript-API


GhostScript
- http://www.ghostscript.com/doc/current/Use.htm#Parameter_switches
- http://www.ghostscript.com/doc/9.05/API.htm#return_codes


P/Invoke
- http://msdn.microsoft.com/en-us/magazine/cc164193.aspx
- http://clrinterop.codeplex.com/releases/view/14120

CSE:
- http://msdn.microsoft.com/en-us/magazine/dd419661.aspx (Handling Corrupted State Exceptions)
- http://stackoverflow.com/questions/2950130/how-to-simulate-a-corrupt-state-exception-in-net-4/2950307#2950307
- http://stackoverflow.com/questions/3561545/how-to-terminate-a-program-when-it-crashes-which-should-just-fail-a-unit-test


Narvalo.Presentation.Mvp
========================

http://aspiringcraftsman.com/tag/model-view-presenter/
http://aspiringcraftsman.com/2007/08/25/interactive-application-architecture/


Narvalo.Runtime.Reliability
===========================

Sources d'inspiration

- kite (java)
	https://github.com/williewheeler/kite
- jrugged (java)
- Hystrix (java)


TODO

- availability & performance

- thread safe
- événements
- annotations
- async

Narvalo.StyleCop.CSharp
=======================

http://scottwhite.blogspot.com/2008/11/creating-custom-stylecop-rules-in-c.html
http://devjitsu.com/blog/2010/06/17/running-stylecop-in-vs-2005-or-vs-express/
http://www.lovethedot.net/2008/05/creating-custom-rules-for-microsoft_6976.html
http://blogs.msdn.com/b/sourceanalysis/
http://www.planetgeek.ch/2009/07/19/custom-stylecop-rules/

						 
http://stackoverflow.com/questions/2759228/difference-between-net-4-client-profile-and-full-framework-download


Conventions de nommage
----------------------

- static readonly + public accessor ?

- convention de nommages

- .Extensions

- Utility

- Helper

- Helpers

- Codec
	  Model Binding
    http://callbackdelegate.blogspot.com/2009/07/modelbinders-and-factory-methods.html
    http://www.hanselman.com/blog/IPrincipalUserModelBinderInASPNETMVCForEasierTesting.aspx
    http://stackoverflow.com/questions/2030059/dataannotation-validations-and-custom-modelbinder

Unicode
    http://www.alanwood.net/unicode/miscellaneous_technical.html
    http://www.fileformat.info/info/unicode/char/2638/index.htm





/*
 * Web.Config
 * ==========
 * 
 * remove Web.config in Views
 * disable ViewState
 * register controls
 * See: http://www.asp.net/learn/whitepapers/aspnet4
 * http://blog.wekeroad.com/blog/asp-net-mvc-avoiding-tag-soup/
 * http://msdn.microsoft.com/en-us/library/y6wb1a0e.aspx
 * T4MVC
 * 
 * http://msdn.microsoft.com/en-us/library/bb126445.aspx
 * http://www.esenciadev.com/2010/04/mvc2-areas-t4-and-sharing-actions/
 * http://mvccontrib.codeplex.com/wikipage?title=T4MVC_doc&referringTitle=T4MVC
*/

/*
 * ASP.NET 2.0 MVP Hacks 
 */
 http://mojoportal.codeplex.com
 http://www.hanselman.com/blog/ABackToBasicsCaseStudyImplementingHTTPFileUploadWithASPNETMVCIncludingTestsAndMocks.aspx
 http://www.ademiller.com/blogs/tech/2009/06/making-sure-fxcop-warnings-and-errors-break-the-build/
 http://bradwilson.typepad.com/blog/2010/04/writing-an-fxcop-task-for-msbuild.html
 http://csharp-source.net/
http://tutorialzine.com/category/tutorials/css-tutorials/
http://zoonek.free.fr/blosxom/
http://blog.stevensanderson.com/2010/03/03/behavior-driven-development-bdd-with-specflow-and-aspnet-mvc/
http://google-gruyere.appspot.com/
http://www.cantrip.org/traits.html
http://jarlsberg.appspot.com/
http://www.codeproject.com/KB/architecture/BddWithSpecFlow.aspx



http://visualstudiogallery.msdn.microsoft.com/en-us/


TODO
----

- CA : Requires.NotNull -> paramètres
- [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]

To debug log4net internals:

<system.diagnostics>
<trace autoflush="true">
    <listeners>
    <add name="textWriterTraceListener"
            type="System.Diagnostics.TextWriterTraceListener"
            initializeData="D:\Tmp\log4net_internal.log"/>
    </listeners>
</trace>
</system.diagnostics>

Base58
     http://code.google.com/p/bitcoinj/source/browse/trunk/src/com/google/bitcoin/core/Base58.java?spec=svn85&r=85
     http://code.google.com/p/bitcoinsharp/source/browse/src/Core/Base58.cs

Security

    http://www.bouncycastle.org/csharp/

Code Contracts
    Interface...
http://social.msdn.microsoft.com/Forums/en-US/codecontracts/thread/5ae40843-a32c-4056-8016-f1127048f5ff
    Code Analysis
http://geekswithblogs.net/terje/archive/2010/10/14/making-static-code-analysis-and-code-contracts-work-together-or.aspx

http://csharpindepth.com/Articles.aspx

Monads
http://channel9.msdn.com/Series/C9-Lectures-Erik-Meijer-Functional-Programming-Fundamentals/Lecture-Series-Erik-Meijer-Functional-Programming-Fundamentals-Chapter-1
        // http://www.haskell.org/haskellwiki/Typeclassopedia

// http://en.wikipedia.org/wiki/Monad_(category_theory)
// http://en.wikipedia.org/wiki/Option_type
// http://stackoverflow.com/questions/7828528/monads-in-c-sharp-why-bind-implementations-require-passed-function-to-return
// http://seanfoy.blogspot.fr/2009/06/andandnet-via-monads.html

http://abdullin.com/journal/2009/10/6/zen-development-practices-c-maybe-monad.html
http://blog.jordanterrell.com/post/Maybe-The-Uh-Stuff-That-Dreams-Are-Made-Of.aspx

        // http://msdn.microsoft.com/en-us/library/dd233182.aspx
        // http://codebetter.com/matthewpodwysocki/2010/01/18/much-ado-about-monads-creating-extended-builders/
        // http://www.introtorx.com/content/v1.0.10621.0/10_LeavingTheMonad.html
        // http://www.hanselman.com/blog/ReactiveExtensionsRxIsNowOpenSource.aspx
        // http://twistedoakstudios.com/blog/Post867_how-would-i-even-use-a-monad-in-c

Parsers

https://github.com/sprache/Sprache
http://code.google.com/p/sprache/


Narvalo.GhostScript
===================

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
