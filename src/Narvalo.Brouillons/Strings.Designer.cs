﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Narvalo {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Narvalo.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exception: {0} {1} {2}..
        /// </summary>
        internal static string DebuggerLogger_MessageFormat {
            get {
                return ResourceManager.GetString("DebuggerLogger_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A left Either has no right value..
        /// </summary>
        internal static string Either_LeftHasNoRightValue {
            get {
                return ResourceManager.GetString("Either_LeftHasNoRightValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A right Either has no left value..
        /// </summary>
        internal static string Either_RightHasNoLeftValue {
            get {
                return ResourceManager.GetString("Either_RightHasNoLeftValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to HTTP InputStream too large..
        /// </summary>
        internal static string HttpRequestBase_InputStreamTooLarge {
            get {
                return ResourceManager.GetString("HttpRequestBase_InputStreamTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A successful outcome has no exception..
        /// </summary>
        internal static string Output_SuccessfulHasNoException {
            get {
                return ResourceManager.GetString("Output_SuccessfulHasNoException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A bad outcome has no value..
        /// </summary>
        internal static string Output_UnsuccessfulHasNoValue {
            get {
                return ResourceManager.GetString("Output_UnsuccessfulHasNoValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; must be of enum type..
        /// </summary>
        internal static string TypeIsNotEnumFormat {
            get {
                return ResourceManager.GetString("TypeIsNotEnumFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A non fatal outcome has no exception info..
        /// </summary>
        internal static string VoidOrError_NotFatal {
            get {
                return ResourceManager.GetString("VoidOrError_NotFatal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A non warning outcome has no error message..
        /// </summary>
        internal static string VoidOrError_NotWarning {
            get {
                return ResourceManager.GetString("VoidOrError_NotWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse attribute &apos;{0}&apos;, line {1}..
        /// </summary>
        internal static string XElement_MalformedAttributeValueFormat {
            get {
                return ResourceManager.GetString("XElement_MalformedAttributeValueFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to parse element &apos;{0}&apos;, line {1}..
        /// </summary>
        internal static string XElement_MalformedElementValueFormat {
            get {
                return ResourceManager.GetString("XElement_MalformedElementValueFormat", resourceCulture);
            }
        }
    }
}