﻿#pragma checksum "Z:\Work .NET\MISAK EXPERIMENT\Vyletnik\SilverlightVylet\Markers\BeginMark.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7A3AD396A1466B1A5039039CC0204165"
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


namespace CustomMarkers {
    
    
    public partial class BeginMark : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Shapes.Polygon Start;
        
        internal System.Windows.Controls.Border borderImage;
        
        internal System.Windows.Controls.Image image;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightVylet;component/Markers/BeginMark.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Start = ((System.Windows.Shapes.Polygon)(this.FindName("Start")));
            this.borderImage = ((System.Windows.Controls.Border)(this.FindName("borderImage")));
            this.image = ((System.Windows.Controls.Image)(this.FindName("image")));
        }
    }
}

