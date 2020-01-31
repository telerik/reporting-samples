using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RadHtmlChart1.DataSource = GetData();
            RadHtmlChart1.DataBind();
        }
    }

    private object GetData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("labelsField");
        dt.Columns.Add("valueField");

        dt.Rows.Add(2004, 100);
        dt.Rows.Add(2005, 150);
        dt.Rows.Add(2006, 90);
        dt.Rows.Add(2007, 120);
        dt.Rows.Add(2008, 200);
        dt.Rows.Add(2009, 135);
        dt.Rows.Add(2010, 80);

        return dt;

    }
}
