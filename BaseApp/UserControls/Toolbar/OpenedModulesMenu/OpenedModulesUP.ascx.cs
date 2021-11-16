using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OpenedModulesUP : System.Web.UI.UserControl
{
    public void Page_Load(object sender, EventArgs e)
    {
    }

    protected void upnlOpenModules_Load(object sender, EventArgs e)
    {
        List<string> lstWindows = new List<string>();
        List<string> lstTitles = new List<string>();


        HiddenField hdnWindowOpen = (HiddenField)Page.Master.FindControl("hdnWindowOpen");
        List<InfoOpenModule> lstOpenModules = JSONParser.ParseOpenWindows(hdnWindowOpen.Value);

        openedModulesMenu.CheckActiveWindows(lstOpenModules);

    }
}
