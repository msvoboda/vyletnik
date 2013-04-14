using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Vyletnik;

namespace SilverlightVylet
{
    public partial class ImageThumb : UserControl
    {
        private GpsPoint m_point = null; 
        public event MapPointEventHandler OnPointClick = null;

        public ImageThumb(GpsPoint point)
        {
            InitializeComponent();

            m_point = point;
            labelName.Content = point.Name;            
        }

        public Image Image
        {
            set
            {
                image.Source = value.Source;
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (OnPointClick != null && m_point != null)
            {
                OnPointClick(m_point);
            }
        }
    
    }
}
