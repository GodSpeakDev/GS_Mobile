﻿// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace GodSpeak.Resources {
    using System;
    using System.Reflection;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Text {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Text() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("GodSpeak.Resources.Text", typeof(Text).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string GetStartedText {
            get {
                return ResourceManager.GetString("GetStartedText", resourceCulture);
            }
        }
        
        internal static string ClaimInviteCodeText {
            get {
                return ResourceManager.GetString("ClaimInviteCodeText", resourceCulture);
            }
        }
        
        internal static string RequestInviteCodeText {
            get {
                return ResourceManager.GetString("RequestInviteCodeText", resourceCulture);
            }
        }
        
        internal static string OkPopup {
            get {
                return ResourceManager.GetString("OkPopup", resourceCulture);
            }
        }
        
        internal static string ErrorPopupTitle {
            get {
                return ResourceManager.GetString("ErrorPopupTitle", resourceCulture);
            }
        }
        
        internal static string SuccessPopupTitle {
            get {
                return ResourceManager.GetString("SuccessPopupTitle", resourceCulture);
            }
        }
        
        internal static string ErrorPopupText {
            get {
                return ResourceManager.GetString("ErrorPopupText", resourceCulture);
            }
        }
        
        internal static string GetACode {
            get {
                return ResourceManager.GetString("GetACode", resourceCulture);
            }
        }
        
        internal static string TryAgain {
            get {
                return ResourceManager.GetString("TryAgain", resourceCulture);
            }
        }
        
        internal static string InviteCodeRequiredMessage {
            get {
                return ResourceManager.GetString("InviteCodeRequiredMessage", resourceCulture);
            }
        }
        
        internal static string InviteCodePlaceholder {
            get {
                return ResourceManager.GetString("InviteCodePlaceholder", resourceCulture);
            }
        }
        
        internal static string EmailPlaceHolder {
            get {
                return ResourceManager.GetString("EmailPlaceHolder", resourceCulture);
            }
        }
        
        internal static string PasswordPlaceHolder {
            get {
                return ResourceManager.GetString("PasswordPlaceHolder", resourceCulture);
            }
        }
        
        internal static string EmailRequiredMessage {
            get {
                return ResourceManager.GetString("EmailRequiredMessage", resourceCulture);
            }
        }
        
        internal static string PasswordRequiredMessage {
            get {
                return ResourceManager.GetString("PasswordRequiredMessage", resourceCulture);
            }
        }
        
        internal static string RequestInviteSuccessfully {
            get {
                return ResourceManager.GetString("RequestInviteSuccessfully", resourceCulture);
            }
        }
        
        internal static string ForgotPasswordSuccessfully {
            get {
                return ResourceManager.GetString("ForgotPasswordSuccessfully", resourceCulture);
            }
        }
    }
}
