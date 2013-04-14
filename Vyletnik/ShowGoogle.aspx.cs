using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vyletnik
{
    public partial class ShowGoogle : System.Web.UI.Page
    {
        string _initParam= "lednice.kml";
        public string InitParam 
        {
            get { return _initParam; }
            set { _initParam = value; }
        }
        string _trip = "lednice-rybniky.kml";
        public string Trip
        {
            get { return _trip; }
            set { _trip = value; }
        }

        string _vylet =  "vylet.kml";
        public string Vylet
        {
            get { return _vylet; }
            set { _vylet = value; }
        }

        string _map;
        public string Map
        {
            get { return _map; }
            set { _map = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string hostUrl = Page.Request.Url.AbsoluteUri;
            Uri ur = new Uri(hostUrl);
            InitParam = Request["vylet"];
            if (InitParam.Length > 0)
            {
                hostUrl = "http://" + ur.Host + ":" + ur.Port.ToString() + "/vyletnik/" + InitParam;
                InitParam = hostUrl;
            }
            else
            {
                InitParam = "#";
            }
            ur = new Uri(hostUrl);
            Trip = Request["trip"];
            if (Trip.Length > 0)
            {
                hostUrl = "http://" + ur.Host + ":" + ur.Port.ToString() + "/vyletnik/" + Trip;
                Trip = hostUrl;
            }
            else
            {
                Trip = "#";
            }

            ur = new Uri(hostUrl);
            if (Vylet.Length > 0)
            {
                hostUrl = "http://" + ur.Host + ":" + ur.Port.ToString() + "/vyletnik/" + Vylet;
                Vylet = hostUrl;
            }
            else
            {
                Vylet = "#";
            }

            Map = LoadTrip(Vylet, Trip);           
        }

        public string LoadTrip(string vylet, string trip)
        {
            GpsTrip trp = new GpsTrip();
            trp.downloadKML(trip);

            string map = "<p><img border=\"0\" src=\"//maps.googleapis.com/maps/api/staticmap?path=color:0x0000ff%7Cweight:5";
            int cnt = 1;
            foreach (GpsPoint pt in trp)
            {
                //path=color:0xff0000ff%7Cweight:5%7C40.737102,-73.990318%7C40.749825,-73.987963%7C40.752946,-73.987384%7C40.755823,-73.986397&size=512x512&sensor=false"
                //path=color:0x0000ff|weight:5|40.737102,-73.990318|40.749825,-73.987963|40.752946,-73.987384|40.755823,-73.986397
                map += "%7C" + pt.ToString();
                //if (cnt++ > 20)
                //    break;
            }
            //&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&sensor=false\" alt=\"Trip\" /></p>";
            map += "&size=512x512&sensor=false\" alt=\"Trip\" /></p>";



            return map;
        }

    }
}