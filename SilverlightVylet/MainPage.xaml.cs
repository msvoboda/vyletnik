using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Threading;
using System.Globalization;
using Vyletnik;
using Microsoft.Maps.MapControl;
using CustomMarkers;

namespace SilverlightVylet
{
    public partial class MainPage : UserControl
    {
        private string m_vylet = "";
        private string m_vylet_trip = "";
        private string m_myvylet = "";
        private GpsDataApp vyletnik = null;
        public static ManualResetEvent allDone = new ManualResetEvent(false);


        public MainPage()
        {
            vyletnik = new GpsDataApp();
            vyletnik.OnTripLoaded += new TripEventHandler(vyletnik_OnTripLoaded);
            InitializeComponent();
            
            vyletnik.Vylet.BackColor = Colors.Red;
            vyletnik.Trip.BackColor = Colors.Blue;
            vyletnik.Trip.TrackType = trackType.trackLines;
        }

        void vyletnik_OnTripLoaded(GpsTrip trip)
        {
            refreshTrip(trip);
        }

        public void setVylet(string vylet, string trip, string myvylet)
        {
            m_vylet = vylet;
            Log(vylet);
            m_vylet_trip = trip;
            Log(trip);
            m_myvylet = myvylet;
            Log(myvylet);

            //vyletnik.Vylet.downloadKML(m_vylet);
            if (m_vylet_trip != "#")
                vyletnik.Trip.downloadKML(m_vylet_trip);             
            if (m_vylet!="#")
                vyletnik.Vylet.downloadVylet(m_vylet);
        }

        private void Log(string line)
        { 
            textLog.Text += "\n"+line;
        }


        public void refreshTrip(GpsTrip trip)
        {
            double min_lat = 512;
            double min_lon = 515;
            double max_lat = -512;
            double max_lon = -515;

            if (trip.Name != null && trip.Name.Length > 0)
                labelVylet.Content = "Výlet: "+trip.Name;            

            if (trip.TrackType == trackType.trackPoints)
            {
                for (int i = 0; i < trip.Count; i++)
                {
                    GpsPoint pt = trip[i];
                    if (pt.Latitude < min_lat)
                        min_lat = pt.Latitude;
                    if (pt.Longitude < min_lon)
                        min_lon = pt.Longitude;
                    if (pt.Latitude > max_lat)
                        max_lat = pt.Latitude;
                    if (pt.Longitude > max_lon)
                        max_lon = pt.Longitude;

                    if (i == 0)
                    {
                        BeginMark punk = new BeginMark();
                        punk.TripPoint = pt;
                        punk.OnPointClick += new MapPointEventHandler(PointClick);
                        if (pt.ThumbImage != null && pt.ThumbImage.Length > 0)
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(pt.ThumbImage, UriKind.Absolute));
                            img.Height = 48;
                            img.Margin = new Thickness(1, 0, 1, 0);
                            //img.MouseLeftButtonDown += new MouseButtonEventHandler(img_MouseLeftButtonDown);                        
                            punk.setImage(pt.ThumbImage);
                            ImageThumb img_th = new ImageThumb(pt);
                            img_th.Image = img;
                            img_th.OnPointClick += new MapPointEventHandler(PointClick);
                            stackImages.Children.Add(img_th);
                        }

                        Location loc = new Location(pt.Latitude, pt.Longitude, 0);
                        punk.BackColor = new SolidColorBrush(trip.BackColor);
                        MyLayer.AddChild(punk, loc);
                    }
                    else
                    {
                        MapPointImage punk = new MapPointImage();
                        punk.TripPoint = pt;
                        punk.OnPointClick+=new MapPointEventHandler(PointClick);
                        if (pt.ThumbImage != null && pt.ThumbImage.Length > 0)
                        {
                            Image img = new Image();
                            img.Source = new BitmapImage(new Uri(pt.ThumbImage, UriKind.Absolute));
                            img.Height = 48;
                            img.Margin = new Thickness(1, 0, 1, 0);
                            //img.MouseLeftButtonDown += new MouseButtonEventHandler(img_MouseLeftButtonDown);                        
                            punk.setImage(pt.ThumbImage);
                            ImageThumb img_th = new ImageThumb(pt);
                            img_th.Image = img;
                            img_th.OnPointClick += new MapPointEventHandler(PointClick);
                            stackImages.Children.Add(img_th);
                        }

                        Location loc = new Location(pt.Latitude, pt.Longitude, 0);
                        punk.BackColor = new SolidColorBrush(trip.BackColor);
                        MyLayer.AddChild(punk, loc);
                    }

                }
            }
            else if (trip.TrackType == trackType.trackLines)
            {
                LocationCollection points = new LocationCollection();
                for (int i = 0; i < trip.Count; i++)
                {
                    GpsPoint pt = trip[i];
                    if (pt.Latitude < min_lat)
                        min_lat = pt.Latitude;
                    if (pt.Longitude < min_lon)
                        min_lon = pt.Longitude;
                    if (pt.Latitude > max_lat)
                        max_lat = pt.Latitude;
                    if (pt.Longitude > max_lon)
                        max_lon = pt.Longitude;

                    Location loc = new Location(pt.Latitude, pt.Longitude, 0);
                    points.Add(loc);
                    //Punkt punk = new Punkt();
                    //punk.BackColor = new SolidColorBrush(trip.BackColor);
                    //MyLayer.AddChild(punk, loc);
                }
                MapPolyline shape = new MapPolyline();
                shape.Locations = points;
                shape.StrokeThickness = 1.5;
                shape.Stroke = new SolidColorBrush(trip.BackColor);                

                MyLayer.Children.Add(shape);

            }

            if (trip.Count > 0)
                map.Center = new Location((min_lat+max_lat)/2,(min_lon+max_lon)/2);
        }

        // uzivatel klikne na point
        void PointClick(GpsPoint point)
        {
            if (point != null)
            {

                pointDetail.TripPoint = point;
                map.Center = new Location(point.Latitude, point.Longitude);
                if (map.ZoomLevel < 14)
                    map.ZoomLevel = 14;
            }
        }
    }         
}
  