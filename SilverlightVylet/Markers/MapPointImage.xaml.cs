using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Vyletnik;

namespace CustomMarkers
{
    public partial class MapPointImage : UserControl
    {
        GpsPoint m_point = null;
        // event
        public event MapPointEventHandler OnPointClick = null;

        public MapPointImage()
        {
            InitializeComponent();
            image.SizeChanged += new SizeChangedEventHandler(image_SizeChanged);
        }

        void image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //borderImage.Width = e.NewSize.Width + 4;
            //borderImage.Height = e.NewSize.Height + 4;            
        }

        public Brush BackColor
        {
            set
            {
                Elipse.Fill = value;
            }
        }

        public void setImage(string img_str)
        {            
            image.Source = new BitmapImage(new Uri(img_str, UriKind.Absolute));
            image.Height = 48;
            image.Visibility = System.Windows.Visibility.Visible;
            //img.MouseLeftButtonDown += new MouseButtonEventHandler(img_MouseLeftButtonDown);                        
        }

        public Image Image
        {
            set
            {
                if (value != null)
                {
                    image = value;
                    image.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        public GpsPoint TripPoint
        {
            set
            {
                m_point = value;
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
