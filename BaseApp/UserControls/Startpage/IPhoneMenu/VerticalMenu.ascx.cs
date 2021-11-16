using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data;
using Telerik.Web.UI;

public partial class UserControls_Menu_IPhoneMenu_VerticalMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //загрузить верхний уровень меню
        if (!IsPostBack)
        {
            RadMenu1.Items.Clear();
            RadMenu1.Items.AddRange(DbConnMenu.LoadTopLevelMenu());
        }
    }

    protected void RadMenu1_ItemClick(object sender, RadMenuEventArgs e)
    {
        //Response.Redirect(e.Item.NavigateUrl);
    } 

}

public class IPhoneItem : ITemplate
{
    System.Web.UI.WebControls.Panel pnlImage = new System.Web.UI.WebControls.Panel();
    System.Web.UI.WebControls.Label lblCaption = new System.Web.UI.WebControls.Label();

    public void InstantiateIn(System.Web.UI.Control container)
    {
        pnlImage.ID = "pnlImage";
        lblCaption.Text = "Hello";
        lblCaption.DataBinding += new EventHandler(lblCaption_DataBinding);

        //txt.ImageUrl = "~/UserControls/Menu/OpenedModulesMenu/images/arrowShowMenu.png";
        lblCaption.ID = "lblCaption";
        container.Controls.Add(pnlImage);
        container.Controls.Add(lblCaption);        
    }

    public string Caption
    {
        get { return lblCaption.Text; }
        set { lblCaption.Text = value; }
    }

    private void lblCaption_DataBinding(object sender, EventArgs e)
    {
        Button target = (Button)sender;
        RadMenuItem item = (RadMenuItem)target.BindingContainer;

        string itemText = (string)DataBinder.Eval(item, "Text");
        target.Text = itemText;
    }

}
