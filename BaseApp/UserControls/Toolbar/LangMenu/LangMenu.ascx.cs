using System;
using System.Configuration;
using System.Threading;
using Telerik.Web.UI;

public partial class UserControls_Menu_LangMenu_LangMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string lang = (Session["lang"] != null) ? Session["lang"].ToString() : Thread.CurrentThread.CurrentCulture.Name;

            if (!String.IsNullOrEmpty(lang))
            {
                RadMenuItem itemByValue = rmLang.FindItemByValue(lang);
                // установка картинки контролу, которая потом меняется css
                if (itemByValue != null)
                    itemByValue.ImageUrl = "~/Images/flag-16.png";
            }
        }
    }

    protected void rmLang_OnItemClick(object sender, RadMenuEventArgs e)
    {
        RadMenuItem selectedItem = rmLang.SelectedItem;
        if (selectedItem.Items.Count == 0)
        {
            if (selectedItem.ImageUrl == "")
            {
                Session["lang"] = selectedItem.Value;
                Response.Redirect("~/" + ConfigurationManager.AppSettings["startPage"]);
            }
            rmLang.ClearSelectedItem();
        }
        
    }
}