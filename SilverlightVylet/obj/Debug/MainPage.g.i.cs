﻿#pragma checksum "C:\Dokumenty\Visual Studio 2010\Vyletnik\SilverlightVylet\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2A92903BF9DA9B2EE3F555BA6244BA5E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Maps.MapControl;
using SilverlightVylet;
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
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Border borderTop;
        
        internal System.Windows.Controls.Label labelVylet;
        
        internal Microsoft.Maps.MapControl.Map map;
        
        internal Microsoft.Maps.MapControl.MapLayer MyLayer;
        
        internal System.Windows.Controls.Border border1;
        
        internal System.Windows.Controls.ScrollViewer scroll;
        
        internal System.Windows.Controls.StackPanel stackImages;
        
        internal SilverlightVylet.PointDetail pointDetail;
        
        internal System.Windows.Controls.Border borderLog;
        
        internal System.Windows.Controls.TextBox textLog;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightVylet;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.borderTop = ((System.Windows.Controls.Border)(this.FindName("borderTop")));
            this.labelVylet = ((System.Windows.Controls.Label)(this.FindName("labelVylet")));
            this.map = ((Microsoft.Maps.MapControl.Map)(this.FindName("map")));
            this.MyLayer = ((Microsoft.Maps.MapControl.MapLayer)(this.FindName("MyLayer")));
            this.border1 = ((System.Windows.Controls.Border)(this.FindName("border1")));
            this.scroll = ((System.Windows.Controls.ScrollViewer)(this.FindName("scroll")));
            this.stackImages = ((System.Windows.Controls.StackPanel)(this.FindName("stackImages")));
            this.pointDetail = ((SilverlightVylet.PointDetail)(this.FindName("pointDetail")));
            this.borderLog = ((System.Windows.Controls.Border)(this.FindName("borderLog")));
            this.textLog = ((System.Windows.Controls.TextBox)(this.FindName("textLog")));
        }
    }
}

