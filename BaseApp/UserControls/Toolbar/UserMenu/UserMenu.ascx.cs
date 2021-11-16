using System;
using Telerik.Web.UI;

public partial class UserControls_Menu_UserMenu_UserMenu : System.Web.UI.UserControl
{
    //user ID
    private const int IdUser = 1;

    public string UserName
    {
        get { return rmUser.Items[IdUser].Value; }
        set { rmUser.Items[IdUser].Value = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (RadMenuItem rmi in rmUser.Items)
            {
                rmi.Attributes.Add("onclick", "return false;");
            }
            string clientUrl = ResolveClientUrl("~/Admin_module/ResetPassword.aspx");

            FillDocsItem();

            rmUser.Items[1].Attributes.Add("onclick", "window.open('" + clientUrl + "')");
        }
    }

    public void FillDocsItem()
    {
        rmUser.Items[0].Items.AddRange(DbConnMenu.LoadDocItems());
    }
}
