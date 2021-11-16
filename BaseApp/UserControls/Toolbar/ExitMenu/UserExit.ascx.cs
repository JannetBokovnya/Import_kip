using System;

public partial class UserControls_Menu_UserMenu_UserMenu : System.Web.UI.UserControl
{

    private const int idClose = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rmExit.Items[idClose].Attributes.Add("onclick", "logOff(); return false;");
        }
    }
}
