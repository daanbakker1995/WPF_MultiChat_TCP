﻿#pragma checksum "..\..\..\ServerApp.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "806A8291777EA3FF2C001068F39D74BFCCE22E3F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfApp;


namespace WpfApp {
    
    
    /// <summary>
    /// ServerApp
    /// </summary>
    public partial class ServerApp : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\ServerApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ChatList;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\ServerApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox InputMessage;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\ServerApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnSendMessage;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\ServerApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox InputServerIP;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\ServerApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox InputPortNumber;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\ServerApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox InputBufferSize;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\ServerApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnStartServer;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\ServerApp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ErrorTextBlock;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfAppServer;component/serverapp.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ServerApp.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ChatList = ((System.Windows.Controls.ListBox)(target));
            return;
            case 2:
            this.InputMessage = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.BtnSendMessage = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\ServerApp.xaml"
            this.BtnSendMessage.Click += new System.Windows.RoutedEventHandler(this.BtnSendMessage_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.InputServerIP = ((System.Windows.Controls.TextBox)(target));
            
            #line 43 "..\..\..\ServerApp.xaml"
            this.InputServerIP.KeyUp += new System.Windows.Input.KeyEventHandler(this.InputServerIP_KeyUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.InputPortNumber = ((System.Windows.Controls.TextBox)(target));
            
            #line 48 "..\..\..\ServerApp.xaml"
            this.InputPortNumber.KeyUp += new System.Windows.Input.KeyEventHandler(this.InputPortNumber_KeyUp);
            
            #line default
            #line hidden
            return;
            case 6:
            this.InputBufferSize = ((System.Windows.Controls.TextBox)(target));
            
            #line 53 "..\..\..\ServerApp.xaml"
            this.InputBufferSize.KeyUp += new System.Windows.Input.KeyEventHandler(this.InputBufferSize_KeyUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BtnStartServer = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\ServerApp.xaml"
            this.BtnStartServer.Click += new System.Windows.RoutedEventHandler(this.BtnStartServer_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ErrorTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

