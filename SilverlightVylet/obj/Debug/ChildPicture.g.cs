﻿#pragma checksum "Z:\Work .NET\MISAK EXPERIMENT\Vyletnik\SilverlightVylet\ChildPicture.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "805421A8262406A6703FB036D7DF5295"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SilverlightVylet {
    
    
    public partial class ChildPicture : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Border border;
        
        internal System.Windows.Controls.Image image;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.Image imageNext;
        
        internal System.Windows.Controls.Image imagePrev;
        
        internal System.Windows.Controls.Label labelCount;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightVylet;component/ChildPicture.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.border = ((System.Windows.Controls.Border)(this.FindName("border")));
            this.image = ((System.Windows.Controls.Image)(this.FindName("image")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.imageNext = ((System.Windows.Controls.Image)(this.FindName("imageNext")));
            this.imagePrev = ((System.Windows.Controls.Image)(this.FindName("imagePrev")));
            this.labelCount = ((System.Windows.Controls.Label)(this.FindName("labelCount")));
        }
    }
}

