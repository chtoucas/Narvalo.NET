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
    internal class SR {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SR() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Narvalo.SR", typeof(SR).Assembly);
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
        ///   Looks up a localized string similar to The supplied value &apos;{0}&apos; is not a well-formed absolute URI..
        /// </summary>
        internal static string AbsoluteUriValidator_UriIsNotAbsoluteFormat {
            get {
                return ResourceManager.GetString("AbsoluteUriValidator_UriIsNotAbsoluteFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &lt;{0}&gt; section is not defined in your config file!.
        /// </summary>
        internal static string ConfigurationManager_MissingSectionFormat {
            get {
                return ResourceManager.GetString("ConfigurationManager_MissingSectionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parameter &apos;values&apos; is null..
        /// </summary>
        internal static string NonQueryStoredProcedure_ValuesIsNull {
            get {
                return ResourceManager.GetString("NonQueryStoredProcedure_ValuesIsNull", resourceCulture);
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
        ///   Looks up a localized string similar to The scheme &apos;{0}&apos; does not support protocol less uri..
        /// </summary>
        internal static string Uri_ProtocolLessUnsupportedSchemeFormat {
            get {
                return ResourceManager.GetString("Uri_ProtocolLessUnsupportedSchemeFormat", resourceCulture);
            }
        }
    }
}
