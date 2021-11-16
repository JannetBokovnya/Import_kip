using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code.Version_API;

public partial class Modules_About_system_index : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string url = "";
        try { url = Request.UrlReferrer.ToString(); }
        catch { }

        btnOk.Attributes.Add("onclick", "LogIn(); return false;");
    }
}