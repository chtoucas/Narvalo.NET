﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Narvalo.Web.Properties {
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
    internal class Strings_Web {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings_Web() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Narvalo.Web.Properties.Strings_Web", typeof(Strings_Web).Assembly);
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
        ///   Looks up a localized string similar to A default AssetProvider was not specified in your web.config. See Narvalo.Web.Configuration for more information on how to configure properly your application..
        /// </summary>
        internal static string AssetManager_DefaultProviderNotConfigured {
            get {
                return ResourceManager.GetString("AssetManager_DefaultProviderNotConfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified default AssetProvider was not found. Most certainly you misconfigured your application. See Narvalo.Web.Configuration for more information on how to configure properly your application..
        /// </summary>
        internal static string AssetManager_DefaultProviderNotFound {
            get {
                return ResourceManager.GetString("AssetManager_DefaultProviderNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provider for static Web assets..
        /// </summary>
        internal static string AssetProvider_Description {
            get {
                return ResourceManager.GetString("AssetProvider_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Found an unknown configuration key: {0}..
        /// </summary>
        internal static string AssetProvider_UnknownConfigurationKey_Format {
            get {
                return ResourceManager.GetString("AssetProvider_UnknownConfigurationKey_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempt to add a provider of invalid type. Most certainly you specified a provider which does not extend AssetProviderBase. See Narvalo.Web.Configuration for more information on how to configure properly your application..
        /// </summary>
        internal static string AssetProviderCollection_InvalidProvider {
            get {
                return ResourceManager.GetString("AssetProviderCollection_InvalidProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default asset provider for ASP.NET MVC..
        /// </summary>
        internal static string DefaultAssetProvider_Description {
            get {
                return ResourceManager.GetString("DefaultAssetProvider_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while trying to bind the HTTP request to a query object. Please see the enclosed exceptions for more details on the error..
        /// </summary>
        internal static string HttpHandlerBase_BindingFailure {
            get {
                return ResourceManager.GetString("HttpHandlerBase_BindingFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This HTTP handler does not support this HTTP method: {0}. Only the following methods are accepted: {1}..
        /// </summary>
        internal static string HttpHandlerBase_InvalidHttpMethod_Format {
            get {
                return ResourceManager.GetString("HttpHandlerBase_InvalidHttpMethod_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unknown error while trying to bind the HTTP request to a query object..
        /// </summary>
        internal static string HttpHandlerBase_UnknownBindingFailure {
            get {
                return ResourceManager.GetString("HttpHandlerBase_UnknownBindingFailure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provider for local static Web assets..
        /// </summary>
        internal static string LocalAssetProvider_Description {
            get {
                return ResourceManager.GetString("LocalAssetProvider_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provider for remote static Web assets..
        /// </summary>
        internal static string RemoteAssetProvider_Description {
            get {
                return ResourceManager.GetString("RemoteAssetProvider_Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing or invalid configuration setting for &apos;baseUri&apos;. Its value must be a valid absolute URL string..
        /// </summary>
        internal static string RemoteAssetProvider_MissingOrInvalidBaseUri {
            get {
                return ResourceManager.GetString("RemoteAssetProvider_MissingOrInvalidBaseUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The URI scheme &apos;{0}&apos; does not support protocol-relative URLs..
        /// </summary>
        internal static string UriExtensions_ProtocolRelativeUnsupportedScheme_Format {
            get {
                return ResourceManager.GetString("UriExtensions_ProtocolRelativeUnsupportedScheme_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &lt;{0}&gt; section is not defined in your web.config!.
        /// </summary>
        internal static string WebConfigurationManager_SectionNotFound_Format {
            get {
                return ResourceManager.GetString("WebConfigurationManager_SectionNotFound_Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &lt;{0}&gt; section is not defined in your web.config for the virtual path: {1}!.
        /// </summary>
        internal static string WebConfigurationManager_SectionNotFoundInPath_Format {
            get {
                return ResourceManager.GetString("WebConfigurationManager_SectionNotFoundInPath_Format", resourceCulture);
            }
        }
    }
}
