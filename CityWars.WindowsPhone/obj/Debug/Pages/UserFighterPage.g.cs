﻿

#pragma checksum "C:\Users\gesh\Desktop\CityWars\CityWars\CityWars.Shared\Pages\UserFighterPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8C007804AECA1141C8ED37356B5F4815"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CityWars.Pages
{
    partial class UserFighterPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 54 "..\..\Pages\UserFighterPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.onFightButtonClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 55 "..\..\Pages\UserFighterPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.onTopCitiesButtonClick;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 39 "..\..\Pages\UserFighterPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Holding += this.onImageHolding;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 50 "..\..\Pages\UserFighterPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.onRegenerateButtonClick;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 29 "..\..\Pages\UserFighterPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.onSignoutButtonClick;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


