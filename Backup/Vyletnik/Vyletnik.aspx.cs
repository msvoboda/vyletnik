using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoogleTools.Web;
using System.Data;

namespace Vyletnik
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            labelRok.Text = DateTime.Now.Year.ToString();

            GoogleBlog blog = new GoogleBlog();
            DataTable blog_table = blog.GetBlog("http://vyletnik.blogspot.com/feeds/posts/default", new DateTime(2010, 1, 1), "");
            ListBlog.DataSource = blog_table;
            ListBlog.DataBind();
            ListVylety.DataSource = blog_table;
            ListVylety.DataBind();
        }
    }
}