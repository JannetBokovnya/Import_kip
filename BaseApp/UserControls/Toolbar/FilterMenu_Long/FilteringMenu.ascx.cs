using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data;
using Telerik.Web.UI;

public partial class UserControls_Menu_FilterMenu_FilteringMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmMainMenu.Items.AddRange(DbConnMenu.LoadMainMenuItems());
        }
    }
}