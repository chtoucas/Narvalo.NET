﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.34209
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Narvalo {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
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
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
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
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
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
        ///   Recherche une chaîne localisée semblable à The supplied value {0} is not a well-formed absolute URI..
        /// </summary>
        internal static string AbsoluteUriValidator_UriIsNotAbsoluteFormat {
            get {
                return ResourceManager.GetString("AbsoluteUriValidator_UriIsNotAbsoluteFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The &lt;{0}&gt; section is not defined in your config file!.
        /// </summary>
        internal static string ConfigurationManager_MissingSectionFormat {
            get {
                return ResourceManager.GetString("ConfigurationManager_MissingSectionFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The value is empty..
        /// </summary>
        internal static string DebugCheck_IsEmpty {
            get {
                return ResourceManager.GetString("DebugCheck_IsEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The value is null..
        /// </summary>
        internal static string DebugCheck_IsNull {
            get {
                return ResourceManager.GetString("DebugCheck_IsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The type must be of enum type..
        /// </summary>
        internal static string DebugCheck_TypeIsNotEnum {
            get {
                return ResourceManager.GetString("DebugCheck_TypeIsNotEnum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The type must be a value type..
        /// </summary>
        internal static string DebugCheck_TypeIsNotValueType {
            get {
                return ResourceManager.GetString("DebugCheck_TypeIsNotValueType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The parameter {0} is null..
        /// </summary>
        internal static string ExceptionFactory_ArgumentNullFormat {
            get {
                return ResourceManager.GetString("ExceptionFactory_ArgumentNullFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Illegal character {0} found at position {1}..
        /// </summary>
        internal static string Int64Encoder_IllegalCharacterFormat {
            get {
                return ResourceManager.GetString("Int64Encoder_IllegalCharacterFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Can not cast a none maybe to its underlying value type..
        /// </summary>
        internal static string Maybe_CannotCastNoneToValue {
            get {
                return ResourceManager.GetString("Maybe_CannotCastNoneToValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à You can not get the underlying value of an empty maybe..
        /// </summary>
        internal static string Maybe_NoneHasNoValue {
            get {
                return ResourceManager.GetString("Maybe_NoneHasNoValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à A successful outcome has no exception..
        /// </summary>
        internal static string Output_SuccessfulHasNoException {
            get {
                return ResourceManager.GetString("Output_SuccessfulHasNoException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à A bad outcome has no value..
        /// </summary>
        internal static string Output_UnsuccessfulHasNoValue {
            get {
                return ResourceManager.GetString("Output_UnsuccessfulHasNoValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The lower end must be lesser than upper end..
        /// </summary>
        internal static string Range_LowerEndNotLesserThanUpperEnd {
            get {
                return ResourceManager.GetString("Range_LowerEndNotLesserThanUpperEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The parameter {0} is empty..
        /// </summary>
        internal static string Require_ArgumentEmptyFormat {
            get {
                return ResourceManager.GetString("Require_ArgumentEmptyFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The value is not greater than or equal to {0}..
        /// </summary>
        internal static string Require_NotGreaterThanOrEqualToFormat {
            get {
                return ResourceManager.GetString("Require_NotGreaterThanOrEqualToFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The value is not in range {0} / {1}..
        /// </summary>
        internal static string Require_NotInRangeFormat {
            get {
                return ResourceManager.GetString("Require_NotInRangeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The value is not less than or equal to {0}..
        /// </summary>
        internal static string Require_NotLessThanOrEqualToFormat {
            get {
                return ResourceManager.GetString("Require_NotLessThanOrEqualToFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The object &apos;this&apos; is null..
        /// </summary>
        internal static string Require_ObjectNull {
            get {
                return ResourceManager.GetString("Require_ObjectNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The property value is empty..
        /// </summary>
        internal static string Require_PropertyEmpty {
            get {
                return ResourceManager.GetString("Require_PropertyEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The property value is null..
        /// </summary>
        internal static string Require_PropertyNull {
            get {
                return ResourceManager.GetString("Require_PropertyNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à The scheme {0} does not support protocol less uri..
        /// </summary>
        internal static string Uri_ProtocolLessUnsupportedSchemeFormat {
            get {
                return ResourceManager.GetString("Uri_ProtocolLessUnsupportedSchemeFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à A non fatal outcome has no exception info..
        /// </summary>
        internal static string VoidOrError_NotFatal {
            get {
                return ResourceManager.GetString("VoidOrError_NotFatal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à A non warning outcome has no error message..
        /// </summary>
        internal static string VoidOrError_NotWarning {
            get {
                return ResourceManager.GetString("VoidOrError_NotWarning", resourceCulture);
            }
        }
    }
}
