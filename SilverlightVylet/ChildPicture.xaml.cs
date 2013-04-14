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

namespace SilverlightVylet
{
    public partial class ChildPicture : ChildWindow
    {
        private GpsPoint m_point = null;
        private int m_img_pos = 0;

        public ChildPicture()
        {
            InitializeComponent();
        }

        public void setImage(string picture, GpsPoint point)
        {
            m_point = point;

            if (m_point.Images.Count <= 1)
            {
               labelCount.Visibility = System.Windows.Visibility.Collapsed;
               imagePrev.Visibility = System.Windows.Visibility.Collapsed;
               imageNext.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                labelCount.Visibility = System.Windows.Visibility.Visible;
                imagePrev.Visibility = System.Windows.Visibility.Visible;
                imageNext.Visibility = System.Windows.Visibility.Visible;

                m_img_pos = 0;
                labelCount.Content = "(" + (m_img_pos+1).ToString()+" / " + point.Images.Count.ToString() + ")";
            }

            Visibility = System.Windows.Visibility.Visible;                    
            image.Source = new BitmapImage(new Uri(picture, UriKind.Absolute));
            Visibility = System.Windows.Visibility.Visible;
        }

        public void ImagePrev()
        {
            if (m_img_pos > 0)
                m_img_pos--;

            image.Source = new BitmapImage(new Uri(m_point.Images[m_img_pos].FullImage, UriKind.Absolute));
            labelCount.Content = "(" + (m_img_pos + 1).ToString() + " / " + m_point.Images.Count.ToString() + ")";
        }

        public void ImageNext()
        {
            if (m_img_pos < m_point.Images.Count - 1)
                m_img_pos++;

            image.Source = new BitmapImage(new Uri(m_point.Images[m_img_pos].FullImage, UriKind.Absolute));
            labelCount.Content = "(" + (m_img_pos + 1).ToString() + " / " + m_point.Images.Count.ToString() + ")";
        }

        private void imageExit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void imagePrev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImagePrev();
        }

        private void imageNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageNext();
        }
    }
}

