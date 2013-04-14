using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using CollectionTools;
using System.Web;

namespace GoogleTools.Web
{
    public class GoogleBase
    {
        protected string m_auth = "";
        protected string strAccount = "misak.svobo";
        protected string pageUrl = "";
        private string m_gservice = "lh2";
        protected bool onlyPublic = true;

        public GoogleBase()
        {
        }

        public GoogleBase(string user, string url)
        {
            strAccount = user;
            pageUrl = url;
        }

        public GoogleBase(string user, string pass, string url)            
        {
            m_auth = GetAuth(user, pass, "google_app", m_gservice);
            strAccount = user;
            pageUrl = url;
            onlyPublic = false;
        }

        public void setGService(string gservice)
        {
            m_gservice = gservice;
        }

        public void setLogin(string user, string pass)
        {
            m_auth = GetAuth(user, pass, "google_app", m_gservice);
            strAccount = user;            
            onlyPublic = false;
        }


        protected string GetAuth(string email, string password, string appId, string service)
        {
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create("https://www.google.com/accounts/ClientLogin");
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.Method = "POST";
            StringBuilder myPost = new StringBuilder();
            myPost.AppendFormat("Email={0}", email);
            myPost.AppendFormat("&Passwd={0}", password);
            myPost.AppendFormat("&service={0}", service);
            myPost.AppendFormat("&source={0}", appId);

            byte[] myData = ASCIIEncoding.ASCII.GetBytes(myPost.ToString());
            myRequest.ContentLength = myData.Length;

            Stream myStream = myRequest.GetRequestStream();
            myStream.Write(myData, 0, myData.Length);
            myStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            Stream myResponseStream = myResponse.GetResponseStream();
            StreamReader myResponseReader = new StreamReader(myResponseStream);
            string strResponse = myResponseReader.ReadToEnd();
            myResponseReader.Close();
            myResponseStream.Close();

            string strAuth = "";
            if (strResponse.LastIndexOf("Auth=") > 0)
            {
                int i = strResponse.LastIndexOf("=") + 1;
                strAuth = strResponse.Substring(i, strResponse.Length - i);
            }
            else
            {
                strAuth = "n/a";
            }

            return strAuth;
        }

        public string GetRequest(string adress)
        {
            string result = "";

            HttpWebResponse objResponse = null;
            HttpWebRequest objRequest = (HttpWebRequest)HttpWebRequest.Create(adress);
            if (onlyPublic == false)
            {
                objRequest.Headers.Add("Authorization: GoogleLogin auth=" + m_auth);
            }
            objRequest.Method = "GET";

            try
            {
                objResponse = (HttpWebResponse)objRequest.GetResponse();
            }
            catch (Exception e)
            {
                //Response.Write(e.Message);
                //Response.End();
            }

            if (objResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader sr = new StreamReader(objResponse.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }

            return result;
        }
    }


    public class PicasaImage
    {
        public string Name { get; set; }
        public string FullImage { get; set; }
        public string MediumImage { get; set; }
    }

    public class PicasaAlbums : GoogleBase
    {
        //public string User { get; set; }
        //public string Name { get; set; }

        public PicasaAlbums(string user, string url)
        : base(user, url) {}

        public PicasaAlbums(string user, string pass, string url)            
        :base(user,pass, url) {}

        public DataTable GetAlbums()
        {
            string result = "";

            HttpWebResponse objResponse = null;
            HttpWebRequest objRequest = (HttpWebRequest)HttpWebRequest.Create("http://picasaweb.google.com/data/feed/api/user/" + strAccount + "?kind=album");
            objRequest.Headers.Add("Authorization: GoogleLogin auth=" + m_auth);
            objRequest.Method = "GET";

            try
            {
                objResponse = (HttpWebResponse)objRequest.GetResponse();
            }
            catch (Exception e)
            {
                //Response.Write(e.Message);
                //Response.End();
            }

            if (objResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader sr = new StreamReader(objResponse.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("gphoto", "http://schemas.google.com/photos/2007");
            nsMgr.AddNamespace("batch", "http://schemas.google.com/gdata/batch");
            nsMgr.AddNamespace("photo", "http://www.pheed.com/pheed/");
            nsMgr.AddNamespace("media", "http://search.yahoo.com/mrss/");
            nsMgr.AddNamespace("georss", "http://www.georss.org/georss");
            nsMgr.AddNamespace("photo", "http://www.pheed.com/pheed/");

            XmlNodeList nl = xml.GetElementsByTagName("entry");

            DataTable table = new DataTable("albums");
            DataColumn colid = new DataColumn("id", Type.GetType("System.String"));
            DataColumn coltitle = new DataColumn("title", Type.GetType("System.String"));
            DataColumn colsum = new DataColumn("summary", Type.GetType("System.String"));
            DataColumn colaccess = new DataColumn("access", Type.GetType("System.String"));
            DataColumn colthumb = new DataColumn("thumb", Type.GetType("System.String"));
            DataColumn colcord = new DataColumn("Coords", Type.GetType("System.String"));
            DataColumn collink = new DataColumn("link", Type.GetType("System.String"));
            DataColumn colphotoid = new DataColumn("photoid", Type.GetType("System.String"));

            table.Columns.Add(colid);
            table.Columns.Add(coltitle);
            table.Columns.Add(colsum);
            table.Columns.Add(colaccess);
            table.Columns.Add(colthumb);
            table.Columns.Add(colcord);
            table.Columns.Add(collink);
            table.Columns.Add(colphotoid);

            for (int i = 0; i < nl.Count; i++)
            {
                XmlNode xentry = nl[i];
                XmlNodeList xalbum = xentry.ChildNodes;
                DataRow row = table.NewRow();
                for (int alb = 0; alb < xalbum.Count; alb++)
                {
                    string tag = xalbum[alb].Name;
                    switch (tag)
                    {
                        case "id":
                            row["id"] = xalbum[alb].InnerText;
                            break;
                        case "title":
                            row["title"] = xalbum[alb].InnerText;
                            break;
                        case "summary":
                            row["summary"] = xalbum[alb].InnerText;
                            break;
                        case "gphoto:access":
                            row["access"] = xalbum[alb].InnerText;
                            break;
                        case "gphoto:name":
                            row["link"] =  pageUrl+"?album="+xalbum[alb].InnerText;
                            break;
                        case "gphoto:id":
                            row["photoid"] = pageUrl + "?album=" + xalbum[alb].InnerText;
                            break;
                        case "media:group":
                            XmlNodeList xmedia = xalbum[alb].ChildNodes;
                            for (int c = 0; c < xmedia.Count; c++)
                            {
                                if (xmedia[c].Name == "media:thumbnail")
                                {
                                    XmlAttribute url = xmedia[c].Attributes["url"];
                                    if (url != null)
                                    {
                                        row["thumb"] = url.InnerText;
                                    }
                                }
                            }
                            break;
                    }
                }
                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable GetPublicAlbums(string filter)
        {
            return this.GetPublicAlbums(filter, pageUrl);
        }

        public DataTable GetPublicAlbums(string filter, string alb_url)
        {
            string result = "";

            bool use_filter = (filter.Length > 0);
            string pole = "";
            string[] arg = filter.Split(new Char []{'='});
            if (use_filter == true && arg.Length == 2)
            {
                pole = arg[0];
                filter = arg[1];
            }
            else
            {
                use_filter = false;
            }

            HttpWebResponse objResponse = null;
            HttpWebRequest objRequest = (HttpWebRequest)HttpWebRequest.Create("http://picasaweb.google.cz/data/feed/base/user/" + strAccount + "?kind=album");
            //objRequest.Headers.Add("Authorization: GoogleLogin auth=" + m_auth);
            objRequest.Method = "GET";

            try
            {
                objResponse = (HttpWebResponse)objRequest.GetResponse();
            }
            catch (Exception e)
            {
                //Response.Write(e.Message);
                //Response.End();
            }

            if (objResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader sr = new StreamReader(objResponse.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("gphoto", "http://schemas.google.com/photos/2007");
            nsMgr.AddNamespace("batch", "http://schemas.google.com/gdata/batch");
            nsMgr.AddNamespace("photo", "http://www.pheed.com/pheed/");
            nsMgr.AddNamespace("media", "http://search.yahoo.com/mrss/");
            nsMgr.AddNamespace("georss", "http://www.georss.org/georss");
            nsMgr.AddNamespace("photo", "http://www.pheed.com/pheed/");

            XmlNodeList nl = xml.GetElementsByTagName("entry");

            DataTable table = new DataTable("albums");
            DataColumn colid = new DataColumn("id", Type.GetType("System.String"));
            DataColumn coltitle = new DataColumn("title", Type.GetType("System.String"));
            DataColumn colsum = new DataColumn("summary", Type.GetType("System.String"));
            DataColumn colaccess = new DataColumn("access", Type.GetType("System.String"));
            DataColumn colthumb = new DataColumn("thumb", Type.GetType("System.String"));
            DataColumn colcord = new DataColumn("Coords", Type.GetType("System.String"));
            DataColumn collink = new DataColumn("link", Type.GetType("System.String"));
            DataColumn colphotoid = new DataColumn("photoid", Type.GetType("System.String"));

            table.Columns.Add(colid);
            table.Columns.Add(coltitle);
            table.Columns.Add(colsum);
            table.Columns.Add(colaccess);
            table.Columns.Add(colthumb);
            table.Columns.Add(colcord);
            table.Columns.Add(collink);
            table.Columns.Add(colphotoid);            

            for (int i = 0; i < nl.Count; i++)
            {
                XmlNode xentry = nl[i];
                XmlNodeList xalbum = xentry.ChildNodes;
                DataRow row = table.NewRow();

                bool add = true;

                for (int alb = 0; alb < xalbum.Count; alb++)
                {
                    string tag = xalbum[alb].Name;

                    switch (tag)
                    {
                        case "id":
                            row["id"] = xalbum[alb].InnerText;
                            break;
                        case "title":
                            if (use_filter == false)
                            {
                                row["title"] = xalbum[alb].InnerText.Trim();
                            }
                            else if (use_filter == true && pole == tag)
                            {
                                string val = xalbum[alb].InnerText.Trim();
                                if (val.Contains(filter) == true)
                                {
                                    int st = val.IndexOf(filter);
                                    val = (val.Remove(st,filter.Length)).Trim();
                                    row["title"] = val;
                                }
                                else
                                {
                                    add = false;
                                }
                            }
                            break;
                        case "summary":
                            row["summary"] = xalbum[alb].InnerText;
                            break;
                        case "gphoto:access":
                            row["access"] = xalbum[alb].InnerText;
                            break;
                        case "gphoto:name":
                            row["link"] = pageUrl + "?album=" + xalbum[alb].InnerText;
                            break;
                        case "link":
                            XmlAttribute href = xalbum[alb].Attributes["href"];
                            if (href != null)
                            {
                                string idurl = href.Value;
                                int id1 = idurl.IndexOf("/albumid/")+9;
                                int id2 = idurl.IndexOf("?");
                                if (id1 > 0 && id2 > 0)
                                {
                                    idurl = idurl.Substring(id1, id2 - id1);
                                    row["photoid"] = alb_url + "?album=" + idurl;
                                }
                            }
                            break;
                        case "media:group":
                            XmlNodeList xmedia = xalbum[alb].ChildNodes;
                            for (int c = 0; c < xmedia.Count; c++)
                            {
                                if (xmedia[c].Name == "media:thumbnail")
                                {
                                    XmlAttribute url = xmedia[c].Attributes["url"];
                                    if (url != null)
                                    {
                                        row["thumb"] = url.InnerText;
                                    }
                                }
                            }
                            break;
                    }
                }

                if (add == true)
                {
                    table.Rows.Add(row);
                }
            }

            return table;
        }


        public DataTable GetFotoAlbum(string albumID)
        {
            string result = "";

            HttpWebResponse objResponse = null;            
            HttpWebRequest objRequest = (HttpWebRequest)HttpWebRequest.Create("http://picasaweb.google.cz/data/feed/base/user/"+this.strAccount+"/albumid/" + albumID + "?hl=cs");
            if (onlyPublic == false)
            {
                objRequest.Headers.Add("Authorization: GoogleLogin auth=" + m_auth);
            }
            objRequest.Method = "GET";

            try
            {
                objResponse = (HttpWebResponse)objRequest.GetResponse();
            }
            catch (Exception e)
            {
                //Response.Write(e.Message);
                //Response.End();
            }

            if (objResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader sr = new StreamReader(objResponse.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("gphoto", "http://schemas.google.com/photos/2007");
            nsMgr.AddNamespace("batch", "http://schemas.google.com/gdata/batch");
            nsMgr.AddNamespace("photo", "http://www.pheed.com/pheed/");
            nsMgr.AddNamespace("media", "http://search.yahoo.com/mrss/");
            nsMgr.AddNamespace("georss", "http://www.georss.org/georss");
            nsMgr.AddNamespace("photo", "http://www.pheed.com/pheed/");

            string table_name = "album";
            XmlNodeList xtitles = xml.GetElementsByTagName("title");
            if (xtitles != null && xtitles.Count > 0)
            {
                table_name = xtitles[0].InnerText;
            }

            XmlNodeList nl = xml.GetElementsByTagName("entry");

            DataTable table = new DataTable(table_name);
            DataColumn colid = new DataColumn("id", Type.GetType("System.String"));
            DataColumn coltitle = new DataColumn("title", Type.GetType("System.String"));
            DataColumn colsum = new DataColumn("summary", Type.GetType("System.String"));            
            DataColumn colthumb = new DataColumn("thumb", Type.GetType("System.String"));
            DataColumn colfullsize = new DataColumn("fullsize", Type.GetType("System.String"));

            table.Columns.Add(colid);
            table.Columns.Add(coltitle);
            table.Columns.Add(colsum);            
            table.Columns.Add(colthumb);
            table.Columns.Add(colfullsize);   

            for (int i = 0; i < nl.Count; i++)
            {
                XmlNode xentry = nl[i];
                XmlNodeList xalbum = xentry.ChildNodes;
                DataRow row = table.NewRow();
                for (int alb = 0; alb < xalbum.Count; alb++)
                {
                    string tag = xalbum[alb].Name;
                    switch (tag)
                    {
                        case "id":
                            row["id"] = xalbum[alb].InnerText;
                            break;
                        case "title":
                            row["title"] = xalbum[alb].InnerText;
                            break;
                        case "summary":
                            row["summary"] = xalbum[alb].InnerText;
                            break;
                        case "media:group":
                            XmlNodeList xmedia = xalbum[alb].ChildNodes;
                            int width = 0;
                            for (int c = 0; c < xmedia.Count; c++)
                            {
                                if (xmedia[c].Name == "media:content")
                                {
                                    XmlAttribute url = xmedia[c].Attributes["url"];
                                    if (url != null)
                                    {
                                        row["fullsize"] = url.Value;
                                    }
                                }
                                else if (xmedia[c].Name == "media:thumbnail")
                                {
                                    XmlAttribute url = xmedia[c].Attributes["url"];
                                    XmlAttribute xwidth = xmedia[c].Attributes["width"];
                                    if (url != null && xwidth != null)
                                    {
                                        int w = Convert.ToInt16(xwidth.Value);
                                        if (w > width)
                                        {
                                            row["thumb"] = url.InnerText;
                                            width = w;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }

    public class GoogleBlog : GoogleBase
    {
        public GoogleBlog()
        {
        }

        private GoogleBlog(string user, string url)
        : base(user, url) {}

        private GoogleBlog(string user, string pass, string url)            
        :base(user,pass, url) {}

        public DataTable GetBlog(string address, DateTime blogfrom, string filter)
        {
            string result = base.GetRequest(address);
            DataTable table = new DataTable("blog");

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            bool use_filter = false;
            if (filter.Length > 0)
            {
                use_filter = true;
            }       

            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xml.NameTable);
            nsMgr.AddNamespace("gphoto", "http://schemas.google.com/photos/2007");
            nsMgr.AddNamespace("batch", "http://schemas.google.com/gdata/batch");
            nsMgr.AddNamespace("photo", "http://www.pheed.com/pheed/");
            nsMgr.AddNamespace("media", "http://search.yahoo.com/mrss/");
            nsMgr.AddNamespace("georss", "http://www.georss.org/georss");
            nsMgr.AddNamespace("photo", "http://www.pheed.com/pheed/");

            XmlNodeList nl = xml.GetElementsByTagName("entry");

            DataColumn colid = new DataColumn("id", Type.GetType("System.String"));
            DataColumn colpub = new DataColumn("published", Type.GetType("System.DateTime"));
            DataColumn coltitle = new DataColumn("title", Type.GetType("System.String"));
            DataColumn colcon = new DataColumn("content", Type.GetType("System.String"));
            DataColumn colrepli = new DataColumn("replies", Type.GetType("System.String"));
            DataColumn colcat = new DataColumn("category", Type.GetType("System.String"));
            DataColumn coldet = new DataColumn("detail", Type.GetType("System.String"));
            DataColumn colvylet = new DataColumn("vylet", Type.GetType("System.String"));
            DataColumn coltrip = new DataColumn("trip", Type.GetType("System.String"));
            DataColumn collink = new DataColumn("vyletlink", Type.GetType("System.String"));
            DataColumn colsilver = new DataColumn("vyletsilver", Type.GetType("System.String"));
            DataColumn colmap = new DataColumn("map", Type.GetType("System.String"));
            

            table.Columns.Add(colid);
            table.Columns.Add(colpub);
            table.Columns.Add(coltitle);
            table.Columns.Add(colcon);
            table.Columns.Add(colrepli);
            table.Columns.Add(colcat);
            table.Columns.Add(coldet);
            table.Columns.Add(colvylet);
            table.Columns.Add(coltrip);
            table.Columns.Add(collink);
            table.Columns.Add(colsilver);
            table.Columns.Add(colmap);

            for (int i = 0; i < nl.Count; i++)
            {
                XmlNode prispevek = nl[i];
                XmlNodeList pri_data = prispevek.ChildNodes;
                DataRow row = table.NewRow();
                
                bool too_old = false;
                bool add = true;
                if (use_filter == true)
                    add = false;

                for (int x = 0; x < pri_data.Count; x++)
                {
                    XmlNode xml_node = pri_data[x];
                    string tag = xml_node.Name;
                    row["category"] = "";

                    switch (tag)
                    {
                        case "id":
                            string id = xml_node.InnerText;
                            int pos = id.LastIndexOf("-");
                            if (pos != -1)
                            {
                                id = id.Substring(pos+1);
                            }
                            row["id"] = id;
                            break;
                        case "title":
                            string title=xml_node.InnerText;
                            row["title"] = title;
                            break;
                        case "content":
                            string content = xml_node.InnerText;
                            ParamList seznam_par = ProcessContent(content);
                            if (seznam_par == null)
                            {
                                row["content"] = content;
                                row["trip"] = "";                                
                                row["vylet"] = "";
                                row["vyletlink"] = "";
                                row["vyletsilver"] = "";
                                row["map"] = "";
                            }
                            else
                            {
                                row["content"] = seznam_par.Content;

                                Param par_trip = null;
                                Param par_vylet = null;
                                Param par_map = null;
                                Param par_zoom = null;
                                if (seznam_par.ContainsKey("trip") == true)
                                    par_trip = seznam_par["trip"];
                                if (seznam_par.ContainsKey("vylet") == true)
                                    par_vylet = seznam_par["vylet"];
                                if (seznam_par.ContainsKey("map") == true)
                                {
                                    par_map = seznam_par["map"];
                                }
                                if (seznam_par.ContainsKey("zoom") == true)
                                    par_zoom = seznam_par["zoom"];

                                if (par_trip != null)
                                    row["trip"] = par_trip.Value;
                                if (par_vylet != null)
                                {
                                    row["vylet"] = par_vylet.Value;
                                    string trip = "";
                                    if (par_trip != null)
                                        trip = par_trip.Value;
                                    row["vyletlink"] = "<a href=\"ShowGoogle.aspx?vylet=" + par_vylet.Value + "&trip="+trip+"\"><img src=\"images/globe2.png\" align=\"middle\" /></a>";
                                    row["vyletsilver"] = "<a href=\"ShowVylet.aspx?vylet=" + par_vylet.Value + "&trip=" + trip + "\"><img src=\"images/globe1.png\" align=\"middle\" /></a>";
                                }
                                if(par_map != null)
                                {
                                    string zoom = "13";
                                    if (par_zoom != null)
                                        zoom = par_zoom.Value;
                                    row["map"] = "<p><img border=\"0\" src=\"//maps.googleapis.com/maps/api/staticmap?center="+par_map.Value+"&zoom="+zoom+"&size=400x400&sensor=false\" /></p>";
                                }

                            }
                            break;
                        case "published":
                            string date = xml_node.InnerText;
                            DateTime dt;
                            bool zkus= DateTime.TryParse(date, out dt);
                            row["published"] = dt;
                            if (dt <  blogfrom)
                            {
                                too_old = true;
                            }
                            break;
                        case "link":
                            string typ = xml_node.Attributes["rel"].Value;
                            string typ_dat = xml_node.Attributes["type"].Value;
                            if (typ == "replies" && typ_dat == "text/html")
                            {
                                string text = xml_node.Attributes["title"].Value;
                                row["replies"] = "<a href=\""+xml_node.Attributes["href"].Value+"\">"+text+"</a>";
                            }
                            break;
                        case "category":
                            string cat = "";
                            XmlAttribute xatt = xml_node.Attributes["term"];
                            if (xatt != null)
                            {
                                cat = xatt.Value;

                            }
                            else
                            {
                                cat = "all"; 
                            }

                            if (use_filter == true)                                       
                            {
                                if (cat.Equals(filter, StringComparison.CurrentCultureIgnoreCase) == true)
                                {
                                    add = true;
                                    break;
                                }
                            }

                            row["category"] = cat;
                            break;
                    }
                }

                if (too_old == false && add == true)
                {
                    table.Rows.Add(row);
                }
            }               

            return table;
        }

        public ParamList ProcessContent(string content)
        {
            int start = content.IndexOf('[');
            int end = content.IndexOf(']',start+1);

            if (start == -1 || end == -1)
            {
                return null;
            }
            

            ParamList list = new ParamList();            

            if (start != -1 && end != -1)
            {
                list.Content = content.Remove(start, end - start+1);

                string param_str = content.Substring(start+2, end - start-2);
                string[] pars = param_str.Split(new Char[] { ';' });

                for (int i = 0; i < pars.Length; i++)
                {
                    string par = pars[i];
                    string[] name_val = par.Split(new Char[] {'='});
                    if (name_val.Length == 2)
                    {
                        string name = name_val[0];
                        string val = name_val[1];
                        Param new_p = new Param();
                        new_p.Name = name;
                        new_p.Value = val;
                        list.Add(name,new_p);
                    }
                }
                
            }

            return list;
        }

        public string LoadTrip(string vylet, string trip)
        {
            
            return "";
        }

    }

    public class ParamList : GSDictionary<string,Param>
    {
        public string Content { get; set; }

    }

    public class Param
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class GoogleFinance : GoogleBase
    {
        public GoogleFinance()
        {
        }

        private GoogleFinance(string user, string url)
        : base(user, url) {}

        private GoogleFinance(string user, string pass, string url)            
        :base(user,pass, url) {}

        public DataTable GetPortfolios()
        {
            //string address = "http://finance.google.com/finance/feeds/default/portfolios";
            string address = "http://finance.google.com/finance/feeds/misak.svobo@gmail.com/portfolios/1/positions";
            string result = base.GetRequest(address);

            DataTable table = new DataTable("Portfolio");

            return table;
        }
    }
        
}
