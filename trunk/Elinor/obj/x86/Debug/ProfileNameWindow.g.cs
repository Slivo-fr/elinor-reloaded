﻿#pragma checksum "..\..\..\ProfileNameWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EDB93275746AA57BA2F5B46678E32DA1"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.1008
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace Elinor {
    
    
    /// <summary>
    /// ProfileNameWindow
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class ProfileNameWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\..\ProfileNameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\..\ProfileNameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbName;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\..\ProfileNameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOk;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\ProfileNameWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Elinor;component/profilenamewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ProfileNameWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 3 "..\..\..\ProfileNameWindow.xaml"
            ((Elinor.ProfileNameWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.WindowLoaded);
            
            #line default
            #line hidden
            
            #line 3 "..\..\..\ProfileNameWindow.xaml"
            ((Elinor.ProfileNameWindow)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.WindowKeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.tbName = ((System.Windows.Controls.TextBox)(target));
            
            #line 7 "..\..\..\ProfileNameWindow.xaml"
            this.tbName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TbNameTextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnOk = ((System.Windows.Controls.Button)(target));
            
            #line 9 "..\..\..\ProfileNameWindow.xaml"
            this.btnOk.Click += new System.Windows.RoutedEventHandler(this.BtnOkClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\ProfileNameWindow.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.BtnCancelClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

