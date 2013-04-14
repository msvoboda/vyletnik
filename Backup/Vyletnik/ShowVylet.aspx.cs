using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vyletnik
{
    public partial class ShowVylet : System.Web.UI.Page
    {
        public string InitParam = "lednice.kml";
        public string Trip = "lednice-rybniky.kml";
        public string Vylet = "vylet.kml";        


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
        }
        

    }
}