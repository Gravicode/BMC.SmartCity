﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BMC.CoreLibrary.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BMC.CoreLibrary.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to [{&quot;Name&quot;:&quot;Rule Water Flow&quot;,&quot;DateCreated&quot;:&quot;2018-05-03T00:00:00&quot;,&quot;DateModified&quot;:&quot;2018-05-03T00:00:00&quot;,&quot;Enabled&quot;:true,&quot;Description&quot;:&quot;Check water flow threshold&quot;,&quot;GroupId&quot;:1,&quot;Severity&quot;:&quot;High&quot;,&quot;Conditions&quot;:[{&quot;Field&quot;:&quot;Water Flow Sensor&quot;,&quot;Operator&quot;:0,&quot;Value&quot;:60.0}]},{&quot;Name&quot;:&quot;Rule Water Height&quot;,&quot;DateCreated&quot;:&quot;2018-05-03T00:00:00&quot;,&quot;DateModified&quot;:&quot;2018-05-03T00:00:00&quot;,&quot;Enabled&quot;:true,&quot;Description&quot;:&quot;Check water height threshold&quot;,&quot;GroupId&quot;:2,&quot;Severity&quot;:&quot;High&quot;,&quot;Conditions&quot;:[{&quot;Field&quot;:&quot;Water Height Sensor&quot;,&quot;Operator&quot;:1,&quot;Va [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string rules {
            get {
                return ResourceManager.GetString("rules", resourceCulture);
            }
        }
    }
}