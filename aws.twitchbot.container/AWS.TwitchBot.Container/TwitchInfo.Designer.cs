﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AWS.TwitchBot.Container {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TwitchInfo {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TwitchInfo() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AWS.TwitchBot.Container.TwitchInfo", typeof(TwitchInfo).Assembly);
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
        ///   Looks up a localized string similar to Nerdtastic.
        /// </summary>
        internal static string ChannelName {
            get {
                return ResourceManager.GetString("ChannelName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 2qjbg57jpjfia3gs0fzg26wtgl3uoi.
        /// </summary>
        internal static string ClientId {
            get {
                return ResourceManager.GetString("ClientId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 7qo31570i45vkpy0tpvweynn9f6owu484k1i9sn8zgup94byyy.
        /// </summary>
        internal static string RefreshToken {
            get {
                return ResourceManager.GetString("RefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to b33ee0sdkgzt0j8hflbzxi9go32zbg.
        /// </summary>
        internal static string Token {
            get {
                return ResourceManager.GetString("Token", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the_ghost_of_alfred.
        /// </summary>
        internal static string UserName {
            get {
                return ResourceManager.GetString("UserName", resourceCulture);
            }
        }
    }
}
