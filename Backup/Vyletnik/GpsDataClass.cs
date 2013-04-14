using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Globalization;
using System.Threading;

//using Log;

namespace Vyletnik
{
    public delegate void TripEventHandler(GpsTrip trip);
    public delegate void MapPointEventHandler(GpsPoint point);

    public class GpsDataClass
    {
    }

    public class GpsDataApp : GpsDataClass
    {

        private GpsTrip m_vylet = null;
        private GpsTrip m_track = null;

        public event TripEventHandler OnTripLoaded = null;

        public GpsDataApp()
        {
            m_vylet = new GpsTrip(this);
            m_vylet.OnTripLoaded += new TripEventHandler(m_vylet_OnTripLoaded);
            m_track = new GpsTrip(this);
            m_track.OnTripLoaded +=new TripEventHandler(m_vylet_OnTripLoaded);
        }

        void m_vylet_OnTripLoaded(GpsTrip trip)
        {
            if (OnTripLoaded != null)
            {
                OnTripLoaded(trip);
            }
        }

        public GpsTrip Vylet
        {
            get { return m_vylet; }
        }

        public GpsTrip Trip
        {
            get { return m_track; }          
        }

        


    }

    public enum trackType { trackPoints, trackLines, trackPointAndLines }

    // trip ... sklada se z bodu
    public class GpsTrip : List<GpsPoint>
    {
        private GpsDataClass m_data = null;        

        public string Name { get; set; }
       
        public trackType TrackType { get; set; }
        // events
        public event TripEventHandler OnTripLoaded = null;

        public GpsTrip(GpsDataClass data)
        {
            m_data = data;
            TrackType = trackType.trackPoints;
        }

        public GpsTrip() {   }


        #region Google KML              

        public void downloadKML(string kml)
        {
            Uri uri = new Uri(kml);

            WebClient client = new WebClient();
            byte[] res = client.DownloadData(uri);
            string data = Encoding.UTF8.GetString(res);
            loadKML(data);
            //client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            //client.DownloadStringAsync(uri);

        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled == false && e.Error == null)
            {
                loadKML(e.Result);
                //allDone.Set();
            }
        }

        public void loadKML(string data)
        {
            /*
            XDocument doc = XDocument.Parse(data);
            XElement root = doc.Element(XName.Get("kml"));
            int a = 0;*/
            using (XmlReader reader = XmlReader.Create(new StringReader(data)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Placemark")
                    {
                        GpsPoint pt = parsePointKML(reader);
                        Add(pt);
                    }
                }
            }

            if (OnTripLoaded != null)
            {
                OnTripLoaded(this);
            }
            //refreshTrip(trip);
        }

        private GpsPoint parsePointKML(XmlReader reader)
        {
            GpsPoint new_pt = new GpsPoint();

            CultureInfo ci = new CultureInfo("en-US");
            CultureInfo old_ci = System.Globalization.CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = ci;
            // Display the culture name and currency symbol.
            NumberFormatInfo nfi = ci.NumberFormat;

            while (reader.Read())
            {
                XmlNodeType node_type = reader.NodeType;
                if (node_type == XmlNodeType.Element && reader.Name == "name")
                {
                    new_pt.Name = reader.ReadInnerXml();
                }
                else if (node_type == XmlNodeType.Element && reader.Name == "description")
                {
                    string desc = reader.ReadInnerXml();
                    string uri = ParseDescription(desc);
                    new_pt.ThumbImage = uri;
                    GpsPointImage pt_img = new GpsPointImage();
                    pt_img.ThumbImage = uri;
                    pt_img.FullImage = uri;
                    new_pt.Images.Add(pt_img);
                }
                else if (node_type == XmlNodeType.Element && reader.Name == "coordinates")
                {
                    string latlon = reader.ReadInnerXml();
                    string[] coord = latlon.Split(new Char[] { ',' });
                    new_pt.Longitude = Convert.ToDouble(coord[0]);
                    new_pt.Latitude = Convert.ToDouble(coord[1]);
                    break;
                }
            }

            Thread.CurrentThread.CurrentCulture = old_ci;

            return new_pt;
        }

        private string ParseDescription(string data)
        {
            int img_zac = data.IndexOf("<img");
            int img_end = data.IndexOf("/>", img_zac + 1);

            if (img_zac != -1 && img_end != -1)
            {
                string img = data.Substring(img_zac, img_end - img_zac);
                int src = img.IndexOf("src=\"");
                int src_zac = img.IndexOf("\"", src + 1);
                int src_end = img.IndexOf("\"", src_zac + 6);
                if (src != -1 && src_zac != -1 && src_end != -1)
                {
                    string url = img.Substring(src_zac + 1, src_end - src_zac - 1);
                    return url;
                }

            }

            return "";
        }
        #endregion

        #region Misak Vylet
        public void downloadVylet(string vylet)
        {
            Uri uri = new Uri(vylet);
            //http://localhost:49958/vylet_lednice.vylet
            Name = vylet;

            WebClient vclient = new WebClient();
            vclient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadVyletCompleted);
            vclient.DownloadStringAsync(uri);
            //allDone.WaitOne();
            // XmlDocument doc = new XmlDocument();

        }

        void DownloadVyletCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Cancelled == false && e.Error == null)
            {
                loadVylet(e.Result);
                //allDone.Set();
            }
        }

        public void loadVylet(string data)
        {
            CultureInfo ci = new CultureInfo("en-US");
            CultureInfo old_ci = System.Globalization.CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = ci;
            // Display the culture name and currency symbol.
            NumberFormatInfo nfi = ci.NumberFormat;              
            /*
            XDocument doc = XDocument.Parse(data);
            XElement root = doc.Element(XName.Get("kml"));
            int a = 0;*/
            using (XmlReader reader = XmlReader.Create(new StringReader(data)))
            {
                GpsPoint new_pt = null;

                while (reader.Read())
                {
                    try
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "vylet")
                        {
                            string name = reader.GetAttribute("Title");
                            if (name != null && name.Length > 0)
                                Name = name;
                            //GpsPoint pt = parsePointVylet(reader);
                            //Add(pt);
                        }
                        else if (reader.NodeType == XmlNodeType.Element && reader.Name == "TripPoint")
                        {
                            GpsPoint pt = new GpsPoint();
                            new_pt = pt;
                            string lat = reader.GetAttribute("Latitude");
                            string lon = reader.GetAttribute("Longitude");
                            string name = reader.GetAttribute("Name");
                            string th = reader.GetAttribute("ThumbImage");
                            pt.Name = name;
                            pt.Longitude = Convert.ToDouble(lon);
                            pt.Latitude = Convert.ToDouble(lat);
                            pt.ThumbImage = th;
                            //GpsPoint pt = parsePointVylet(reader);

                            Add(pt);
                        }
                        else if (reader.NodeType == XmlNodeType.Element && new_pt != null && reader.Name == "Content")
                        {
                            string content = reader.ReadInnerXml();
                            new_pt.Text = content;
                        }
                        else if (reader.NodeType == XmlNodeType.Element && new_pt != null && reader.Name == "Image")
                        {
                            string thumb = reader.GetAttribute("Thumbnail");
                            string src = reader.GetAttribute("Src");

                            GpsPointImage pt_img = new GpsPointImage();
                            pt_img.ThumbImage = thumb;
                            pt_img.FullImage = src;
                            new_pt.Images.Add(pt_img);
                        }
                    }
                    catch(Exception e)
                    {
                        int a = 0;
                    }
                }
            }

            Thread.CurrentThread.CurrentCulture = old_ci;

            if (OnTripLoaded != null)
            {
                OnTripLoaded(this);
            }
            //refreshTrip(trip);
        
        }

        private GpsPoint parsePointVylet(XmlReader reader)
        {
            GpsPoint pt = new GpsPoint();
            return pt;
        }

        #endregion

    }

    public class GpsPoint
    {
        public List<GpsPointImage> m_images = new List<GpsPointImage>();

        public int pointID { get; set; }
        // position
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        //
        public double Range { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string ThumbImage { get; set; }
        public List<GpsPointImage> Images
        {
            get
            {
                return m_images;
            }
        }

        public override string ToString()
        {
            CultureInfo ci = new CultureInfo("en-US");
            CultureInfo old_ci = System.Globalization.CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = ci;
            // Display the culture name and currency symbol.
            NumberFormatInfo nfi = ci.NumberFormat;
            string num = Latitude.ToString() + "," + Longitude.ToString();
            Thread.CurrentThread.CurrentCulture = old_ci;


            return num;
        }
    }

    public class GpsPointImage
    {
        public string FullImage { get; set; }
        public string ThumbImage { get; set; }

        public string ToString()
        {
            return FullImage;
        }
    }



    
}
